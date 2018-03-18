using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.HftTests
{
    class AssetPairsTests
    {
        public class GetAssetPairs : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetAssetPairsTest()
            {
                var response = hft.AssetPairs.GetAssetPairs();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Count, Is.GreaterThan(5), "AssetPairs Count less than 5??");
            }
        }

        public class GetAssetPairsInvalidId : HftBaseTest
        {
            [TestCase("123456")]
            [TestCase("testAssetId")]
            [TestCase("!@%^&(")]
            [Category("HFT")]
            public void GetAssetPairsInvalidIdTest(string id)
            {
                var response = hft.AssetPairs.GetAssetPairs(id);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class GetAssetPairValidateAsset : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetAssetPairValidateAssetTest()
            {
                var response = hft.AssetPairs.GetAssetPairs();
                response.Validate.StatusCode(HttpStatusCode.OK);

                var number = new Random().Next(response.GetResponseObject().Count);

                var assetFromList = response.GetResponseObject()[number];

                var assetResponse = hft.AssetPairs.GetAssetPairs(assetFromList.Id);
                assetResponse.Validate.StatusCode(HttpStatusCode.OK);
                AreEqualByJson(assetFromList, assetResponse.GetResponseObject(), $"AssetPair {assetFromList.Id} from /asserPairs and from /assetPairs/assetId does not equal");
            }
        }
    }
}
