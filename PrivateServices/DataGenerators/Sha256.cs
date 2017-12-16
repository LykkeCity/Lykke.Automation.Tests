using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LykkeAutomationPrivate.DataGenerators
{
    public static class Sha256
    {
        public static string GenerateHash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
