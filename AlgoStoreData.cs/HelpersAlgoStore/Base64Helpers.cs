using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.HelpersAlgoStore
{
    public class Base64Helpers
    {
        public static string EncodeToBase64(string stringToConvert)
        {
            byte[] encodedBytes = Encoding.UTF8.GetBytes(stringToConvert);
            return Convert.ToBase64String(encodedBytes);
        }

        public static string DecodeBase64String(string base64String)
        {
            byte[] decodedBytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}
