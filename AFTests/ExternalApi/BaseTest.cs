using LykkeAutomation.Api;
using LykkeAutomation.TestsCore;
using LykkeAutomationPrivate.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsCore.TestsCore;

namespace LykkeAutomation.Tests
{
   public class BaseTest
    {
        public LykkeExternalApi lykkeExternalApi = new LykkeExternalApi();
        private Allure2Report allure = new Allure2Report();
        public ApiSchemes apiSchemes;
        public IList<string> schemesError;

        [SetUp]
        public void SetUp()
        {
            allure.AllureBeforeTest();
            apiSchemes = new ApiSchemes();
            schemesError = new List<string>();
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
            }
            catch (Exception e)
            {
                ex = e;
            }
            finally
            {
                AllureReport.GetInstance().StepFinished(TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Result.Outcome.Status, ex);
            }
        }
        #endregion



        public static void ValidateScheme(bool valid, IList<string> errors)
        {
            if (!valid)
            {
                errors.ToList().ForEach(e => Console.WriteLine(e));
                Assert.Fail("Scheme not valid");
            }
        }
    }
}
