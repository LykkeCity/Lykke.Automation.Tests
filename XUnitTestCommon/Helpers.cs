using System;
using System.Collections.Generic;
using System.Linq;

namespace XUnitTestCommon
{
    public class Helpers
    {
        public static Dictionary<string, string> EmptyDictionary = new Dictionary<string, string>();
        public static Random Random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static string GenerateTimeStamp()
        {
            var now = DateTime.Now;
            return string.Format("{0}-{1}-{2}_{3}-{4}", now.Year, now.Month, now.Day, now.Hour, now.Minute);
        }

        public static string GetFullTimestamp()
        {
            return DateTime.Now.ToString("s");
        }

        public static string GetFullUtcTimestamp()
        {
            return DateTime.UtcNow.ToString("s");
        }

        public static string GetTimestampIso8601()
        {
            return DateTime.UtcNow.ToString(GlobalConstants.ISO_8601_DATE_FORMAT);
        }
    }
}
