using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Downloader.Core.Utils;
using FY;

namespace BGRAuto
{
    class Program
    {
        private const string CdnBase = "http://ps1.str.cloudn-service.com/android_res/";
        private const string Version = CdnBase + "version/version-android.xml";

        private static readonly byte[] _aesKey = Convert.FromBase64String("ExmGM04QVp52sgGnwCwoTO+imSnWTXg5y+QrpVBCXNs=");
        private static readonly byte[] _aesIv = Convert.FromBase64String("2vYybHoCnSa9SD8Nu6ssAA==");

        private static DownHelper dh;
        private static List<(string key, string ver)> _list;

        private static readonly SemaphoreSlim Sign = new SemaphoreSlim(0);

        static void Main(string[] args)
        {
            dh = new DownHelper(30);
#if DEBUG
            dh.Proxy = new WebProxy("127.0.0.1", 1081);
#endif
            dh.DownloadFileCompletedEvent += DhOnDownloadFileCompletedEvent;
            dh.DownloadFileCancelledEvent += (sender, exception, url, path) => Environment.Exit(1);

            Compare();
            Download();
            Sign.Wait();

            Dump();
            Format();
            Upload();
        }

        private static void Dump()
        {
            var process = Process.Start("BraveAuto.exe");
            process?.WaitForExit();
        }

        private static void Upload()
        {
            if (File.Exists("upload.sh"))
            {
                var process = Process.Start("bash", "upload.sh");
                process?.WaitForExit();
            }
        }

        private static void Format()
        {
            var directories = Directory.GetDirectories(".", "UIAtlas", SearchOption.AllDirectories);
            foreach (var directory in directories)
            {
                var isCG = directory.Contains("DIALOGBG");

                var id = Path.GetFileName(Path.GetDirectoryName(directory));

                var path = $"Output/{id}";
                if (id.StartsWith("60"))
                    path = "Output/SKIN";
                else if (id.StartsWith("9"))
                    path = "Output/MISC";
                else if (id.StartsWith("50"))
                    continue;

                EnsureDir(path);

                if (isCG)
                    foreach (var file in Directory.GetFiles(directory))
                        File.Move(file, $"{path}/{Path.GetFileName(file)}");
                else
                    File.Move($"{directory}/{id}.png", $"{path}/{id}.png");
            }

            const string set = "setting/JP.unity3d";

            if (File.Exists(set))
                File.Move(set, $"Output/JP-{_list.Find(s => s.key == "setting/JP").ver}.unity3d");
        }

        private static void EnsureDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static void DhOnDownloadFileCompletedEvent(object sender, string url, string path)
        {
            if (path.EndsWith(".unity3d"))
            {
                var d = File.ReadAllBytes(path);
                d = DecryptBytes(d);
                File.WriteAllBytes(path, d);
            }

            Console.WriteLine($@"{(dh.WrongNum == 0 ? "" : dh.WrongNum + "/")}{dh.FinishedNum}/{dh.TotalNum}{
                (dh.FinishedNum + dh.WrongNum == dh.TotalNum ? " FN" : "")}");

            if (dh.FinishedNum + dh.WrongNum == dh.TotalNum)
                Sign.Release();
        }

        public static byte[] DecryptBytes(byte[] message)
        {
            if (message == null || message.Length == 0)
                return message;

            byte[] result;
            using (var aes = Aes.Create())
            {
                aes.Key = _aesKey;
                aes.IV = _aesIv;
                using var memoryStream = new MemoryStream();
                using var cryptoTransform = aes.CreateDecryptor();
                using var cryptoStream =
                    new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(message, 0, message.Length);
                cryptoStream.FlushFinalBlock();
                result = memoryStream.ToArray();
            }

            return result;
        }

        private static void Download()
        {
            foreach (var (key, _) in _list)
            {
                dh.Add($"{CdnBase}android/{Utility.GetPackName(key)}", $"{key}.unity3d");
            }

            if (dh.Queue.Count > 0)
                dh.StartDownQueue();
            else
                Sign.Release();
        }

        private static void Compare()
        {
            if (File.Exists("download.sh"))
            {
                var process = Process.Start("bash", "download.sh");
                process?.WaitForExit();
            }

            dh.DownloadSingle(Version, "version-android.xml");

            var d1 = GetRoot("version-android.xml");
            var d2 = GetRoot("version-android2.xml");

            _list = d1.Elements("version").Except(d2.Elements("version"), new VersionComparer())
                .AsParallel()
                .Select(s => (key: s.Attribute("asset_key")?.Value, ver: s.Attribute("asset_ver")?.Value))
                .Where(s => !s.key.Contains("/VOICE/"))
                .Where(s => !s.key.Contains("/SKILL/"))
                .Where(s => !s.key.Contains("/ICON/"))
                .Where(s => !s.key.Contains("/ATLAS/"))
                .Where(s => !s.key.Contains("/TUTORIAL/"))
                .Where(s => !s.key.Contains("dialog"))
                .Where(s => !s.key.Contains("heroicon"))
                .Where(s => !s.key.Contains("APK"))
                .ToList();
        }

        private static XElement GetRoot(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);
            var xe = doc.DocumentElement;

            var atlas = new Dictionary<string, string>();
            var define = new Dictionary<string, string>();
            var xnl = xe.SelectNodes("/root/ALIAS");
            if (xnl.Count > 0)
                foreach (XmlNode item in xnl)
                {
                    foreach (XmlAttribute item2 in item.Attributes)
                        atlas.Add(item2.Value, item2.Name);
                    xe.RemoveChild(item);
                }
            xnl = xe.SelectNodes("/root/DEFINE");
            if (xnl.Count > 0)
                foreach (XmlNode item in xnl)
                {
                    foreach (XmlAttribute item2 in item.Attributes)
                        define.Add(item2.Name, item2.Value);
                    xe.RemoveChild(item);
                }

            var temp = xe.OuterXml;
            foreach (var (key, value) in atlas)
            {
                temp = temp.Replace($"{key}=", $"{value}=");
                temp = temp.Replace($"{key.ToLower()}=", $"{value}=");
            }

            foreach (var (key, value) in define)
            {
                temp = temp.Replace($"\"{key}\"", $"\"{value}\"");
                temp = temp.Replace($"\"{key.ToLower()}\"", $"\"{value}\"");
            }

            var xd = XDocument.Parse(temp);
            return xd.Root;
        }

        private class VersionComparer : IEqualityComparer<XElement>
        {
            public bool Equals(XElement x, XElement y)
            {
                //Check whether the compared objects reference the same data.
                if (object.ReferenceEquals(x, y)) return true;

                //Check whether any of the compared objects is null.
                if (x is null || y is null)
                    return false;

                var xk = x.Attribute("asset_key").Value;
                var xv = x.Attribute("asset_ver").Value;

                var yk = y.Attribute("asset_key").Value;
                var yv = y.Attribute("asset_ver").Value;

                //Check whether the versions' properties are equal.
                return xk == yk && xv == yv;
            }

            // If Equals() returns true for a pair of objects 
            // then GetHashCode() must return the same value for these objects.

            public int GetHashCode(XElement versionElement)
            {
                if (!versionElement.HasAttributes)
                    return 0;

                var k = versionElement.Attribute("asset_key").Value;
                var v = versionElement.Attribute("asset_ver").Value;

                return $"{k}_{v}".GetHashCode();
            }
        }
    }
}
