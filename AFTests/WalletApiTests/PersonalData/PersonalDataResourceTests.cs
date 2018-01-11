using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.Api;
using LykkeAutomation.TestsCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.Tests;

namespace AFTests.WalletApiTests
{
    class PersonalDataResourceTests
    {

        public class PersonalDataInvalidToken : WalletApiBaseTest
        {
            ApiSchemes apiSchemes = new ApiSchemes();

            [Test]
            [Parallelizable]
            [Category("PersonalData"), Category("WalletApi")]
            public void PersonalDataInvalidTokenTest()
            {
                var invalidToken = "invalidToken";
                var response = walletApi.PersonalData.GetPersonalDataResponse(invalidToken);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), $"Invalid status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, $"Invalid response content ");
            }
        }

        public class PersonalDataEmptyToken : WalletApiBaseTest
        {
            ApiSchemes apiSchemes = new ApiSchemes();

            [Test]
            [Parallelizable]
            [Category("PersonalData"), Category("WalletApi")]
            public void PersonalDataEmptyTokenTest()
            {
                var emptyToken = "";
                var response = walletApi.PersonalData.GetPersonalDataResponse(emptyToken);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Invalid status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Invalid response content");
            }
        }

        public class PersonalDataValidToken : WalletApiBaseTest
        {
            ApiSchemes apiSchemes = new ApiSchemes();

            [Test]
            [Parallelizable]
            [Category("PersonalData"), Category("WalletApi")]
            public void PersonalDataValidTokenTest()
            {
                AccountRegistrationModel user = new AccountRegistrationModel().GetTestModel();
                var registationResponse = walletApi.Registration.PostRegistrationResponse(user).GetResponseObject();
                Assert.That(registationResponse.Error, Is.Null, "Error is not null");

                var response = walletApi.PersonalData.GetPersonalDataResponse(registationResponse.Result.Token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Invalid status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.False, "Invalid response content");

                JObject responseObject = JObject.Parse(response.Content);
                bool valid = responseObject.IsValid(apiSchemes.PersonalDataSheme.PersonalDataResponseSchema, out schemesError);
                ValidateScheme(valid, schemesError);

                var responseModel = walletApi.PersonalData.GetPersonalDataResponse(registationResponse.Result.Token).GetResponseObject();
                Assert.That(responseModel.PersonalData.FullName, Is.EqualTo(user.FullName), "Full Name is not the same");
                Assert.That(responseModel.PersonalData.Email, Is.EqualTo(user.Email), "Email is not the same");
                Assert.That(responseModel.PersonalData.Phone, Is.EqualTo(user.ContactPhone), "Phone is not the same");
            }
        }
    }
}
