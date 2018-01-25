using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.BlockchainsIntegrationTests.LiteCoin
{
    class AssetsTests
    {
        public class GetAssets : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void GetAssetsTest()
            {
                var response = litecoinApi.Assets.GetAssets("2",null);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Items.Count, Is.GreaterThanOrEqualTo(1), "Assets count is less then 1");
                Assert.That(response.Content, Does.Contain("LTC"), "LTC not present in asseets");
            }
        }

        public class GetAssetsInvalidTake : LitecoinBaseTest
        {
            [TestCase("")]
            [TestCase("qwerty")]
            [TestCase("35,23")]
            [TestCase("!@*()")]
            [Category("Litecoin")]
            public void GetAssetsInvalidTakeTest(string take)
            {
                var response = litecoinApi.Assets.GetAssets(take, null);
                response.Validate.StatusCode(HttpStatusCode.BadRequest, $"Unexpected Status code {response.StatusCode} for take: {take}");
            }
        }

        public class GetAssetsContinuation : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void GetAssetsContinuationTest()
            {
                var cont = TestData.GenerateString(8);
                var response = litecoinApi.Assets.GetAssets("2", cont);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Items.Count, Is.GreaterThanOrEqualTo(1), "Assets count is less then 1");
                Assert.That(response.Content, Does.Contain("LTC"), "LTC not present in asseets");
            }
        }

        public class GetAssetId : LitecoinBaseTest
        {
            [TestCase("LTC")]
            [Category("Litecoin")]
            public void GetAssetsIdTest(string assetId)
            {
                var response = litecoinApi.Assets.GetAsset(assetId);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Name, Is.EqualTo("LiteCoin"));
            }
        }

        public class GetAssetInvalidId : LitecoinBaseTest
        {
            [TestCase("1234567")]
            [TestCase("testAssetId")]
            [TestCase("!@&*(")]
            [Category("Litecoin")]
            public void GetAssetInvalidIdTest(string assetId)
            {
                var response = litecoinApi.Assets.GetAsset(assetId);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
