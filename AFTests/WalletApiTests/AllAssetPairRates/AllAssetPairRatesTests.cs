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
    }
}
