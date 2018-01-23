using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.CountryPhoneCodes
{
    class CountryPhoneCodesTests
    {
        public class GetCountryPhoneCodes : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetCountryPhoneCodesTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.CountryPhoneCodes.GetCountryPhoneCodes(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
                Assert.That(response.GetResponseObject().Result.CountriesList.Count, Is.GreaterThan(5), "Country list count less than 5");
            }
        }

        public class GetCountryPhoneCodesInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Category("WalletApi")]
            public void GetCountryPhoneCodesInvalidTokenTest(string token)
            {
                var response = walletApi.CountryPhoneCodes.GetCountryPhoneCodes(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
