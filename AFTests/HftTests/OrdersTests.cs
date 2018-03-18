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
                var request = new LimitOrderRequest() { Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var responseOrder = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                responseOrder.Validate.StatusCode(HttpStatusCode.OK);

                var id = responseOrder.Content.Replace("\"","");
                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
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
                var request = new LimitOrderRequest()
                { Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostOrdersLimitNegative : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersLimitNegativeTest()
            {
                var request1 = new LimitOrderRequest()
                { Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy };
                var request2 = new LimitOrderRequest()
                { Price = 1.0, OrderAction = OrderAction.Buy, Volume = 0.1 };
                var request3 = new LimitOrderRequest()
                { AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };
                var request4 = new LimitOrderRequest()
                { };

                Assert.Multiple(() => 
                {
                    Assert.That(hft.Orders.PostOrdersLimitOrder(request1, ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(hft.Orders.PostOrdersLimitOrder(request2, ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(hft.Orders.PostOrdersLimitOrder(request3, ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(hft.Orders.PostOrdersLimitOrder(request4, ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }

        public class PostOrdersCancelLimit : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersCancelLimitTest()
            {
                var request = new LimitOrderRequest() { Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.5 };

                var limit = hft.Orders.PostOrdersLimitOrder(request, ApiKey);

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

        public class GetOrderBooksInOrderBookStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksInOrderBookStatusTest()
            {
                var limitRequest = new LimitOrderRequest()
                { Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var limitResponse = hft.Orders.PostOrdersLimitOrder(limitRequest, ApiKey);
                limitResponse.Validate.StatusCode(HttpStatusCode.OK);
                var id = limitResponse.Content.Replace("\"","");

                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Status, Does.Contain(OrderStatus.InOrderBook.ToString()).IgnoreCase);
            }
        }

        //not enough funds
        public class GetOrderBooksNotEnoughFundsStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksNotEnoughFundsStatusTest()
            {
                var limitRequest = new LimitOrderRequest()
                { Price = 100000.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var limitResponse = hft.Orders.PostOrdersLimitOrder(limitRequest, ApiKey);
                limitResponse.Validate.StatusCode(HttpStatusCode.BadRequest);
                var id = limitResponse.JObject.Property("Result").Value.ToString();

                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Status, Does.Contain(OrderStatus.NotEnoughFunds.ToString()).IgnoreCase);
            }
        }

        //unknow asset ?? {"Error":{"Code":"UnknownAsset","Field":null,"Message":"Unknown asset"}}
        public class GetOrderBooksUnknowAssetStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksUnknowAssetStatusTest()
            {
                var limitRequest = new LimitOrderRequest()
                { Price = 1.0, AssetPairId = "BLRBTS", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var limitResponse = hft.Orders.PostOrdersLimitOrder(limitRequest, ApiKey);
                Assert.That(limitResponse.Content, Does.Contain("Unknown asset").IgnoreCase);
            }
        }

        // Lead to negative spread
        public class GetOrderBooksLeadToNegativeSpreadStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksLeadToNegativeSpreadStatusTest()
            {
                //{
                //                    "AssetPairId": "USDCHF",
                //  "OrderAction": "Buy",
                //  "Volume": 1,
                //  "Price": 1.2
                //}

                //                {
                //                    "AssetPairId": "USDCHF",
                //  "OrderAction": "Sell",
                //  "Volume": 1,
                //  "Price": 0.8
                //                }

                var limitRequest1 = new LimitOrderRequest()
                { Price = 1.2, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.000001 };

                var limitRequest2 = new LimitOrderRequest()
                { Price = 0.8, AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.000001 };

                var limitResponse1 = hft.Orders.PostOrdersLimitOrder(limitRequest1, ApiKey);
                var limitResponse2 = hft.Orders.PostOrdersLimitOrder(limitRequest2, ApiKey);

                limitResponse2.Validate.StatusCode(HttpStatusCode.BadRequest);
                var id = limitResponse2.JObject.Property("Result").Value.ToString();

                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(() => hft.Orders.GetOrderById(id, ApiKey).GetResponseObject().Status, Does.Contain(OrderStatus.LeadToNegativeSpread.ToString()).IgnoreCase.After(1*60*1000, 2*1000));


                //var limitRequest = new LimitOrderRequest()
                //{ Price = 1.0, AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.0000001 };

                //var limitResponse = hft.Orders.PostOrdersLimitOrder(limitRequest, ApiKey);
                //limitResponse.Validate.StatusCode(HttpStatusCode.BadRequest);
                //var id = limitResponse.JObject.Property("Result").Value.ToString();

                //var response = hft.Orders.GetOrderById(id, ApiKey);
                //response.Validate.StatusCode(HttpStatusCode.OK);
                //Assert.That(response.GetResponseObject().Status, Does.Contain(OrderStatus.LeadToNegativeSpread.ToString()).IgnoreCase);
            }
        }

        //cancel 
        public class GetOrderBooksCancelStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksCancelStatusTest()
            {
                var request = new LimitOrderRequest() { Price = 0.01, AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = 0.5 };

                var limit = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                limit.Validate.StatusCode(HttpStatusCode.OK);

                var id = limit.Content.Replace("\"","");

                var response = hft.Orders.PostOrdersCancelOrder(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var responseOrder = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(() => hft.Orders.GetOrderById(id, ApiKey).GetResponseObject().Status, Does.Contain(OrderStatus.Cancelled.ToString()).IgnoreCase.After(30*1000, 1*1000));
            }
        }
    }
}
