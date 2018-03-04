using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.HftTests
{
    class OrdersTests
    {
        public class GetOrders : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrdersTest()
            {
                string assetId = "AHAU100";
                var take = "10";
                var skip = "0";

                var response = hft.Orders.GetOrders(OrderStatus.InOrderBook, skip, take, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class GetOrderByInvalidId : HftBaseTest
        {
            [TestCase("invalidId")]
            [TestCase("00000")]
            [TestCase("-125")]
            [TestCase("15.25")]
            [TestCase("!@^&*(")]
            [Category("HFT")]
            public void GetOrderByInvalidIdTest(string id)
            {
                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class GetOrderByValidId : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderByValidIdTest()
            {
                var id = "some valid id";
                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class PostOrdersMarket : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersMarketTest()
            {
                var request = new MarketOrderRequest() {Asset = "CHF", AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.2 };

                var response = hft.Orders.PostOrdersMarket(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Result, Is.Not.Null);

                var requestSell = new MarketOrderRequest() { Asset = "CHF", AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var responseSell = hft.Orders.PostOrdersMarket(requestSell, ApiKey);
            }
        }

        public class PostOrdersMarketBuy : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersMarketBuyTest()
            {
                var request = new MarketOrderRequest() { Asset = "CHF", AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.2 };

                var response = hft.Orders.PostOrdersMarket(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var requestBuy = new MarketOrderRequest() { Asset = "CHF", AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1};

                var responseBuy = hft.Orders.PostOrdersMarket(requestBuy, ApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(responseBuy.GetResponseObject().Result, Is.Not.Null);
            }
        }

        public class PostOrdersMarketWrongAsset : HftBaseTest
        {
            [TestCase("1234")]
            [TestCase("invalidAsset")]
            [TestCase("")]
            [TestCase("!^&*(")]
            [Category("HFT")]
            public void PostOrdersMarketWrongAssetTest(string asset)
            {
                var request = new MarketOrderRequest() { Asset = asset, AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.5 };

                var response = hft.Orders.PostOrdersMarket(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class PostOrdersMarketWrongAssetPair : HftBaseTest
        {
            [TestCase("1234")]
            [TestCase("invalidAsset")]
            [TestCase("")]
            [TestCase("!^&*(")]
            [Category("HFT")]
            public void PostOrdersMarketWrongAssetPairTest(string assetPair)
            {
                var request = new MarketOrderRequest() { Asset = "CHF", AssetPairId = assetPair, OrderAction = OrderAction.Sell, Volume = 0.5 };

                var response = hft.Orders.PostOrdersMarket(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class PostOrdersMarketWrongVolume : HftBaseTest
        {
            [TestCase(0000)]
            [TestCase(0.00000000001)]
            [TestCase(-15.2)]
            [Category("HFT")]
            public void PostOrdersMarketWrongVolumeTest(double volume)
            {
                var request = new MarketOrderRequest() { Asset = "CHF", AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = volume };

                var response = hft.Orders.PostOrdersMarket(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class PostOrdersMarketNotFullRequest : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersMarketNotFullRequestTest()
            {
                var requests = new List<MarketOrderRequest>();
                requests.Add(new MarketOrderRequest() { Asset = "CHF", AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell });
                requests.Add(new MarketOrderRequest() { Asset = "CHF", OrderAction = OrderAction.Sell, Volume = 0.5 });
                requests.Add(new MarketOrderRequest() { AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.5 });
                Assert.Multiple(() => 
                {
                    Assert.That(hft.Orders.PostOrdersMarket(requests[0], ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(hft.Orders.PostOrdersMarket(requests[1], ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(hft.Orders.PostOrdersMarket(requests[2], ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }

        public class PostOrdersLimitBuy : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersLimitBuyTest()
            {
                var request = new LimitOrderRequest() {Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostOrdersLimitSell : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersLimitSellTest()
            {
                var request = new LimitOrderRequest() { Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        //add negative limit test

        public class PostOrdersCancelLimit : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersCancelLimitTest()
            {
                var request = new LimitOrderRequest() { Price = 1.0, AssetPairId = "AUDEUR", OrderAction = OrderAction.Sell, Volume = 0.5 };

                var limit = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                //limit.Validate.StatusCode(HttpStatusCode.OK);

                var id = "";
                try { id = limit.JObject.Property("Result").Value.ToString(); } catch (Exception) { }

                var response = hft.Orders.PostOrdersCancelOrder(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostOrdersCancelInvalidId : HftBaseTest
        {
            [TestCase("1234567")]
            [TestCase("invalidId")]
            [TestCase("!@^&*()")]
            [TestCase("00000")]
            [TestCase("-15543")]
            [TestCase("15.36")]
            [Category("HFT")]
            public void PostOrdersCancelInvalidIdTest(string id)
            {
                var response = hft.Orders.PostOrdersCancelOrder(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class PostOrdersCancelInvalidIdInvalidKey : HftBaseTest
        {
            [TestCase("1234567")]
            [TestCase("invalidId")]
            [TestCase("!@^&*()")]
            [TestCase("00000")]
            [TestCase("-15543")]
            [TestCase("15.36")]
            [Category("HFT")]
            public void PostOrdersCancelInvalidIdInvalidKeyTest(string id)
            {
                var response = hft.Orders.PostOrdersCancelOrder(id, "789-987-951");
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        //add tests with invalid api-key

        //add negavtive test: try to cancel market order
    }
}
