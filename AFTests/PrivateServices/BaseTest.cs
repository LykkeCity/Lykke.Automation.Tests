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
using XUnitTestCommon.Reports;

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
    }
}
