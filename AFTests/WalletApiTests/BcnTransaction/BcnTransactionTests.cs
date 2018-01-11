using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.BcnTransaction
{
    class BcnTransactionTests
    {
        public class GetBcnTransaction : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBcnTransactionTest()
            {
                Assert.Ignore("Get logic for this case");
                var newUser = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                string bcnId = "testId"; //get valid transaction ID
                var response = walletApi.BcnTransaction.GetBcnTransaction(bcnId, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null.Or.Empty);
                Assert.That(response.GetResponseObject().Result.Transaction, Is.Not.Null, "unexpected transaction id");
            }
        }

        public class GetBcnTransactionInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^(0")]
            [Category("WalletApi")]
            public void GetBcnTransactionInvalidTokenTest(string token)
            {
                string bcnId = "testId"; //get valid transaction ID
                var response = walletApi.BcnTransaction.GetBcnTransaction(bcnId, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        #region offchain
        public class GetBcnTransactionOffChain : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBcnTransactionOffChainTest()
            {
                Assert.Ignore("Get logic for this case");
                var newUser = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                string bcnId = "testId"; //get valid transaction ID
                var response = walletApi.BcnTransaction.GetBcnTransactionOffChain(bcnId, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null.Or.Empty);
                Assert.That(response.GetResponseObject().Result.Transaction, Is.Not.Null, "unexpected transaction id");
            }
        }

        public class GetBcnTransactionOffchainInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^(0")]
            [Category("WalletApi")]
            public void GetBcnTransactionOffchainInvalidTokenTest(string token)
            {
                string bcnId = "testId"; //get valid transaction ID
                var response = walletApi.BcnTransaction.GetBcnTransactionOffChain(bcnId, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
        #endregion
    }
}
