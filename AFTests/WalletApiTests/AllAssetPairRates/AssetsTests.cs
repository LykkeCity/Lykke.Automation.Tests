using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.AllAssetPairRates
{
    class AssetsTests
    {

        public class GetAssets : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetsTest()
            {
                var assets = walletApi.Assets.GetAssets();
                assets.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(assets.GetResponseObject().Result.Assets.Count, Is.GreaterThanOrEqualTo(2), "Assets count is less then 2");
            }
        }

        public class GetAssetById : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetByIdTest()
            {
                var assets = walletApi.Assets.GetAssets();
                assets.Validate.StatusCode(System.Net.HttpStatusCode.OK);
                var firstAsset = assets.GetResponseObject().Result.Assets[0];

                var asset = walletApi.Assets.GetAssetId(firstAsset.Id);
                asset.Validate.StatusCode(HttpStatusCode.OK);
                AreEqualByJson(firstAsset, asset.GetResponseObject().Result.Asset);
            }
        }

        public class GetAssetByInvalidId : WalletApiBaseTest
        {
            [TestCase("invalidId")]
            [TestCase("1234")]
            [TestCase("!@$$")]
            [Category("WalletApi")]
            public void GetAssetByInvalidIdTest(string assetId)
            {
                var asset = walletApi.Assets.GetAssetId(assetId);
                asset.Validate.StatusCode(HttpStatusCode.NotFound, "Need to verify if 500 is ok in case asset not found by id");
            }
        }

        public class GetAssetIdAttributes : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetIdAttributesTest()
            {
                var user = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var asset = walletApi.Assets.GetAssets().GetResponseObject().Result.Assets[0];
                var assetAttributes = walletApi.Assets.GetAssetIdAttributes(asset.Id, registeredClient.GetResponseObject().Result.Token);
                assetAttributes.Validate.StatusCode(HttpStatusCode.OK, "Take a look at this resource. Probably broken");
                var pairs = assetAttributes.GetResponseObject().Result.Pairs;
                //add validation
            }
        }

        public class GetAssetIdAttribute : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAssetIdAttributeTest()
            {
                var user = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var asset = walletApi.Assets.GetAssets().GetResponseObject().Result.Assets[0];
                var assetAttributes = walletApi.Assets.GetAssetIdAttributes(asset.Id, registeredClient.GetResponseObject().Result.Token);
                assetAttributes.Validate.StatusCode(HttpStatusCode.OK, "Fix that after Assets attributes resource will be fixed");
                var pairs = assetAttributes.GetResponseObject().Result.Pairs;
                //add validation
            }
        }

        public class PostAssetsDescriptionList : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostAssetsDescriptionListTest()
            {
                var randomCount = new Random().Next(10);
                var assets = walletApi.Assets.GetAssets().GetResponseObject().Result.Assets;
                var list = new List<string>();
                
                for(int i= 0; i<randomCount; i++)
                {
                    list.Add(assets[i].Id);
                }
                var assetDescriptionModel = new GetAssetDescriptionsListModel() { Ids = list };

                var response = walletApi.Assets.PostAssetsDescriptionList(assetDescriptionModel);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Result.Descriptions.Count, Is.EqualTo(randomCount));
                response.GetResponseObject().Result.Descriptions.ToList().ForEach(a =>
                { Assert.That(list.Contains(a.Id), $"list does not contain {a.Id}"); });
            }
        }
    }
}
