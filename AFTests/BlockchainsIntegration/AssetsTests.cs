using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.BlockchainsIntegrationTests
{
    class AssetsTests
    {
        public class GetAssets : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetAssetsTest()
            {
                var response = blockchainApi.Assets.GetAssets("100",null);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Items.Count, Is.GreaterThanOrEqualTo(1), "Assets count is less then 1");
                Assert.That(response.Content, Does.Contain(ASSET_ID).IgnoreCase, $"{ASSET_ID} not present in asseets");
            }
        }

        public class GetAssetsInvalidTake : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("qwerty")]
            [TestCase("35,23")]
            [TestCase("!@*()")]
            [Category("BlockchainIntegration")]
            public void GetAssetsInvalidTakeTest(string take)
            {
                var response = blockchainApi.Assets.GetAssets(take, null);
                response.Validate.StatusCode(HttpStatusCode.BadRequest, $"Unexpected Status code {response.StatusCode} for take: {take}");
            }
        }

        public class GetAssetsContinuation : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetAssetsContinuationTest()
            {
                var cont = TestData.GenerateString(8);
                var response = blockchainApi.Assets.GetAssets("100", cont);
                Assert.That(response.StatusCode, Is.Not.EqualTo(HttpStatusCode.InternalServerError));
            }
        }

        public class GetAssetId : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetAssetsIdTest()
            {
                var response = blockchainApi.Assets.GetAsset(ASSET_ID);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().AssetId, Is.EqualTo(ASSET_ID).IgnoreCase);
            }
        }

        public class GetAssetInvalidId : BlockchainsIntegrationBaseTest
        {
            [TestCase("1234567")]
            [TestCase("testAssetId")]
            [TestCase("!@&*(")]
            [Category("BlockchainIntegration")]
            public void GetAssetInvalidIdTest(string assetId)
            {
                var response = blockchainApi.Assets.GetAsset(assetId);
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
