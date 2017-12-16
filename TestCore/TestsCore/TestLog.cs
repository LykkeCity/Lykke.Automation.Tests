using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LykkeAutomation.TestsCore
{
    public static class TestLog
    {
        private static Dictionary<string, StringBuilder> _log = new Dictionary<string, StringBuilder>();
        private static Dictionary<string, StringBuilder> _stepLog = new Dictionary<string, StringBuilder>();

        #region TestLog functions

        public static void Write(string value)
        {
            try
            {
                TestContext.Out?.Write(GetTimeStamp() + value);
                SaveLogToDictionary(value);
            }
            catch { }
        }

        public static void Write(string format, params object[] args)
        {
            try
            {
                TestContext.Out?.Write(GetTimeStamp() + String.Format(format, args));
                SaveLogToDictionary(String.Format(format, args));
            }
            catch { }
        }

        public static void WriteLine(string value)
        {
            try
            {
                TestContext.Out?.WriteLine(GetTimeStamp() + "\r\n" + value);
                SaveLogToDictionary(value + '\n');
            }
            catch { }
        }

        public static void WriteStep(string value)
        {
            try
            {
                TestContext.Out?.WriteLine($"Step: <{value}>" + "\r\n");
                SaveLogToDictionary($"Step: <{value}>" + "\r\n");
            }
            catch { }
        }

        public static void WriteLine(string format, params object[] args)
        {
            try
            {
                TestContext.Out?.WriteLine(GetTimeStamp() + String.Format(format, args));
                SaveLogToDictionary(String.Format(format, args) + '\n');
            }
            catch { }
        }

        public static void WriteException(Exception exception)
        {
            try
            {
                string e = exception.Message + '\n' + exception.StackTrace + '\n';
                TestContext.Out?.Write(GetTimeStamp() + e);
                SaveLogToDictionary(e);
            }
            catch { }
        }
        public static void WriteException(Exception exception, string description)
        {
            try
            {
                string e = description + '\n' + exception.Message + '\n' + exception.StackTrace + '\n';
                TestContext.Out?.Write(GetTimeStamp() + e);
                SaveLogToDictionary(e);
            }
            catch { }
        }

        #endregion

        private static string GetTimeStamp() => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] ";

        static private void SaveLogToDictionary(string log)
        {
            string key = GetKey();
            if (!_log.ContainsKey(key))
            {
                _log.Add(key, new StringBuilder(log));
            }
            else
            {
                _log[key].Append(log);
            }

            if (!_stepLog.ContainsKey(key))
            {
                _stepLog.Add(key, new StringBuilder(log));
            }
            else
            {
                _stepLog[key].Append(log);
            }
        }

        public static string GetLog()
        {
            string key = GetKey();
            if (_log.ContainsKey(key))
            {
                string log = _log[key].ToString();
                _log.Remove(key);
                return log;
            }
            return "";
        }

        public static string GetStepLog()
        {
            string key = GetKey();
            if (_stepLog.ContainsKey(key))
            {
                string log = _stepLog[key].ToString();
                _stepLog[key].Clear();
                return log;
            }
            return "";
        }

        private static string GetKey()
        {
            try
            {
                return TestContext.CurrentContext.Test.FullName;
            }
            catch
            {
                return "UMT";
            }
        }

        public static class Debug
        {
            public static void WriteLine(string value)
            {
                    TestContext.Progress.WriteLine(DateTime.Now.ToLongTimeString() + ": " + value);
            }

            public static void Write(string value)
            {
                    TestContext.Progress.Write(DateTime.Now.ToLongTimeString() + ": " + value);
            }
        }
    }
}
