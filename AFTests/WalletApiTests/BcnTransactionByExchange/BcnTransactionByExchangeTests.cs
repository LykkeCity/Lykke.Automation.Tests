using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BcnTransactionByExchange
{
    class BcnTransactionByExchangeTests
    {

        public class GetBcnTransactionByExchange : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBcnTransactionByExchangeTest()
            {
                Assert.Ignore("How to get exchange validId");
                var exchangeId = "validId"; // how to get valid EId
                var newUser = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BcnTransactionByExchange.GetBcnTransactionByExchange(exchangeId, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null, "Error is not null");
            }
        }

        public class GetBcnTransactionByExchangeInvalidExchangeId : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBcnTransactionByExchangeInvalidExchangeIdTest()
            {
                var exchangeId = TestData.GenerateString(6);
                var newUser = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BcnTransactionByExchange.GetBcnTransactionByExchange(exchangeId, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class GetBcnTransactionByExchangeInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("1234567")]
            [TestCase("!@$%^&(0")]
            [Category("WalletApi")]
            public void GetBcnTransactionByExchangeInvalidTokenTest(string token)
            {
                var exchangeId = TestData.GenerateString(6);
                
                var response = walletApi.BcnTransactionByExchange.GetBcnTransactionByExchange(exchangeId, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized, "Seems Authorization should be first check instead of echange id");
            }
        }
    }
}
