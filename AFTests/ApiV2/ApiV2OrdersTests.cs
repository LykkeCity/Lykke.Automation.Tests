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

        [Test]
        [Category("ApiV2")]
        public void PostOrdersMarket()
        {
            Step("Make POST orders/market and validate response", () => 
            {
                var model = new MarketOrderRequest
                {
                    AssetId = "BTC",
                    AssetPairId = "BTCUSD",
                    OrderAction = OrderAction.Sell,
                    Volume = 0.01
                };

                var response = apiV2.Orders.PostOrdersMarket(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteOrdersLimit()
        {
            Step("Make DELETE /api/orders/limit and validate response", () => 
            {
                var model = new LimitOrderCancelMultipleRequest
                {
                    AssetPairId = "BTCUSD"
                };

                var response = apiV2.Orders.DeleteOrdersLimit(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteOrdersWrongOrderLimit()
        {
            Step("Make DELETE /api/orders/limit  with invalida assetPairId and validate response", () =>
            {
                var model = new LimitOrderCancelMultipleRequest
                {
                    AssetPairId = Guid.NewGuid().ToString()
                };

                var response = apiV2.Orders.DeleteOrdersLimit(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostOrdersLimit()
        {
            Step("Make POST /api/orders/limit and validate reqeust", () => 
            {
                var model = new LimitOrderRequest
                {
                    AssetPairId = "BTCUSD",
                    OrderAction = OrderAction.Buy,
                    Price = 1,
                    Volume = 1
                };

                var response = apiV2.Orders.PostOrdersLimit(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostOrdersStopLimit()
        {
            Step("Make POST /api/orders/stoplimit and validate response", () => 
            {
                var model = new StopLimitOrderRequest
                {
                    AssetPairId = "BTCUSD",
                    LowerLimitPrice = 100,
                    LowerPrice = 150,
                    OrderAction = OrderAction.Sell,
                    UpperLimitPrice = 500,
                    UpperPrice = 450,
                    Volume = 0.1
                };

                var response = apiV2.Orders.PostOrdersStopLimit(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteLimitOrderInvalidOrderIdGuid()
        {
            var orderId = Guid.NewGuid().ToString();

            Step($"Make DELETE /api/orders/limit/{orderId} and validate response", () => 
            {
                var response = apiV2.Orders.DeleteOrdersLimitOrder(orderId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [TestCase("123456")]
        [TestCase("0000")]
        [TestCase("+-*/")]
        [TestCase("'%^")]
        [Category("ApiV2")]
        public void DeleteLimitOrderInvalidOrderIds(string orderId)
        {
            Step($"Make DELETE /api/orders/limit/{orderId} and validate response", () =>
            {
                var response = apiV2.Orders.DeleteOrdersLimitOrder(orderId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteLimitOrderValidOrder()
        {
            var orderId = "wrongOrderId";

            Step("Make StopLimitOrderRequest and get valid orderId", () => 
            {
                var model = new StopLimitOrderRequest
                {
                    AssetPairId = "BTCUSD",
                    LowerLimitPrice = 100,
                    LowerPrice = 150,
                    OrderAction = OrderAction.Sell,
                    UpperLimitPrice = 500,
                    UpperPrice = 450,
                    Volume = 0.1
                };

                var response = apiV2.Orders.PostOrdersStopLimit(model, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

                orderId = response.Content.Replace("\"", "");
            });

            Step($"Make DELETE /api/orders/limit/{orderId}", () => 
            {
                var response = apiV2.Orders.DeleteOrdersLimitOrder(orderId, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}
