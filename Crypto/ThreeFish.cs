﻿using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace CourseProject.Crypto
{
    public class ThreeFish
    {
        private static readonly ThreefishEngine engine = new(256);
        private static readonly PaddedBufferedBlockCipher engineMode = new(new EcbBlockCipher(engine), new Pkcs7Padding());

        public static byte[]? Encrypt(byte[] data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            engineMode.Init(true, new KeyParameter(keyBytes));
            return Process(data);
        }

        public static byte[]? Decrypt(byte[] data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            engineMode.Init(false, new KeyParameter(keyBytes));
            return Process(data);
        }

        private static byte[]? Process(byte[] data)
        {
            try
            {
                int outputSize = engineMode.GetOutputSize(data.Length);
                byte[] processedData = new byte[outputSize];
                int processResutl = engineMode.ProcessBytes(data, 0, data.Length, processedData, 0);
                engineMode.DoFinal(processedData, processResutl);

                return processedData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
