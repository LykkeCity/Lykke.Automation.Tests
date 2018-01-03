using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.AllAssetPairRates
{
    class AllAssetPairRatesTests
    {
        public class AllAssetPairRates : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void AllAssetPairRatesTest()
            {
                var allAssets = walletApi.AllAssetPairRates.GetAllAssetPairRates();
                allAssets.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(allAssets.GetResponseObject().Result.Rates.Count, Is.GreaterThan(1), "Assets count is less than 2");
            }
        }

        public class AllAssets : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void AllAssetsTest()
            {
                var allAssets = walletApi.AllAssets.GetAllAssetsPair();
                allAssets.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(allAssets.GetResponseObject().Result.AssetPairs.Count, Is.GreaterThan(1), "Assets count is less than 2");
            }
        }

        public class AllAssetPairId : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void AllAssetPairIdTest()
            {
                var allAssets = walletApi.AllAssets.GetAllAssetsPair();
                allAssets.Validate.StatusCode(HttpStatusCode.OK);
                var firstAsset = allAssets.GetResponseObject().Result.AssetPairs[0];
                var testFirstAsset = walletApi.AllAssets.GetAllAssetsPair(firstAsset.Id);

                AreEqualByJson(testFirstAsset.GetResponseObject().Result.AssetPair, firstAsset);
            }
        }

        #region Get assets categories
        public class GetAssetCategories : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetCategoriesTest()
            {
                var user = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var assetCategories = walletApi.AssetsCategories.GetAssetsCategories(registeredClient.GetResponseObject().Result.Token);
                assetCategories.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(assetCategories.GetResponseObject().Result.AssetCategories.Count, Is.GreaterThanOrEqualTo(1), "Asset categories count less than 1");
            }
        }

        public class GetAssetCategoriesInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("test")]
            [TestCase("1234")]
            [TestCase("!@#$%")]
            [Category("WalletApi")]
            public void GetAssetCategoriesInvalidTokenTest(string token)
            {
                var assetCategories = walletApi.AssetsCategories.GetAssetsCategories(token);
                assetCategories.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
        #endregion

        #region assetDescription
        public class GetAssetDescription : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetDescriptionTest()
            {
                var assetId = "dd99af06-d1c9-4e6a-821c-10cb16a5cc5d"; //bitcoin
                var user = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var assetDescription = walletApi.AssetDescription.GetAssetDescription(assetId, registeredClient.GetResponseObject().Result.Token);
                assetDescription.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(assetDescription.GetResponseObject().Result.FullName, Is.EquivalentTo("Bitcoin"), "unexpected asset name");
                Assert.That(assetDescription.GetResponseObject().Error, Is.Null, "Error is not null");
            }
        }

        public class GetAssetDescriptionInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234")]
            [TestCase("test")]
            [TestCase("!@#$")]
            [Category("WalletApi")]
            public void GetAssetDescriptionInvalidTokenTest(string token)
            {
                var assetId = "dd99af06-d1c9-4e6a-821c-10cb16a5cc5d"; //bitcoin
                
                var assetDescription = walletApi.AssetDescription.GetAssetDescription(assetId, token);
                assetDescription.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        #endregion

        #region asset pair

        public class GetAssetPair : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetPairTest()
            {
                var pair = "BTCUSD";
                var user = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var assetPair = walletApi.AssetPair.GetAssetPair(pair, registeredClient.GetResponseObject().Result.Token);
                assetPair.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(pair, Is.EqualTo(assetPair.GetResponseObject().Result.AssetPair.Id), "Unexpected asset pair ID");
            }
        }

        public class GetAssetPairInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234")]
            [TestCase("test")]
            [TestCase("!@#$%^")]
            [Category("WalletApi")]
            public void GetAssetPairInvalidTokenTest(string token)
            {
                var pair = "BTCUSD";

                var assetPair = walletApi.AssetPair.GetAssetPair(pair, token);
                assetPair.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class GetAssetPairInvalidPair : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetPairInvalidPairTest()
            {
                var pair = "invalidPair";
                var user = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var assetPair = walletApi.AssetPair.GetAssetPair(pair, registeredClient.GetResponseObject().Result.Token);
                assetPair.Validate.StatusCode(HttpStatusCode.InternalServerError);// is this response code expected?
            }
        }
        #endregion
    }
}
