using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using Web.Utils;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.Tests;

namespace AFTests.Web
{
    public class WebBaseTest : BaseTest
    {
        protected LykkeRemoteWebDriver Driver1;
        protected LykkeRemoteWebDriver Driver2;
        static string HubUrl = "http://51.137.110.154:4445/wd/hub";
        //static string HubUrl = "http://127.0.0.1:8888";

        // [SetUp]
        public void SetUp()
        {
            RemoveAllVideoFromSelenoid("http://51.137.110.154:4444/wd/hub");
        }

        [TearDown]
        public void CloseDrivers()
        {
            if(Driver1 != null)
            {
                var sessionId = Driver1.SessionId.ToString();
                Driver1?.Quit();
                allure.AttachVideo(TestContext.CurrentContext.Test.ID, HubUrl, sessionId);
                RemoveVideoFromDocker(sessionId);
            }

            if (Driver2 != null)
            {
                var sessionId = Driver2.SessionId.ToString();
                Driver2?.Quit();
                allure.AttachVideo(TestContext.CurrentContext.Test.ID, HubUrl, sessionId);
                RemoveVideoFromDocker(sessionId);
            }
        }

        private static void RemoveVideoFromDocker(string sessionId)
        {
            var delete = Requests.For(HubUrl.Replace("/wd/hub","")).Delete($"/video/{sessionId}").Build().Execute();
            if (delete.StatusCode != System.Net.HttpStatusCode.OK)
                TestContext.Progress.WriteLine($"Error while deletion video for session {sessionId}");
        }

        private static void RemoveVideoFromDocker(string sessionId, string hubUrl)
        {
            var delete = Requests.For(hubUrl.Replace("/wd/hub", "")).Delete($"/video/{sessionId}").Build().Execute();
            if (delete.StatusCode != System.Net.HttpStatusCode.OK)
                TestContext.Progress.WriteLine($"Error while deletion video for session {sessionId}");
        }

        public static void RemoveAllVideoFromSelenoid(string selenoidNodeUrl)
        {
            var response = Requests.For(selenoidNodeUrl.Replace("/wd/hub", "")).Get("/video").Build().Execute();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return;

            var links = Regex.Matches(response.Content, "href=\".{33}mp4");
            for(int i =0; i< links.Count; i++)
            {
                RemoveVideoFromDocker(links[i].Value.Replace("href=\"", ""), selenoidNodeUrl);
            }
        }

        public LykkeRemoteWebDriver CreateWebDriver() => new LykkeRemoteWebDriver(HubUrl);

        public LykkeRemoteWebDriver CreateWebDriver(string hubUrl) => new LykkeRemoteWebDriver(hubUrl);

        private static string MakeScreenShot(LykkeRemoteWebDriver driver)
        {
            var screenshot = driver.GetScreenshot();
            var fileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "allure-results" ,Guid.NewGuid() + ".png");
            File.WriteAllBytes(fileName, screenshot.AsByteArray);
            return fileName;
        }

        protected new void Step(string name, Action action)
        {
            Console.WriteLine(name);
            var result = TestExecutionContext.CurrentContext.CurrentTest.MakeTestResult();
            var currentStatus = Allure.Commons.Status.passed;
            allure.StartStep(name, result);
            Exception exc = null;
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                exc = e;
                currentStatus = Allure.Commons.Status.failed;
            }
            finally { }

            List<Allure.Commons.Attachment> attachments = new List<Allure.Commons.Attachment>();

            if (Driver1 != null)
                attachments.Add(allure.GetScreenShotAttachment(Driver1.GetScreenshot()));

            if (Driver2 != null)
                attachments.Add(allure.GetScreenShotAttachment(Driver2.GetScreenshot()));

            allure.UpdateStep(new Allure.Commons.StepResult
            {
                status = currentStatus,
                attachments = attachments
            });

            allure.StopStep();
            if (exc != null)
            {
                throw exc;
            }
        }

        [OneTimeTearDown]
        public void OTTearDown()
        {
        }

    }
}
