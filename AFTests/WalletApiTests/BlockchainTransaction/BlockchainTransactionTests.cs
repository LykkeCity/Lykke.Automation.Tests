using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BlockchainTransaction
{
    class BlockchainTransactionTests
    {

        public class GetBlockchainTransaction : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBlockchainTransactionTest()
            {
                Assert.Ignore("Get Valid blockChainHash");
                var blockChainHash = TestData.GenerateString(12);

                var newUser = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BlockchainTransaction.GetBlockchainTransaction(blockChainHash, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetBlockchainTransactionInvalidHash : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBlockchainTransactionInvalidHashTest()
            {
                var blockChainHash = TestData.GenerateString(12);

                var newUser = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BlockchainTransaction.GetBlockchainTransaction(blockChainHash, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        public class GetBlockchainTransactionInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&)9")]
            [Category("WalletApi")]
            public void GetBlockchainTransactionInvalidTokenTest(string token)
            {
                var blockChainHash = TestData.GenerateString(12);

                var response = walletApi.BlockchainTransaction.GetBlockchainTransaction(blockChainHash, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
