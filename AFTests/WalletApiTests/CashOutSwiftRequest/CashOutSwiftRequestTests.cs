using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.CashOutSwiftRequest
{
    class CashOutSwiftRequestTests
    {

        public class PostCashOutSwiftRequest : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostCashOutSwiftRequestTest()
            {
                Assert.Ignore("Get CashOut swift model");
                var model = new SwiftCashOutReqModel()
                {
                    AssetId = "BTC",
                    AccHolderAddress = TestData.GenerateString(8),
                    AccHolderCity = "Minsk",
                    AccHolderCountry = "Blr",
                    AccHolderZipCode = TestData.GenerateNumbers(6),
                    AccName = TestData.GenerateLetterString(6),
                    AccNumber = TestData.GenerateNumbers(6),
                    Amount = 10,
                    BankName = "BankOfAmerica",
                    Bic = TestData.GenerateString(6)
                };

                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.CashOutSwiftRequest.PostCashOutSwiftRequest(model, registeredClient.Result.Token);
                response.Validate.StatusCode(System.Net.HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostCashOutSwiftRequestInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@%^*(0")]
            [Category("WalletApi")]
            public void PostCashOutSwiftRequestInvalidTokenTest(string token)
            {
                var model = new SwiftCashOutReqModel()
                {
                    AssetId = "BTC",
                    AccHolderAddress = TestData.GenerateString(8),
                    AccHolderCity = "Minsk",
                    AccHolderCountry = "Blr",
                    AccHolderZipCode = TestData.GenerateNumbers(6),
                    AccName = TestData.GenerateLetterString(6),
                    AccNumber = TestData.GenerateNumbers(6),
                    Amount = 10,
                    BankName = "BankOfAmerica",
                    Bic = TestData.GenerateString(6)
                };

                var response = walletApi.CashOutSwiftRequest.PostCashOutSwiftRequest(model, token);
                response.Validate.StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}
