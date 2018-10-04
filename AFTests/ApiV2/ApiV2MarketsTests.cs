using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    class ApiV2MarketsTests
    {
        public class GetMarkets : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetMarketsTest()
            {
                Step("Make GET /api/markets request and validate response", () => 
                {
                    var response = apiV2.Markets.GetMarkets();
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject(), Is.Not.Null);
                    Assert.That(response.GetResponseObject().Count, Is.GreaterThan(0));
                });
            }
        }

        public class GetMarketsAssetPairId : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetMarketsAssetPairIdTest()
            {
                var assetPairId = "BTCUSD";
                Step($"Make GET /api/markets/{assetPairId} and validate response", () => 
                {
                    var response = apiV2.Markets.GetMarketsAssetPairId(assetPairId);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetMarketsInvalidAssetPairId : ApiV2BaseTest
        {
            [TestCase("123456789")]
            [TestCase("000000000")]
            [TestCase("invalidAssetPairId")]
            [TestCase("())(@")]
            [Category("ApiV2")]
            public void GetMarketInvalidsAssetPairIdTest(object assetPairId)
            {
                Step($"Make GET /api/markets/{assetPairId} and validate response", () =>
                {
                    var response = apiV2.Markets.GetMarketsAssetPairId(assetPairId.ToString());
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }
    }
}
