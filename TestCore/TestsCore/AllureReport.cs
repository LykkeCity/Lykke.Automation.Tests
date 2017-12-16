using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Framework;
using Allure.Commons;
using TestsCore.TestsCore;

namespace LykkeAutomation.TestsCore
{
    public class AllureReport
    {
        private static AllureReport allureReport;
        private static AllureLifecycle _lifecycle;
        private static Dictionary<string, List<dynamic>> _caseStorage = new Dictionary<string, List<dynamic>>();
        private object debugLock = new object();
        private static string resultDir;
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

        public void RunStarted()
        {
            _lifecycle = AllureLifecycle.Instance;
        }

        public void RunFinished()
        {
            CreateEnvFile();
           // CreateCategoryFile();
        }

        public void CaseStarted(string fullName, string name, string description)
        {
            string fixtureName = GetFixtureName(fullName);

           // var labels = new List<Label> { Label.Thread(), Label.Feature("some feature label"), Label.Host(), Label.Epic("label epic"), Label.Severity(SeverityLevel.critical), Label.Story("label story"), Label.Tag("label tag") };
            lock (_caseStorage)
            {
                _lifecycle.StartTestCase(new TestResult() { uuid = fixtureName, name = name, description = description/*, labels = labels*/ });
            }
        }

        public void CaseFinished(string fullName, TestStatus result, Exception exception)
        {
            lock (_caseStorage)
            {
                var testDescription = TestContext.CurrentContext.Test.Properties["Description"];
                var textDescription = testDescription.Count > 0 ? testDescription[0].ToString() : "No Description";

                var cats = TestContext.CurrentContext.Test.Properties["Category"];

                var parameters = new List<Parameter>();
                foreach (var cat in cats)
                {
                    parameters.Add(new Parameter() { name = "Category", value = cat.ToString() });
                }

                string fixtureName = GetFixtureName(fullName);

                List<Attachment> attaches = new List<Attachment>();
                var testLogPath = TestContext.CurrentContext.WorkDirectory + $"/allure-results/{Guid.NewGuid()}.log";
                var log = TestLog.GetLog();
                File.WriteAllText(testLogPath, log);
                attaches.Add(new Attachment() { name = "TestLog", source = testLogPath, type = "application/json" });

                if (result == TestStatus.Failed)
                {
                    AssertionException ex = new AssertionException(TestContext.CurrentContext.Result.Message);
                    string st = TestContext.CurrentContext.Result.Assertions.ToList().Count == 0 ?
                        "" : TestContext.CurrentContext.Result.Assertions.ToList()?[0].StackTrace;

                    _lifecycle.StopTestCase(x => { x.uuid = fixtureName; x.status = Status.failed; x.attachments = attaches; x.statusDetails = new StatusDetails() { message = ex.Message, trace = st }; x.description = textDescription; x.parameters = parameters; });
                    _lifecycle.WriteTestCase(fixtureName);

                }
                else
                if (result == TestStatus.Skipped)
                {
                    _lifecycle.StopTestCase(x => { x.uuid = fixtureName; x.status = Status.skipped; x.attachments = attaches; x.statusDetails = new StatusDetails() { message = TestContext.CurrentContext.Result.Message }; x.description = textDescription; x.parameters = parameters; });
                    _lifecycle.WriteTestCase(fixtureName);
                }
                else
                {
                    var st = TestContext.CurrentContext.Result;

                    _lifecycle.StopTestCase(x => { x.uuid = fixtureName; x.status = Status.passed; x.attachments = attaches; x.statusDetails = new StatusDetails() { message = st.Outcome.Status.ToString() }; x.description = textDescription; x.parameters = parameters; });
                    _lifecycle.WriteTestCase(fixtureName);
                }
            }
        }

        public void StepStarted(string fullName, string stepName)
        {
            lock (_caseStorage)
            {
                _lifecycle.StartStep(fullName, new StepResult() {name = stepName });;
            }
        }

        public void StepFinished(string fullName, TestStatus result, Exception exception = null)
        {
            lock (_caseStorage)
            {
                _lifecycle.StopStep(fullName);
            }
        }

        public void StartTestContainer()
        {
            lock (_caseStorage)
            {
                _lifecycle.StartTestContainer(new TestResultContainer() { });
            }
        }
       
        private void CreateEnvFile()
        {
            string propertiesPath = Path.Combine(_lifecycle.ResultsDirectory, "environment.properties");
            AllurePropertiesBuilder.Instance.AddPropertyPair("Date", DateTime.Now.ToString());
            AllurePropertiesBuilder.Instance.SaveAllureProperties(propertiesPath);
        }

        private void CreateCategoryFile()
        {
            string categoryFilePath = Path.Combine(resultDir, "categories.json");
            File.WriteAllText(categoryFilePath, new AllureCategoriesJson().GetJson());
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
                TestLog.WriteLine("Cannot get fixture name", fullName);
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                return fullName.Substring(fullName.LastIndexOf('.') + 1);
            }
        }
    }

}
