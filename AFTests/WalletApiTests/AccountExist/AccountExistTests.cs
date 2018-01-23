using LykkeAutomation.Api.ApiModels.AccountExistModels;
using LykkeAutomation.TestsCore;
using XUnitTestCommon.TestsData;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using XUnitTestCommon.Tests;
using LykkeAutomation.Api;

namespace AFTests.WalletApiTests
{
    class AccountExistTests
    {
        public class AccountExistInvalidEmail : WalletApiBaseTest
        {
            [Test]
            [Parallelizable]
            [Category("AccountExist"), Category("WalletApi")]
            public void AccountExistInvalidEmailTest()
            {
                ApiSchemes apiSchemes = new ApiSchemes();

                string invalidEmail = TestData.GenerateEmail();

                var response = walletApi.AccountExist.GetAccountExistResponse(invalidEmail);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Invalid status code");

                var obj = JObject.Parse(response.Content);
                ValidateScheme(obj.IsValid(apiSchemes.AccountExistSchemes.AuthResponseScheme, out schemesError), schemesError);

                var model = AccountExistModel.ConvertToAccountExistModel(response.Content);
                Assert.That(model.Result.IsEmailRegistered, Is.False, "Email is registered");
                Assert.That(model.Error, Is.Null, "Error is not null");
            }
        }   
    }
}
