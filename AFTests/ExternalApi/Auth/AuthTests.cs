using LykkeAutomation.Api;
using LykkeAutomation.Api.ApiModels.AuthModels;
using LykkeAutomation.ApiModels.RegistrationModels;
using LykkeAutomation.TestsCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.Tests;
using static LykkeAutomation.Api.ApiModels.AuthModels.AuthModels;

namespace AFTests.ExternalApiTests
{
    class AuthTests
    {

        public class SuccessAuthAfterRegistration : BaseTest
        {
            [Test]
            [Parallelizable]
            [Category("Auth"), Category("All")]
            public void SuccessAuthAfterRegistrationTest()
            {
                LykkeExternalApi lykkeExternalApi = new LykkeExternalApi();
                ApiSchemes apiSchemes = new ApiSchemes();

                AccountRegistrationModel newUser = new AccountRegistrationModel();
                var response = lykkeExternalApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                Assert.That(response.Error, Is.Null, $"Error message not empty {response.Error?.Message}");

                AuthenticateModel auth = new AuthenticateModel(newUser);
                var authResponse = lykkeExternalApi.Auth.PostAuthResponse(auth);
                Assert.That(authResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Invalid status code");

                var obj = JObject.Parse(authResponse.Content);
                Assert.That(obj.IsValid(apiSchemes.AuthScheme.AuthResponseScheme), Is.True, "Responce JSON is not valid scheme");

                var authModel = AuthModels.ConvertToAuthModelResponse(authResponse.Content);
                Assert.That(authModel.Result.PersonalData.FullName, Is.EqualTo(newUser.FullName), "Invalid Full Name");
            }
        }
    }
}
