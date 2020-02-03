#!/bin/bash

set -e

gpg --batch --passphrase $SECURE -o W.tar -d W.tar.enc
tar -xf W.tar