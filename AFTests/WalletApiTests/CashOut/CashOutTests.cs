using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.CashOut
{
    class CashOutTests
    {

        public class PostCashOut : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostCashOutTest()
            {
                Assert.Ignore("Get valid cashOutPostModel");
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var cashOutModel = new CashOutPostModel() {Amount = 10, AssetId = "BTC", MultiSig = TestData.GenerateString(6) };

                var response = walletApi.CashOut.PostCashOut(cashOutModel, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostCashOutInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%*(0")]
            [Category("WalletApi")]
            public void PostCashOutInvalidTokenTest(string token)
            {
                var cashOutModel = new CashOutPostModel() { Amount = 10, AssetId = "BTC", MultiSig = TestData.GenerateString(6) };

                var response = walletApi.CashOut.PostCashOut(cashOutModel, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
