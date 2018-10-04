using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2OrdersTests : ApiV2TokenBaseTest
    {
        [Test]
        [Category("ApiV2")]
        public void GetOrders()
        {
            Step("Make GET /api/orders request and validate response", () => 
            {
                var response = apiV2.Orders.GetOrders("20", "50", token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostOrdersLimitCancel()
        {
            var orderId = "not exist";
            var assetPairId = "not found";

            Step("Make GET /api/AssetPairs and find BTCUSD assetPairId", () => 
            {
                var response = apiV2.AssetPairs.GetAssetPairs();
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            assetPairId = response.GetResponseObject().AssetPairs.ToList().FirstOrDefault(a => { return a.BaseAssetId.ToLower() == "btc" && a.QuotingAssetId.ToLower() == "usd"; })?.Id;
            });

            Step("Make POST /api/orders/limit and get orderId", () => 
            {
                var model = new LimitOrderRequest
                {
                    AssetPairId = "BTCUSD",
                    OrderAction = OrderAction.Sell,
                    Price = 5800,
                    Volume = 0.100                    
                };

                var response = apiV2.Orders.PostOrdersLimit(model, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });

            Step("Make POST /api/orders/limit/{orderId}/cancel", () => 
            {
                var response = apiV2.Orders.PostOrdersLimitOrderCancel("", token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}
