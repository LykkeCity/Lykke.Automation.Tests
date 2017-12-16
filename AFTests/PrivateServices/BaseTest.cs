using LykkeAutomationPrivate.Api;
using LykkeAutomation.TestsCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestsCore.TestsCore;

namespace LykkeAutomationPrivate.Tests
{
    public class BaseTest
    {
        public LykkeApi lykkeApi = new LykkeApi();
        private Allure2Report allure = new Allure2Report();

        [SetUp]
        public void SetUp()
        {
            allure.AllureBeforeTest();
        }

        [TearDown]
        public void TearDown()
        {
            allure.AllureAfterTest();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            allure.AllureBeforeAllTestsInClass();
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            allure.AllureAfterAllTestsInClass();
        }


    #region allure helpers

        public static void Step(string name, Action action)
        {
            Exception ex = null;
            try
            {
                AllureReport.GetInstance().StepStarted(TestContext.CurrentContext.Test.FullName, name);
                TestLog.WriteLine($"Step: {name}");
                action();
            }catch(Exception e) {
                ex = e;
            }
            finally
            {
                AllureReport.GetInstance().StepFinished(TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Result.Outcome.Status, ex);
            }
        }
        #endregion

    }
}
