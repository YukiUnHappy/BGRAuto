using System;
using System.Security.Cryptography;
using System.Text;

namespace FY
{
    // Token: 0x02000313 RID: 787
    public static class Utility
    {
        // Token: 0x06000397 RID: 919 RVA: 0x0000CB7C File Offset: 0x0000AD7C
        static Utility()
        {
            // Note: this type is marked as 'beforefieldinit'.
        }

        public static string GetPackName(string assetBundleKey)
        {
            return Utility.GetSHA256Hash(Utility.GetBase64Encode(assetBundleKey));
        }

        // Token: 0x06000360 RID: 864 RVA: 0x0000C381 File Offset: 0x0000A581
        public static void CheckAddVal(ref int val, int addVal)
        {
            Utility.CheckAddVal(ref val, addVal, -2147483648, 2147483647);
        }

        // Token: 0x06000362 RID: 866 RVA: 0x0000C3B5 File Offset: 0x0000A5B5
        public static void CheckAddVal(ref int val, uint addVal)
        {
            Utility.CheckAddVal(ref val, addVal, 2147483647);
        }

        // Token: 0x06000365 RID: 869 RVA: 0x0000C430 File Offset: 0x0000A630
        public static void CheckAddVal(ref uint val, int addVal)
        {
            Utility.CheckAddVal(ref val, addVal, 0u, 4294967295u);
        }

        // Token: 0x06000367 RID: 871 RVA: 0x0000C452 File Offset: 0x0000A652
        public static void CheckAddVal(ref uint val, uint addVal)
        {
            Utility.CheckAddVal(ref val, addVal, 4294967295u);
        }

        // Token: 0x06000361 RID: 865 RVA: 0x0000C394 File Offset: 0x0000A594
        public static void CheckAddVal(ref int val, uint addVal, int max)
        {
            if (addVal > 0u)
            {
                if ((long)val + (long)((ulong)addVal) < (long)max)
                {
                    val = Convert.ToInt32((long)val + (long)((ulong)addVal));
                    return;
                }
                val = max;
            }
        }

        // Token: 0x06000366 RID: 870 RVA: 0x0000C43B File Offset: 0x0000A63B
        public static void CheckAddVal(ref uint val, uint addVal, uint max)
        {
            if (addVal > 0u)
            {
                if (val + addVal < max)
                {
                    val += addVal;
                    return;
                }
                val = max;
            }
        }

        // Token: 0x0600035F RID: 863 RVA: 0x0000C354 File Offset: 0x0000A554
        public static void CheckAddVal(ref int val, int addVal, int min, int max)
        {
            if (addVal <= 0)
            {
                if (addVal < 0)
                {
                    if (val + addVal > min)
                    {
                        val += addVal;
                        return;
                    }
                    val = min;
                }
                return;
            }
            if (val + addVal < max)
            {
                val += addVal;
                return;
            }
            val = max;
        }

        // Token: 0x06000364 RID: 868 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
        public static void CheckAddVal(ref uint val, int addVal, uint min, uint max)
        {
            if (addVal <= 0)
            {
                if (addVal < 0)
                {
                    if ((ulong)val + (ulong)((long)addVal) > (ulong)min)
                    {
                        val = Convert.ToUInt32((long)((ulong)val + (ulong)((long)addVal)));
                        return;
                    }
                    val = min;
                }
                return;
            }
            if ((ulong)val + (ulong)((long)addVal) < (ulong)max)
            {
                val = Convert.ToUInt32((long)((ulong)val + (ulong)((long)addVal)));
                return;
            }
            val = max;
        }

        // Token: 0x06000369 RID: 873 RVA: 0x0000C473 File Offset: 0x0000A673
        public static void CheckCostVal(ref uint val, uint costVal)
        {
            Utility.CheckCostVal(ref val, costVal, 0u);
        }

        // Token: 0x06000363 RID: 867 RVA: 0x0000C3C3 File Offset: 0x0000A5C3
        public static void CheckCostVal(ref int val, uint costVal, int min)
        {
            if (costVal > 0u)
            {
                if ((long)val - (long)((ulong)costVal) > (long)min)
                {
                    val = Convert.ToInt32((long)val - (long)((ulong)costVal));
                    return;
                }
                val = min;
            }
        }

