using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;
using Web.Utils;
using XUnitTestCommon.Tests;

namespace AFTests.Mobile
{
    public class MobileBaseTest : BaseTest
    {

        //static string APPIUM_URL = "http://127.0.0.1:4723/wd/hub";
        static string APPIUM_URL = "http://51.137.110.154:4445/wd/hub";
        //static string APPIUM_URL = "http://127.0.0.1:8888";
        private string APPLICATION = @"/apk/LykkeWallet_dev_1478.apk";
        //private string APPLICATION = @"c:\Lykke\Lykke.Automation.Tests\LykkeWallet_dev_1478.apk";

        protected AndroidDriver<AppiumWebElement> Driver;

        public static void UnInstallAppThroADB()
        {
            System.Diagnostics.Process.Start("adb", "uninstall com.lykkex.LykkeWallet");
        }

        [SetUp]
        public void SetUp()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(MobileCapabilityType.PlatformName, "Android");
            capabilities.SetCapability(MobileCapabilityType.AutomationName, "appium");
            //capabilities.SetCapability("avd", "android7.1-1");
            capabilities.SetCapability(MobileCapabilityType.DeviceName, "android7.1-1");
            capabilities.SetCapability(MobileCapabilityType.App, APPLICATION);
            capabilities.SetCapability(MobileCapabilityType.BrowserName, "android");
            capabilities.SetCapability("enableVideo", true);
            capabilities.SetCapability("enableVNC", true);
            capabilities.SetCapability("newCommandTimeout", 180);
            capabilities.SetCapability("appWaitPackage", "com.android.settings"); //Wait
            capabilities.SetCapability("appWaitActivity", "com.android.settings.CredentialStorage");
            Uri serverUri = new Uri(APPIUM_URL);

            Driver = new AndroidDriver<AppiumWebElement>(serverUri, capabilities, TimeSpan.FromSeconds(600));
        }

        [TearDown]
        public void TearDown()
        {
            var sessionId = Driver?.SessionId.ToString();
            if (Driver != null)
                Driver.RemoveApp("com.lykkex.LykkeWallet");
            else
            {
                UnInstallAppThroADB();
            }
            Driver?.Quit();
            
            allure.AttachVideo(TestContext.CurrentContext.Test.ID, APPIUM_URL, sessionId);
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

            if (Driver != null)
                attachments.Add(allure.GetScreenShotAttachment(Driver.GetScreenshot()));

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
    }
}
