using Allure.Commons;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Web.Utils;
using XUnitTestCommon.RestRequests;

namespace XUnitTestCommon.TestsCore
{
    public class Allure2Report
    {
        private static AllureLifecycle Allure => AllureLifecycle.Instance;
        private static int count = 0;

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

        public void StartStep(string name, ITestResult result)
        {
            Allure.StartStep(Guid.NewGuid().ToString(), new StepResult() { status = Status.passed, name = name});
        }

        public void UpdateStep(StepResult result)
        {
            Allure.UpdateStep(x => 
            {
                x.attachments = result.attachments;
                x.status = result.status;
                x.statusDetails = result.statusDetails;
                x.descriptionHtml = result.descriptionHtml;
            });
        }

        public void StopStep()
        {
            Allure.StopStep();
        }

        public void PrepareEnvFile()
        {
            string propertiesPath = Path.Combine(Allure.ResultsDirectory, "environment.properties");
            File.Delete(propertiesPath);
            TestContext.WriteLine($"Count of env file execution {count++}");
        }

        public void CreateEnvFile()
        {
            string propertiesPath = Path.Combine(Allure.ResultsDirectory, "environment.properties");
            AllurePropertiesBuilder.Instance.AddPropertyPair("Date", DateTime.Now.ToString());
            
            AllurePropertiesBuilder.Instance.SaveAllureProperties(propertiesPath);
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
            var descr = nunitTest.Properties.Get("Description")?.ToString() ?? nunitTest.Parent.Properties.Get("Description")?.ToString();

            var testResult = new Allure.Commons.TestResult
            {
                uuid = nunitTest.Id,
                historyId = nunitTest.FullName,
                name = nunitTest.MethodName,
                fullName = nunitTest.FullName,
                descriptionHtml = descr ?? "",
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

        private List<Allure.Commons.Parameter> GetParameters()
        {
            var parameters = TestExecutionContext.CurrentContext.CurrentTest.Arguments;
            if (!parameters.Any())
                return new List<Allure.Commons.Parameter>();
            return parameters.Select(p => new Allure.Commons.Parameter() { name = "param", value = p?.ToString() ?? "null" }).ToList();
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

        public void AttachVideo(string caseId, string hubUrl, string sessionId)
        {
            if (hubUrl.Contains("127.0.0.1") || hubUrl.Contains("localhost"))
                return;

            if (sessionId == null)
                return;

            var videoAttach = GetVideoAttach(hubUrl, sessionId);
            if (videoAttach != null)
                Allure.UpdateTestCase(caseId, x => x.attachments.Add(videoAttach));
        }

        private void WaitForVideoWillBeSavedFromDocker(string hubUrl, string sessionId)
        {
            int timer = 50;
            while (timer-- > 0)
            {
                try
                {
                    var videoUrl = $"{hubUrl.Replace("wd/hub", "")}";
                    var client = new RestClient(videoUrl);
                    var request = new RestRequest($"video/{sessionId}", Method.GET);
                    var response = client.Execute(request);
                    var currentLength = long.Parse(response.Headers.First(h => h.Name == "Content-Length").Value.ToString());

                    while (timer-- > 0 && (currentLength >= long.Parse(client.Execute(request).Headers.First(h => h.Name == "Content-Length").Value.ToString()) || long.Parse(client.Execute(request).Headers.First(h => h.Name == "Content-Length").Value.ToString()) < 1000))
                    {
                      
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                   
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        //TODO: refactor that
        private Attachment GetVideoAttach(string hubUrl, string sessionId)
        {
            if (!hubUrl.Contains("wd/hub"))
            {
                TestContext.Progress.WriteLine($"Hub URL {hubUrl} does not contains 'hub/url/'");
                return null;
            }

            WaitForVideoWillBeSavedFromDocker(hubUrl, sessionId);

            string testOutput = TestExecutionContext.CurrentContext.CurrentResult.Output;
            string attachFile = $"{sessionId}.mp4";
            var videoUrl = $"{hubUrl.Replace("wd/hub","video/")}{sessionId}";
            int timer = 30;
            while (timer-- > 0)
            {
                try
                {
                    new WebClient().DownloadFile(videoUrl, Path.Combine(Allure.ResultsDirectory, attachFile));
                    break;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            return new Attachment()
            {
                name = "Video",
                source = attachFile,
                type = "video/mp4"
            };
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

        public Attachment GetScreenShotAttachment(Screenshot screenshot)
        {
            var fileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "allure-results", Guid.NewGuid() + ".png");
            File.WriteAllBytes(fileName, screenshot.AsByteArray);

            return new Allure.Commons.Attachment()
            {
                name = "Browser1",
                type = "image/png",
                source = fileName
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
