using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    class ApiV2OrderbookTests
    {
        public class GetOrderBook : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetOrderBookTest()
            {
                var assetPairId = "BTCUSD";
                Step($"Make GET /api/Orderbook with {assetPairId} and validate response", () => 
                {
                    var response = apiV2.Orderbook.GetOrderbook(assetPairId);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                });
            }
        }

        public class GetOrderBookInvalidAssetPair : ApiV2BaseTest
        {
            [TestCase("00000")]
            [TestCase("-12545")]
            [TestCase("X   A")]
            [TestCase("()(")]
            [TestCase("^()#!")]
            [Category("ApiV2")]
            public void GetOrderBookInvalidAssetPairTest(string invalidAssetPairId)
            {
                Step($"Make GET /api/Orderbook/{invalidAssetPairId} and validate response is BadRequest", () => 
                {
                    var response = apiV2.Orderbook.GetOrderbook(invalidAssetPairId);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                });
            }
        }
    }
}
