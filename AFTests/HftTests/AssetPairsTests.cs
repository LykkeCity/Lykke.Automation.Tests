namespace AFTests.HftTests
{
    using NUnit.Framework;
    using System;
    using System.Net;

    class AssetPairsTests
    {
        public class GetAssetPairs : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetAssetPairsTest()
            {
                var response = hft.AssetPairs.GetAssetPairs().Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.ResponseObject.Count, Is.GreaterThan(5), "AssetPairs Count less than 5??");
            }
        }

        public class GetAssetPairsInvalidId : HftBaseTest
        {
            [TestCase("123456")]
            [TestCase("testAssetId")]
            [TestCase("!@%^&(&*)$%€§1")]
            [Category("HFT")]
            public void GetAssetPairsInvalidIdTest(string id)
            {
                hft.AssetPairs.GetAssetPairs(id).Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class GetAssetPairValidateAsset : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetAssetPairValidateAssetTest()
            {
                var response = hft.AssetPairs.GetAssetPairs().Validate.StatusCode(HttpStatusCode.OK);
                var number = new Random().Next(response.ResponseObject.Count);
                var assetFromList = response.ResponseObject[number];
                var assetResponse = hft.AssetPairs.GetAssetPairs(assetFromList.Id).Validate.StatusCode(HttpStatusCode.OK);

                AreEqualByJson(assetFromList, assetResponse.ResponseObject, $"AssetPair {assetFromList.Id} from /asserPairs and from /assetPairs/assetId does not equal");
            }
        }
    }
}