        // Token: 0x06000368 RID: 872 RVA: 0x0000C45C File Offset: 0x0000A65C
        public static void CheckCostVal(ref uint val, uint costVal, uint min)
        {
            if (costVal > 0u)
            {
                if (val - costVal > min)
                {
                    val -= costVal;
                    return;
                }
                val = min;
            }
        }

        // Token: 0x0600035B RID: 859 RVA: 0x0000C2D4 File Offset: 0x0000A4D4
        public static float Clamp(this float val, float x, float y)
        {
            if (x < y)
            {
                if (val < x)
                {
                    return x;
                }
                if (val > y)
                {
                    return y;
                }
            }
            else
            {
                if (val < y)
                {
                    return y;
                }
                if (val > x)
                {
                    return x;
                }
            }
            return val;
        }

        // Token: 0x0600035D RID: 861 RVA: 0x0000C314 File Offset: 0x0000A514
        public static int Clamp(this int val, int x, int y)
        {
            if (x < y)
            {
                if (val < x)
                {
                    return x;
                }
                if (val > y)
                {
                    return y;
                }
            }
            else
            {
                if (val < y)
                {
                    return y;
                }
                if (val > x)
                {
                    return x;
                }
            }
            return val;
        }

        // Token: 0x0600035A RID: 858 RVA: 0x0000C2C2 File Offset: 0x0000A4C2
        public static float Clamp01(this float val)
        {
            return val.Clamp(0f, 1f);
        }

        // Token: 0x06000381 RID: 897 RVA: 0x0000C7AC File Offset: 0x0000A9AC
        public static int CumulativeDistribution(int[] nProbS)
        {
            if (nProbS == null || nProbS.Length == 0)
            {
                return -1;
            }
            if (nProbS.Length == 1)
            {
                return 0;
            }
            int num = 0;
            int[] array = new int[nProbS.Length];
            for (int i = 0; i < nProbS.Length; i++)
            {
                num += nProbS[i];
                array[i] = num;
            }
            if (num < 1)
            {
                return 0;
            }
            int randomInt = Utility.GetRandomInt(1, num);
            for (int j = 0; j < array.Length; j++)
            {
                if (randomInt <= array[j])
                {
                    return j;
                }
            }
            return array.Length - 1;
        }

        // Token: 0x06000382 RID: 898 RVA: 0x0000C820 File Offset: 0x0000AA20
        public static int CumulativeDistribution(this Random r, int[] nProbS)
        {
            if (nProbS == null || nProbS.Length == 0)
            {
                return -1;
            }
            if (nProbS.Length == 1)
            {
                return 0;
            }
            int num = 0;
            int[] array = new int[nProbS.Length];
            for (int i = 0; i < nProbS.Length; i++)
            {
                num += nProbS[i];
                array[i] = num;
            }
            if (num < 1)
            {
                return 0;
            }
            int randomInt = r.GetRandomInt(1, num);
            for (int j = 0; j < array.Length; j++)
            {
                if (randomInt <= array[j])
                {
                    return j;
                }
            }
            return array.Length - 1;
        }

        // Token: 0x0600036F RID: 879 RVA: 0x0000C591 File Offset: 0x0000A791
        public static float FloatTwo(float fNum)
        {
            return (float)Utility.ToInt(fNum * 100f) / 100f;
        }

        // Token: 0x06000370 RID: 880 RVA: 0x0000C5A6 File Offset: 0x0000A7A6
        public static float FloatTwo(double dNum)
        {
            return (float)Utility.ToInt(dNum * 100.0) / 100f;
        }

        // Token: 0x0600036B RID: 875 RVA: 0x0000C4A4 File Offset: 0x0000A6A4
        public static string GetBase64Decode(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            return Encoding.ASCII.GetString(bytes);
        }

        // Token: 0x0600036A RID: 874 RVA: 0x0000C480 File Offset: 0x0000A680
        public static string GetBase64Encode(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        // Token: 0x0600036C RID: 876 RVA: 0x0000C4C8 File Offset: 0x0000A6C8
        public static string GetMD5Hash(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] value = MD5.Create().ComputeHash(bytes);
            return BitConverter.ToString(value).Replace("-", "").ToLower();
        }

