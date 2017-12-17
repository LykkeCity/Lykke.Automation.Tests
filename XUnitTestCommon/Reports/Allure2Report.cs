using Allure.Commons;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestsCore.TestsCore
{
    public class Allure2Report
    {
        private static AllureLifecycle Allure => AllureLifecycle.Instance;

        //Need for run via VS
        static Allure2Report()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.WorkDirectory);
            TestContext.Progress.WriteLine($"Writing Allure results to {Allure.ResultsDirectory}");
        }

        //[OneTimeSetUp]
        public void AllureBeforeAllTestsInClass()
        {
            var nunitFixture = TestExecutionContext.CurrentContext.CurrentTest;

            var fixture = new TestResultContainer
            {
                uuid = nunitFixture.Id,
                name = nunitFixture.ClassName
            };
            Allure.StartTestContainer(fixture);
        }

        //[OneTimeTearDown]
        public void AllureAfterAllTestsInClass()
        {
            AddMissedTests();

            var nunitFixture = TestExecutionContext.CurrentContext.CurrentTest;

            Allure.StopTestContainer(nunitFixture.Id);
            Allure.WriteTestContainer(nunitFixture.Id);
        }

        private void AddMissedTests()
        {
            var nunitFixtureResult = TestExecutionContext.CurrentContext.CurrentResult;
            if (nunitFixtureResult.ResultState.Site == FailureSite.SetUp)
            {
                var failedTestResults = nunitFixtureResult.Children.Where(r => r is TestCaseResult);
                foreach (var testResult in failedTestResults)
                {
                    AddMissedTest(testResult);
                }
            }
        }

        private void AddMissedTest(ITestResult result)
        {
            var testResult = new Allure.Commons.TestResult
            {
                uuid = result.Test.Id,
                historyId = result.Test.FullName,
                name = result.Test.MethodName,
                fullName = result.Test.FullName,
                labels = new List<Label>
                    {
                        Label.Suite(result.Test.ClassName),
                        Label.Thread(),
                        Label.Host(),
                        Label.TestClass(result.Test.ClassName),
                        Label.TestMethod(result.Test.MethodName),
                        Label.Package(result.Test.Fixture?.ToString() ?? result.Test.ClassName)
                    },
                status = GetNunitStatus(result.ResultState),
                statusDetails = new StatusDetails
                {
                    message = result.Message,
                    trace = result.StackTrace
                }
            };

            Allure.StartTestCase(testResult);
            Allure.StopTestCase(result.Test.Id);
            Allure.WriteTestCase(result.Test.Id);
        }

        //[SetUp]
        public void AllureBeforeTest()
        {
            var nunitTest = TestExecutionContext.CurrentContext.CurrentTest;
            var x = TestContext.CurrentContext.Test;
            string suitName = nunitTest.ClassName.Split('.')[2]; //LykkeAutomationPrivate.Tests.ClientAccount.DeleteClientAccount.DeleteClientAccountTest -> ClientAccount

            var testResult = new Allure.Commons.TestResult
            {
                uuid = nunitTest.Id,
                historyId = nunitTest.FullName,
                name = nunitTest.MethodName,
                fullName = nunitTest.FullName,
                parameters = GetParameters(),
                labels = new List<Label>
                    {
                        Label.Suite(suitName),
                        Label.Thread(),
                        Label.Host(),
                        Label.TestClass(nunitTest.ClassName),
                        Label.TestMethod(nunitTest.MethodName),
                        Label.Package(nunitTest.ClassName.Replace('+', '.')),
                    }
            };

            testResult.labels.AddRange(GetCategories());
            Allure.StartTestCase(testResult);
        }

        private IList<Label> GetCategories()
        {
            var key = "Category";
            var categories = new List<string>();
            var test = TestExecutionContext.CurrentContext.CurrentTest;
            if (test.Properties.ContainsKey(key))
                categories.AddRange(test.Properties[key].Cast<string>());
            if (test.Parent != null && test.Parent.Properties.ContainsKey(key))
                categories.AddRange(test.Parent.Properties[key].Cast<string>());
            return categories.Select(c => Label.Feature(c)).ToList();
        }

        private List<Parameter> GetParameters()
        {
            var parameters = TestExecutionContext.CurrentContext.CurrentTest.Arguments;
            if (!parameters.Any())
                return new List<Parameter>();
            return parameters.Select(p => new Parameter() { name = "param", value = p?.ToString() ?? "null" }).ToList();
        }

        //[TearDown]
        public void AllureAfterTest()
        {
            var nunitTest = TestExecutionContext.CurrentContext.CurrentTest;

            AttachTestLog(nunitTest.Id);
            Allure.UpdateTestCase(nunitTest.Id, x =>
            {
                x.statusDetails = new StatusDetails
                {
                    message = TestContext.CurrentContext.Result.Message,
                    trace = TestContext.CurrentContext.Result.StackTrace
                };
                x.status = GetNunitStatus(TestContext.CurrentContext.Result.Outcome);
                x.attachments.AddRange(Allure2Helper.GetAttaches());
            });

            Allure.StopTestCase(nunitTest.Id);
            Allure.WriteTestCase(nunitTest.Id);
        }

        private void AttachTestLog(string caseId)
        {
            var logAttach = GetTestLog();
            if (logAttach != null)
                Allure.UpdateTestCase(caseId, x => x.attachments.Add(logAttach));
        }

        private Attachment GetTestLog()
        {
            string testOutput = TestExecutionContext.CurrentContext.CurrentResult.Output;
            string attachFile = Guid.NewGuid().ToString("N") + ".log";
            try
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(Allure.ResultsDirectory, attachFile), testOutput);
            }
            catch (Exception)
            {
                return null;
            }

            return new Attachment()
            {
                name = "Output",
                source = attachFile,
                type = "text/plain"
            };
        }

        private static Status GetNunitStatus(ResultState result)
        {
            switch (result.Status)
            {
                case TestStatus.Inconclusive:
                    return Status.none;
                case TestStatus.Skipped:
                    return Status.skipped;
                case TestStatus.Passed:
                    return Status.passed;
                case TestStatus.Warning:
                    return Status.broken;
                case TestStatus.Failed:
                    if (String.IsNullOrEmpty(result.Label))
                        return Status.failed;
                    else
                        return Status.broken;
                default:
                    return Status.none;
            }
        }
    }
}
