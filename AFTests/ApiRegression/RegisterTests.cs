using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using XUnitTestCommon.TestsData;

namespace AFTests.ApiRegression
{
    public class RegisterTests : ApiRegressionBaseTest
    {
        [Test]
        [Category("ApiRegression")]
        public void RegisterTest()
        {
            var email = TestData.GenerateEmail();

            var getClientState = walletApi.ClientState.GetClientState(email, null);
            Assert.Multiple(() =>
            {
                getClientState.Validate.StatusCode(System.Net.HttpStatusCode.OK);
                var getClientStateData = getClientState.GetResponseObject();
                Assert.That(getClientStateData.Error, Is.Null);
                Assert.That(getClientStateData.Result.IsRegistered, Is.False);
            });

            //var postAuth = walletApi.Auth.PostAuthResponse()
        }
    }
}