        // Token: 0x0600038B RID: 907 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
        public static float GetNormalDistribution(float k, float K)
        {
            double num = Math.Sinh((double)k);
            double num2 = Math.Sinh((double)Utility.GetRandomFloat(k, K));
            return (float)(num2 - num) / (float)(Math.Sinh((double)K) - num);
        }

        // Token: 0x0600038C RID: 908 RVA: 0x0000CA08 File Offset: 0x0000AC08
        public static float GetNormalDistribution(this Random r, float k, float K)
        {
            double num = Math.Sinh((double)k);
            double num2 = Math.Sinh((double)r.GetRandomFloat(k, K));
            return (float)(num2 - num) / (float)(Math.Sinh((double)K) - num);
        }

        // Token: 0x0600038D RID: 909 RVA: 0x0000CA3B File Offset: 0x0000AC3B
        public static float GetNormalDistribution(float k, float K, float a, float b)
        {
            return a + Utility.GetNormalDistribution(k, K) * (b - a);
        }

        // Token: 0x0600038E RID: 910 RVA: 0x0000CA4A File Offset: 0x0000AC4A
        public static float GetNormalDistribution(this Random r, float k, float K, float a, float b)
        {
            return a + r.GetNormalDistribution(k, K) * (b - a);
        }

        // Token: 0x0600037F RID: 895 RVA: 0x0000C78D File Offset: 0x0000A98D
        public static bool GetRandomBool()
        {
            return Utility.GetRandomInt(0, 1) != 0;
        }

        // Token: 0x06000380 RID: 896 RVA: 0x0000C79B File Offset: 0x0000A99B
        public static bool GetRandomBool(this Random r)
        {
            return r.GetRandomInt(0, 1) != 0;
        }

        // Token: 0x06000387 RID: 903 RVA: 0x0000C938 File Offset: 0x0000AB38
        public static double GetRandomDouble()
        {
            double result;
            lock (Utility.syncObj)
            {
                result = Utility.random.NextDouble();
            }
            return result;
        }

        // Token: 0x06000388 RID: 904 RVA: 0x0000C978 File Offset: 0x0000AB78
        public static double GetRandomDouble(this Random r)
        {
            return r.NextDouble();
        }

        // Token: 0x06000389 RID: 905 RVA: 0x0000C980 File Offset: 0x0000AB80
        public static double GetRandomDouble(double a, double b)
        {
            double result;
            lock (Utility.syncObj)
            {
                result = a + Utility.random.NextDouble() * (b - a);
            }
            return result;
        }

        // Token: 0x0600038A RID: 906 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
        public static double GetRandomDouble(this Random r, double a, double b)
        {
            return a + r.NextDouble() * (b - a);
        }

        // Token: 0x06000383 RID: 899 RVA: 0x0000C894 File Offset: 0x0000AA94
        public static float GetRandomFloat()
        {
            float result;
            lock (Utility.syncObj)
            {
                result = (float)Utility.random.NextDouble();
            }
            return result;
        }

        // Token: 0x06000384 RID: 900 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
        public static float GetRandomFloat(this Random r)
        {
            return (float)r.NextDouble();
        }

        // Token: 0x06000385 RID: 901 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
        public static float GetRandomFloat(float a, float b)
        {
            float result;
            lock (Utility.syncObj)
            {
                result = a + (float)Utility.random.NextDouble() * (b - a);
            }
            return result;
        }

        // Token: 0x06000386 RID: 902 RVA: 0x0000C928 File Offset: 0x0000AB28
        public static float GetRandomFloat(this Random r, float a, float b)
        {
            return a + (float)r.NextDouble() * (b - a);
        }

        // Token: 0x06000379 RID: 889 RVA: 0x0000C6A8 File Offset: 0x0000A8A8
        public static int GetRandomInt()
        {
            int result;
            lock (Utility.syncObj)
            {
                result = Utility.random.Next();
            }
            return result;
        }

        // Token: 0x0600037A RID: 890 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
        public static int GetRandomInt(this Random r)
        {
            return r.Next();
        }

        // Token: 0x0600037B RID: 891 RVA: 0x0000C6F0 File Offset: 0x0000A8F0
        public static int GetRandomInt(int n)
        {
            if (n == 0)
            {
                return 0;
            }
            int result;
            lock (Utility.syncObj)
            {
                result = Utility.random.Next() % (n + 1);
            }
            return result;
        }

