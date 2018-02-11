using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LykkeAutomationPrivate.DataGenerators
{
    public static class AesUtils
    {
        public static string Encrypt(string key, string password)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            if (keyBytes.Length > 64)
                throw new Exception("Dont know what to do: key lenght is more than 64 bytes");
            var keyData = new byte[64];
            keyBytes.CopyTo(keyData, 0);
            var passData = ConvertToBytes(password);
            var encryptedKeyData = Encrypt(keyData, passData);

            return ToHexString(encryptedKeyData);
        }

        public static string Decrypt(string encodedKey, string password)
        {
            var keyData = FromHexString(encodedKey);
            var passData = ConvertToBytes(password);

            var decodedKey = Decrypt(keyData, passData);
            return System.Text.Encoding.ASCII.GetString(decodedKey).TrimEnd('\0');
        }

        private static byte[] Encrypt(byte[] data, byte[] password)
        {
            using (AesCryptoServiceProvider csp = CreateProvider(password))
            {
                ICryptoTransform encrypter = csp.CreateEncryptor();
                return encrypter.TransformFinalBlock(data, 0, data.Length);
            }
        }

        private static byte[] Decrypt(byte[] data, byte[] password)
        {
            using (AesCryptoServiceProvider csp = CreateProvider(password))
            {
                ICryptoTransform decrypter = csp.CreateDecryptor();
                return decrypter.TransformFinalBlock(data, 0, data.Length);
            }
        }

        private static AesCryptoServiceProvider CreateProvider(byte[] password)
        {
            return new AesCryptoServiceProvider
            {
                KeySize = 128,
                BlockSize = 128,
                Key = password,
                Padding = PaddingMode.None,
                Mode = CipherMode.ECB
            };
        }

        private static byte[] ConvertToBytes(string password)
        {
            string encrtptionPhrase = String.Empty;
            if (password.Length > 16)
            {
                encrtptionPhrase = password.Remove(16);
            }
            else if (password.Length < 16)
            {
                encrtptionPhrase = password + new string(' ', 16 - password.Length);
            }
            else
            {
                encrtptionPhrase = password;
            }

            return Encoding.UTF8.GetBytes(encrtptionPhrase);
        }

        private static string ToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static byte[] FromHexString(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                 .Where(x => x % 2 == 0)
                 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                 .ToArray();
        }
    }
}
