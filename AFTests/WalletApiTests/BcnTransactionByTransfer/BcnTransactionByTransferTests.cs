using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BcnTransactionByTransfer
{
    class BcnTransactionByTransferTests
    {

        public class GetBcnTransactionByTranfer : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBcnTransactionByTranferTest()
            {
                Assert.Ignore("Get valid transfer id for transactio");
                var transferId = TestData.GenerateString(6);
                var newUser = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BcnTransactionByTransfer.GetBcnTransactionByTransfer(transferId, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null, "Unexpected Error");
            }
        }

        public class GetBcnTransactionByTranferInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@%^&*9)")]
            [Category("WalletApi")]
            public void GetBcnTransactionByTranferInvalidTokenTest(string token)
            {
                var transferId = TestData.GenerateString(6);            

                var response = walletApi.BcnTransactionByTransfer.GetBcnTransactionByTransfer(transferId, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
