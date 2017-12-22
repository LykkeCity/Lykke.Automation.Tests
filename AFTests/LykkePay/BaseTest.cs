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
using TestsCore.TestsCore;

namespace LykkePay.Tests
{
    public class BaseTest
    {
        public LykkePayApi lykkePayApi = new LykkePayApi();
        private Allure2Report allure = new Allure2Report();

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
            allure.AllureBeforeTest();
            TestContext.WriteLine("SetUp");
        }


        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");
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

    #endregion
    }
}
