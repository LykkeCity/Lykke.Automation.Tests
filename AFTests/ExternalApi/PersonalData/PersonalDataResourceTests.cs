using LykkeAutomation.Api;
using LykkeAutomation.ApiModels;
using LykkeAutomation.ApiModels.RegistrationModels;
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

namespace LykkeAutomation.Tests.PersonalData
{
    class PersonalDataResourceTests
    {

        public class PersonalDataInvalidToken : BaseTest
        {
            LykkeExternalApi lykkeExternalApi = new LykkeExternalApi();
            ApiSchemes apiSchemes = new ApiSchemes();

            [Test]
            [Parallelizable]
            [Category("PersonalData"), Category("All")]
            public void PersonalDataInvalidTokenTest()
            {
                var invalidToken = "invalidToken";
                var response = lykkeExternalApi.PersonalData.GetPersonalDataResponse(invalidToken);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), $"Invalid status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, $"Invalid response content ");
            }
        }

        public class PersonalDataEmptyToken : BaseTest
        {
            LykkeExternalApi lykkeExternalApi = new LykkeExternalApi();
            ApiSchemes apiSchemes = new ApiSchemes();

            [Test]
            [Parallelizable]
            [Category("PersonalData"), Category("All")]
            public void PersonalDataEmptyTokenTest()
            {
                var emptyToken = "";
                var response = lykkeExternalApi.PersonalData.GetPersonalDataResponse(emptyToken);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Invalid status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Invalid response content");
            }
        }

        public class PersonalDataValidToken : BaseTest
        {
            LykkeExternalApi lykkeExternalApi = new LykkeExternalApi();
            ApiSchemes apiSchemes = new ApiSchemes();

            [Test]
            [Parallelizable]
            [Category("PersonalData"), Category("All")]
            public void PersonalDataValidTokenTest()
            {
                AccountRegistrationModel user = new AccountRegistrationModel();
                var registationResponse = lykkeExternalApi.Registration.PostRegistrationResponse(user).GetResponseObject();
                Assert.That(registationResponse.Error, Is.Null, "Error is not null");

                var response = lykkeExternalApi.PersonalData.GetPersonalDataResponse(registationResponse.Result.Token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Invalid status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.False, "Invalid response content");

                JObject responseObject = JObject.Parse(response.Content);
                bool valid = responseObject.IsValid(apiSchemes.PersonalDataSheme.PersonalDataResponseSchema, out schemesError);
                ValidateScheme(valid, schemesError);

                var responseModel = lykkeExternalApi.PersonalData.GetPersonalDataResponse(registationResponse.Result.Token).GetResponseObject();
                Assert.That(responseModel.PersonalData.FullName, Is.EqualTo(user.FullName), "Full Name is not the same");
                Assert.That(responseModel.PersonalData.Email, Is.EqualTo(user.Email), "Email is not the same");
                Assert.That(responseModel.PersonalData.Phone, Is.EqualTo(user.ContactPhone), "Phone is not the same");
            }
        }
    }
}
