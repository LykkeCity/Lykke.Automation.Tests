using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BcnTransactionByCashOperation
{
    class BcnTransactionByCashOperationTests
    {

        public class GetBcnTransactionByCashOperation : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBcnTransactionByCashOperationTest()
            {
                Assert.Ignore("Get Valid id");
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                string bcnId = "testId"; //get valid transaction ID

                var response = walletApi.BcnTransactionByCashOperation.GetBcnTransactionByCashOperation(bcnId, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null, "Unexpected Error");
            }
        }

        public class GetBcnTransactionByInvalidCashOperation : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            [Description("Seems, authorization should have highest priority then cashOperationId. Thats why it fails. Check")]
            public void GetBcnTransactionByInvalidCashOperationTest()
            {
                string bcnId = TestData.GenerateString(6);
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                
                var response = walletApi.BcnTransactionByCashOperation.GetBcnTransactionByCashOperation(bcnId, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class GetBcnTransactionByInvalidCashOperationInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("1234567")]
            [TestCase("!@$%(0")]
            [Category("WalletApi")]
            [Description("Seems, authorization should have highest priority then cashOperationId. Thats why it fails. Check")]
            public void GetBcnTransactionByInvalidCashOperationInvalidTokenTest(string token)
            {
                string bcnId = TestData.GenerateString(6);

                var response = walletApi.BcnTransactionByCashOperation.GetBcnTransactionByCashOperation(bcnId, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
