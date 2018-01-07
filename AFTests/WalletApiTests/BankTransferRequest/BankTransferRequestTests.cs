using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using LykkePay.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BankTransferRequest
{
    class BankTransferRequestTests
    {
        public class PostBankTransferRequest : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            [Description("Need to check cases if AssetID is not correct and/or balanceChange not correct")]
            public void PostBankTransferRequestTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var trModel = new TransferReqModel()
                {
                   AssetId = "BTC",
                   BalanceChange = 10
                };

                var response = walletApi.BankTransferRequest.PostBankTransferRequest(trModel, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null.Or.Empty);
            }
        }

        public class PostBankTransferRequestInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("123457")]
            [TestCase("testToken")]
            [TestCase("!@%^&*(0")]
            [Category("WalletApi")]
            public void PostBankTransferRequestInvalidTokenTest(string token)
            {
                var trModel = new TransferReqModel()
                {
                    AssetId = "BTC",
                    BalanceChange = 10
                };

                var response = walletApi.BankTransferRequest.PostBankTransferRequest(trModel, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
