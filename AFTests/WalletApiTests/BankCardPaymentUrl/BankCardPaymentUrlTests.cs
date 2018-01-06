using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BankCardPaymentUrl
{
    class BankCardPaymentUrlTests
    {
        public class BankCardPaymentUrl : WalletApiBaseTest
        {
            [Test]
            [Description("Check that error is not Null, but object is still created")]
            [Category("WalletApi")]
            public void BankCardPaymentUrlTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var cardPaymentUrl = new BankCardPaymentUrlInputModel()
                {
                    Address = TestData.GenerateString(6),
                    Amount = 10,
                    AssetId = "BTC",
                    City = "Minsk",
                    Country = TestData.GenerateLetterString(6),
                    Email = TestData.GenerateEmail(),
                    FirstName = TestData.GenerateLetterString(6),
                    LastName = TestData.GenerateLetterString(8),
                    Phone = TestData.GeneratePhone(),
                    Zip = TestData.GenerateLetterString(6),
                };

                var response = walletApi.BankCardPaymentUrl.PostBankCardPaymentUrl(cardPaymentUrl, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                var responseObject = response.GetResponseObject();
                Assert.That(responseObject.Error.ToString(), Is.Null.Or.Empty);
            }
        }

        public class BankCardPaymentUrlInvalidToken : WalletApiBaseTest
        {
            [TestCase("1234567")]
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("!@$&(")]
            [Category("WalletApi")]
            public void BankCardPaymentUrlInvalidTokenTest(string token)
            {
               var cardPaymentUrl = new BankCardPaymentUrlInputModel()
                {
                    Address = TestData.GenerateString(6),
                    Amount = 10,
                    AssetId = "BTC",
                    City = "Minsk",
                    Country = TestData.GenerateLetterString(6),
                    Email = TestData.GenerateEmail(),
                    FirstName = TestData.GenerateLetterString(6),
                    LastName = TestData.GenerateLetterString(8),
                    Phone = TestData.GeneratePhone(),
                    Zip = TestData.GenerateLetterString(6),
                };

                var response = walletApi.BankCardPaymentUrl.PostBankCardPaymentUrl(cardPaymentUrl, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
