using LykkeAutomation.Api;
using LykkeAutomation.ApiModels;
using LykkeAutomation.ApiModels.RegistrationModels;
using LykkeAutomation.TestsCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.Tests;

namespace AFTests.ExternalApiTests
{
    class RegistrationResourceTests
    {

        public class PostRegistrationPositive : BaseTest
        {
            LykkeExternalApi lykkeExternalApi = new LykkeExternalApi();
            ApiSchemes apiSchemes = new ApiSchemes();

            [Test]
            [Parallelizable]
            [Category("Registration"), Category("All")]
            public void PostRegistrationPositiveTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel();
                var response = lykkeExternalApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                Assert.That(response.Error, Is.Null, $"Error message not empty {response.Error?.Message}");
                Assert.That(response.Result.PersonalData.FullName, Is.EqualTo(newUser.FullName), "FullName is not the same");
            }
        }
    }
}