        // Token: 0x0600037C RID: 892 RVA: 0x0000C738 File Offset: 0x0000A938
        public static int GetRandomInt(this Random r, int n)
        {
            if (n == 0)
            {
                return 0;
            }
            return r.Next() % (n + 1);
        }

        // Token: 0x0600037D RID: 893 RVA: 0x0000C749 File Offset: 0x0000A949
        public static int GetRandomInt(int lower, int upper)
        {
            if (lower == upper)
            {
                return lower;
            }
            if (lower > upper)
            {
                return upper + Utility.GetRandomInt(lower - upper);
            }
            return lower + Utility.GetRandomInt(upper - lower);
        }

        // Token: 0x0600037E RID: 894 RVA: 0x0000C76A File Offset: 0x0000A96A
        public static int GetRandomInt(this Random r, int lower, int upper)
        {
            if (lower == upper)
            {
                return lower;
            }
            if (lower > upper)
            {
                return upper + r.GetRandomInt(lower - upper);
            }
            return lower + r.GetRandomInt(upper - lower);
        }

        // Token: 0x0600036D RID: 877 RVA: 0x0000C50C File Offset: 0x0000A70C
        public static string GetSHA1Hash(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] value = SHA1.Create().ComputeHash(bytes);
            return BitConverter.ToString(value).Replace("-", "").ToLower();
        }

