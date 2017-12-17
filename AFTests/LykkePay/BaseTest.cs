using LykkeAutomation.TestsCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LykkePay.Api;
using XUnitTestCommon.Reports;

namespace LykkePay.Tests
{
    public class BaseTest
    {
        public LykkePayApi lykkePayApi;

        #region response info
       

        public static void ValidateScheme(bool valid, IList<string> errors)
        {
            if (!valid)
            {
                errors.ToList().ForEach(e => Console.WriteLine(e));
                Assert.Fail("Scheme not valid");
            }
        }

        public static void AreEqualByJson(object expected, object actual)
        {

            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(expectedJson, Is.EqualTo(actualJson),  "Objects are not equals");
        }

        #endregion

        #region before after
        [SetUp]
        public void SetUp()
        {
            XUnitTestCommon.Reports.AllureReport.GetInstance().CaseStarted(TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Test.Name, "");

            lykkePayApi = new LykkePayApi();

            TestContext.WriteLine("SetUp");
        }


        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");

            XUnitTestCommon.Reports.AllureReport.GetInstance().CaseFinished(TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Result.Outcome.Status);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            XUnitTestCommon.Reports.AllureReport.GetInstance().RunStarted();
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //XUnitTestCommon.Reports.AllureReport.GetInstance().RunFinished();
        }

    #endregion

    #region allure helpers

        public static void Step(string name, Action action)
        {
            Exception ex = null;
            try
            {
                XUnitTestCommon.Reports.AllureReport.GetInstance().StepStarted(TestContext.CurrentContext.Test.FullName,
                    name);
                Logger.WriteLine($"Step: {name}");
                action();
            }catch(Exception e) {
                ex = e;
            }
            finally
            {
                XUnitTestCommon.Reports.AllureReport.GetInstance().StepFinished(TestContext.CurrentContext.Test.FullName,
             TestContext.CurrentContext.Result.Outcome.Status, ex);
            }
        }
        #endregion

    }
}
