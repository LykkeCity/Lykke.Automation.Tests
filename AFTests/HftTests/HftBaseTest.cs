using HFT;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;
using XUnitTestCommon.TestsCore;

namespace AFTests.HftTests
{
    class HftBaseTest : BaseTest
    {
        protected const string ApiKey = "3982d2d7-ae7b-4995-8074-d563707b986e";

        protected Hft hft = new Hft();

        [OneTimeTearDown]
        public void SetProperty()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Date", DateTime.Now.ToString());
            try
            {
                var isAlive = hft.IsAlive.GetIsAlive().GetResponseObject();
                AllurePropertiesBuilder.Instance.AddPropertyPair("Env", isAlive.Env);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
            }
            catch (Exception) { /*do nothing*/}

            new Allure2Report().CreateEnvFile();
        }
    }
}
