using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.BaseAsset
{
    class BaseAssetTests
    {
        public class GetBaseAssetAfterPostWalletApi : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBaseAssetAfterPostWalletApiTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var model = new PostClientBaseCurrencyModel() {Id = "BTC" };

                var response = walletApi.BaseAsset.PostBaseAsset(model, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null.Or.Empty);

                var getResponse = walletApi.BaseAsset.GetBaseAsset(registeredClient.Result.Token);
                getResponse.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(getResponse.GetResponseObject().Error, Is.Null.Or.Empty);
            }
        }

        public class GetBaseAssetNoPostWalletApi : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBaseAssetNoPostWalletApiTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getResponse = walletApi.BaseAsset.GetBaseAsset(registeredClient.Result.Token);
                Assert.That(getResponse.StatusCode, Does.Not.EqualTo(HttpStatusCode.OK));
                Assert.That(getResponse.GetResponseObject().Error, Is.Not.Null.Or.Empty);
            }
        }

        public class GetBaseAssetInvalidTokenWalletApi : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("12344743")]
            [TestCase("!@$%^&")]
            [Category("WalletApi")]
            public void GetBaseAssetInvalidTokenWalletApiTest(string token)
            {
                var getResponse = walletApi.BaseAsset.GetBaseAsset(token);
                getResponse.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
