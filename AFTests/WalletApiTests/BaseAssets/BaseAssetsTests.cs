using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.BaseAssets
{
    class BaseAssetsTests
    {

        public class BaseAssetsNull : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void BaseAssetsNullTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BaseAssets.GetBaseAssets(registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
                Assert.That(response.GetResponseObject().Result.Assets.Count, Is.EqualTo(0));
            }
        }

        public class BaseAssetsInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("12345678")]
            [TestCase("testToken")]
            [TestCase("!@%&^(9")]
            [Category("WalletApi")]
            public void BaseAssetsInvalidTokenTest(string token)
            {
                var response = walletApi.BaseAssets.GetBaseAssets(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class BaseAssetsNotEmptyAfterAddedAsset : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            [Description("Investigate this resource. Test fails")]
            public void BaseAssetsNotEmptyAfterAddedAssetTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var model = new PostClientBaseCurrencyModel() { Id = "BTC" };

                var response = walletApi.BaseAsset.PostBaseAsset(model, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var getResponse = walletApi.BaseAsset.GetBaseAsset(registeredClient.Result.Token);
                getResponse.Validate.StatusCode(HttpStatusCode.OK);

                var responseBaseAssets = walletApi.BaseAssets.GetBaseAssets(registeredClient.Result.Token);
                responseBaseAssets.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(responseBaseAssets.GetResponseObject().Error, Is.Null);
                Assert.That(responseBaseAssets.GetResponseObject().Result.Assets.Count, Is.EqualTo(1));
            }
        }
    }
}
