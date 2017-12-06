using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Framework;
using Allure.Commons;

namespace XUnitTestCommon.Reports
{
    public class AllureReport
    {
        private static AllureReport allureReport;
        private static AllureLifecycle _lifecycle;
        private static Dictionary<string, List<dynamic>> _caseStorage = new Dictionary<string, List<dynamic>>();
        private object debugLock = new object();
        private static object myLock = new object();
        protected Exception result;

        private AllureReport()
        {
        }

        public static AllureReport GetInstance()
        {
            if (allureReport == null)
            {
                lock (myLock)
                {
                    if (allureReport == null)
                        allureReport = new AllureReport();
                }
            }
            return allureReport;
        }

        public void RunStarted(string workDirectory)
        {
            _lifecycle = AllureLifecycle.Instance;
        }

        public void CaseStarted(string fullName, string name, string description)
        {
            string fixtureName = GetFixtureName(fullName);
            lock (_caseStorage)
            {
                _lifecycle.StartTestCase(new TestResult() { uuid = fixtureName, name = name, description = description });
            }
        }

        public void CaseFinished(string fullName, TestStatus result, Exception exception)
        {
            lock (_caseStorage)
            {
                string fixtureName = GetFixtureName(fullName);

                List<Attachment> attaches = new List<Attachment>();
                var testLogPath = TestContext.CurrentContext.WorkDirectory + $"/allure-results/Test_{Helpers.GenerateTimeStamp()}.log";
                var log = Logger.GetLog();
                File.WriteAllText(testLogPath, log);
                attaches.Add(new Attachment() { name = "TestLog", source = testLogPath, type = "application/json" });


                if (result == TestStatus.Failed)
                {

                    AssertionException ex = new AssertionException(TestContext.CurrentContext.Result.Message);
                    string st = TestContext.CurrentContext.Result.Assertions.ToList().Count == 0 ?
                        "" :
                        TestContext.CurrentContext.Result.Assertions.ToList()?[0].StackTrace;

                    _lifecycle.StopTestCase(x => { x.uuid = fixtureName; x.status = Status.failed; x.attachments = attaches; x.statusDetails = new StatusDetails() { message = ex.Message, trace = st }; });
                    _lifecycle.WriteTestCase(fixtureName);

                }
                else
                {
                    _lifecycle.StopTestCase(x => { x.uuid = fixtureName; x.status = Status.passed; x.attachments = attaches; x.statusDetails = new StatusDetails() { message = $"Test case {fixtureName} passed successfully." }; });
                    _lifecycle.WriteTestCase(fixtureName);
                }
            }
        }

        public void StepStarted(string fullName, string stepName)
        {
            lock (_caseStorage)
            {
                _lifecycle.StartStep(fullName, new StepResult() { name = stepName }); ;
            }
        }

        public void StepFinished(string fullName, TestStatus result, Exception exception = null)
        {
            lock (_caseStorage)
            {
                _lifecycle.StopStep(fullName);
            }
        }

        private string GetFixtureName(string fullName)
        {
            try
            {
                string fixtureName = fullName.Contains("+") ? fullName.Remove(fullName.IndexOf('+')) : fullName;
                return fixtureName.Substring(fixtureName.LastIndexOf('.') + 1);
            }
            catch (IndexOutOfRangeException)
            {
                Logger.WriteLine("Cannot get fixture name", fullName);
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                return fullName.Substring(fullName.LastIndexOf('.') + 1);
            }
        }
    }
}
