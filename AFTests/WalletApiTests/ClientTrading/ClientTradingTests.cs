using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.ClientTrading
{
    class ClientTradingTests
    {

        public class GetClientTradingTermsOfUse : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetClientTradingTermsOfUseTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.ClientTrading.GetClientTradingTermsOfUse(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetClientTradingTermsOfUseInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Category("WalletApi")]
            public void GetClientTradingTermsOfUseInvalidTokenTest(string token)
            {
                var response = walletApi.ClientTrading.GetClientTradingTermsOfUse(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class PostClientTradingTermsOfUseAgree : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientTradingTermsOfUseAgreeTest()
            {
                Assert.Ignore("Check why 'Live margin trading is not available'???");

                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getResponse = walletApi.ClientTrading.GetClientTradingTermsOfUse(registrationresponse.Result.Token);
                getResponse.Validate.StatusCode(HttpStatusCode.OK);

                var response = walletApi.ClientTrading.PostClientTradingTermsOfUseAgree(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostClientTradingTermsOfUseAgreeInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Category("WalletApi")]
            public void PostClientTradingTermsOfUseAgreeInvalidTokenTest(string token)
            {
                var response = walletApi.ClientTrading.PostClientTradingTermsOfUseAgree(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
