using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BroadcastTransaction
{
    class BroadcastTransactionTests
    {

        public class PostBroadcastTransaction : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostBroadcastTransactionTest()
            {
                Assert.Ignore("Get Valid ApiTransaction model");
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                var model = new ApiTransaction() { Hex = TestData.GenerateString(8), Id = TestData.GenerateString(12) };
                var response = walletApi.BroadcastTransaction.PostBroadcastTransaction(model, registeredClient.Result.Token);

                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostBroadcastTransactionInvalidTransaction : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostBroadcastTransactionInvalidTransactionTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                var model = new ApiTransaction() { Hex = TestData.GenerateString(8), Id = TestData.GenerateString(12) };
                var response = walletApi.BroadcastTransaction.PostBroadcastTransaction(model, registeredClient.Result.Token);

                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Not.Null);
            }
        }

        public class PostBroadcastTransactionInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@%^&(0")]
            [Category("WalletApi")]
            public void PostBroadcastTransactionInvalidTokenTest(string token)
            {
                var model = new ApiTransaction() { Hex = TestData.GenerateString(8), Id = TestData.GenerateString(12) };
                var response = walletApi.BroadcastTransaction.PostBroadcastTransaction(model, token);

                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