        // Token: 0x0600036E RID: 878 RVA: 0x0000C550 File Offset: 0x0000A750
        public static string GetSHA256Hash(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] value = SHA256.Create().ComputeHash(bytes);
            return BitConverter.ToString(value).Replace("-", "").ToLower();
        }

        // Token: 0x06000394 RID: 916 RVA: 0x0000CB64 File Offset: 0x0000AD64
        public static bool hasFlag(this uint flag, uint mask)
        {
            return (flag & mask) != 0u;
        }

        // Token: 0x0600035C RID: 860 RVA: 0x0000C2F3 File Offset: 0x0000A4F3
        public static bool InRange(this float val, float x, float y)
        {
            if (x < y)
            {
                return x <= val && val <= y;
            }
            return y <= val && val <= x;
        }

        // Token: 0x0600035E RID: 862 RVA: 0x0000C333 File Offset: 0x0000A533
        public static bool InRange(this int val, int x, int y)
        {
            if (x < y)
            {
                return x <= val && val <= y;
            }
            return y <= val && val <= x;
        }

        // Token: 0x06000393 RID: 915 RVA: 0x0000CB5A File Offset: 0x0000AD5A
        public static long Sec2Tick(long sec)
        {
            return sec * 10000000L;
        }

        // Token: 0x06000395 RID: 917 RVA: 0x0000CB6F File Offset: 0x0000AD6F
        public static uint setFlag(this uint flag, uint mask)
        {
            return flag | mask;
        }

        // Token: 0x06000392 RID: 914 RVA: 0x0000CB50 File Offset: 0x0000AD50
        public static long Tick2Sec(long tick)
        {
            return tick / 10000000L;
        }

        // Token: 0x0600038F RID: 911 RVA: 0x0000CA5C File Offset: 0x0000AC5C
        public static long Time()
        {
            return (DateTime.UtcNow.Ticks - Utility.g_StartTime.Ticks) / 10000000L;
        }

        // Token: 0x06000371 RID: 881 RVA: 0x0000C5BF File Offset: 0x0000A7BF
        public static int ToInt(float fTmp)
        {
            return Utility.ToInt(fTmp, 10000);
        }

        // Token: 0x06000373 RID: 883 RVA: 0x0000C5F8 File Offset: 0x0000A7F8
        public static int ToInt(double dTmp)
        {
            return Utility.ToInt(dTmp, 10000);
        }

        // Token: 0x06000372 RID: 882 RVA: 0x0000C5CC File Offset: 0x0000A7CC
        public static int ToInt(float fTmp, int MultiplyBase)
        {
            long num = (long)((double)fTmp * (double)MultiplyBase);
            if (num % (long)MultiplyBase == (long)(MultiplyBase - 1))
            {
                return (int)((num + 1L) / (long)MultiplyBase);
            }
            return (int)(num / (long)MultiplyBase);
        }

        // Token: 0x06000374 RID: 884 RVA: 0x0000C608 File Offset: 0x0000A808
        public static int ToInt(double dTmp, int MultiplyBase)
        {
            long num = (long)(dTmp * (double)MultiplyBase);
            if (num % (long)MultiplyBase == (long)(MultiplyBase - 1))
            {
                return (int)((num + 1L) / (long)MultiplyBase);
            }
            return (int)(num / (long)MultiplyBase);
        }

        // Token: 0x06000375 RID: 885 RVA: 0x0000C633 File Offset: 0x0000A833
        public static uint ToUInt(float fTmp)
        {
            return Utility.ToUInt(fTmp, 10000);
        }

        // Token: 0x06000377 RID: 887 RVA: 0x0000C66C File Offset: 0x0000A86C
        public static uint ToUInt(double dTmp)
        {
            return Utility.ToUInt(dTmp, 10000);
        }

        // Token: 0x06000376 RID: 886 RVA: 0x0000C640 File Offset: 0x0000A840
        public static uint ToUInt(float fTmp, int MultiplyBase)
        {
            long num = (long)((double)fTmp * (double)MultiplyBase);
            if (num % (long)MultiplyBase == (long)(MultiplyBase - 1))
            {
                return (uint)((num + 1L) / (long)MultiplyBase);
            }
            return (uint)(num / (long)MultiplyBase);
        }

        // Token: 0x06000378 RID: 888 RVA: 0x0000C67C File Offset: 0x0000A87C
        public static uint ToUInt(double dTmp, int MultiplyBase)
        {
            long num = (long)(dTmp * (double)MultiplyBase);
            if (num % (long)MultiplyBase == (long)(MultiplyBase - 1))
            {
                return (uint)((num + 1L) / (long)MultiplyBase);
            }
            return (uint)(num / (long)MultiplyBase);
        }

        // Token: 0x06000396 RID: 918 RVA: 0x0000CB74 File Offset: 0x0000AD74
        public static uint unsetFlag(this uint flag, uint mask)
        {
            return flag & ~mask;
        }

        // Token: 0x17000030 RID: 48
        public static long lUnique
        {
            // Token: 0x06000391 RID: 913 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
            get
            {
                long result;
                lock (Utility.syncUnique)
                {
                    Utility.lUniqueCnt += 1L;
                    if ((Utility.lCreateTick / 65536L + Utility.lUniqueCnt) * 65536L < Utility.Tick)
                    {
                        Utility.lCreateTick = Utility.Tick;
                        Utility.lUniqueCnt = 0L;
                    }
                    result = (Utility.lCreateTick / 65536L + Utility.lUniqueCnt) * 65536L + (long)((ulong)Utility.m_cSetID * 256uL) + (long)((ulong)Utility.m_cSubSet);
                }
                return result;
            }
        }

        // Token: 0x1700002F RID: 47
        public static long Tick
        {
            // Token: 0x06000390 RID: 912 RVA: 0x0000CA88 File Offset: 0x0000AC88
            get
            {
                return DateTime.UtcNow.Ticks - Utility.g_StartTime.Ticks;
            }
        }

        // Token: 0x04000B8B RID: 2955
        public static DateTime g_StartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        // Token: 0x04000B8A RID: 2954
        public static readonly long InitTicks = DateTime.Now.Ticks;

        // Token: 0x04000B8F RID: 2959
        private static long lCreateTick = Utility.Tick;

        // Token: 0x04000B8E RID: 2958
        private static long lUniqueCnt = 0L;

        // Token: 0x04000B85 RID: 2949
        private const long MULTIPLY_SET_ID = 256L;

        // Token: 0x04000B86 RID: 2950
        private const long MULTIPLY_SUB_SET = 256L;

        // Token: 0x04000B8C RID: 2956
        public static byte m_cSetID = 0;

        // Token: 0x04000B8D RID: 2957
        public static byte m_cSubSet = 0;

        // Token: 0x04000B89 RID: 2953
        private static readonly Random random = new Random();

        // Token: 0x04000B88 RID: 2952
        private static readonly object syncObj = new object();

        // Token: 0x04000B90 RID: 2960
        private static readonly object syncUnique = new object();

        // Token: 0x04000B87 RID: 2951
        private const long TOTAL_MUTIPLY_BASE = 65536L;
    }
}
