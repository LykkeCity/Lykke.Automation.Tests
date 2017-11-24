using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return string.Format("{0}-{1}-{2}_{3}-{4}-{5}-{6}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
        }
    }
}
