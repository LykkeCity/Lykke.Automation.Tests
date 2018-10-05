using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;

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

                var response = hft.Orders.GetOrders(OrderStatusQuery.InOrderBook, skip, take, ApiKey);
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
                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.NotFound, HttpStatusCode.BadRequest));
            }
        }

        public class GetOrderByValidId : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderByValidIdTest()
            {
                var request = new PlaceLimitOrderModel() { Price = 1.0, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 0.1 };

                var responseOrder = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                responseOrder.Validate.StatusCode(HttpStatusCode.OK);

                var id = responseOrder.GetResponseObject().Id.ToString();
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
                var request = new PlaceLimitOrderModel() { Price = 10, AssetPairId = AssetPair, OrderAction = OrderAction.Sell, Volume = 3.2 };

                var response = hft.Orders.PostOrdersLimitOrder(request, SecondWalletApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var requestSell = new PlaceMarketOrderModel() { Asset = FirstAssetId, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 2.3 };

                var responseSell = hft.Orders.PostOrdersMarket(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostOrdersMarketBuy : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void PostOrdersMarketBuyTest()
            {
                var request = new PlaceLimitOrderModel() { Price = 100, AssetPairId = AssetPair, OrderAction = OrderAction.Sell, Volume = 3.2 };

                var response = hft.Orders.PostOrdersLimitOrder(request, SecondWalletApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var requestBuy = new PlaceMarketOrderModel() { Asset = FirstAssetId, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 3.1 };

                var responseBuy = hft.Orders.PostOrdersMarket(requestBuy, ApiKey);
                Assert.That(responseBuy.GetResponseObject().Price, Is.Not.Null);
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
                var request = new PlaceMarketOrderModel() { Asset = asset, AssetPairId = AssetPair, OrderAction = OrderAction.Sell, Volume = 0.5 };

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
                var request = new PlaceMarketOrderModel() { Asset = SecondAssetId, AssetPairId = assetPair, OrderAction = OrderAction.Sell, Volume = 0.5 };

                var response = hft.Orders.PostOrdersMarket(request, ApiKey);
                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.NotFound, HttpStatusCode.BadRequest));
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
                var request = new PlaceMarketOrderModel() { Asset = SecondAssetId, AssetPairId = AssetPair, OrderAction = OrderAction.Sell, Volume = volume };

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
                var requests = new List<PlaceMarketOrderModel>();
                requests.Add(new PlaceMarketOrderModel() { Asset = SecondAssetId, AssetPairId = AssetPair, OrderAction = OrderAction.Sell });
                requests.Add(new PlaceMarketOrderModel() { Asset = SecondAssetId, OrderAction = OrderAction.Sell, Volume = 0.5 });
                requests.Add(new PlaceMarketOrderModel() { AssetPairId = AssetPair, OrderAction = OrderAction.Sell, Volume = 0.5 });
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
                var request = new PlaceLimitOrderModel() { Price = 1.0, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 0.1 };

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
                var request = new PlaceLimitOrderModel()
                { Price = 1.0, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 0.1 };

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
                var request1 = new PlaceLimitOrderModel()
                { Price = 1.0, AssetPairId = AssetPair, OrderAction = OrderAction.Buy };
                var request2 = new PlaceLimitOrderModel()
                { Price = 1.0, OrderAction = OrderAction.Buy, Volume = 0.1 };
                var request3 = new PlaceLimitOrderModel()
                { AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 0.1 };
                var request4 = new PlaceLimitOrderModel()
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
                var request = new PlaceLimitOrderModel() { Price = 10000000.0, AssetPairId = AssetPair, OrderAction = OrderAction.Sell, Volume = 0.5 };

                var limit = hft.Orders.PostOrdersLimitOrder(request, ApiKey);

                var id = limit.GetResponseObject().Id.ToString();

                var response = hft.Orders.DeleteOrder(id, ApiKey);
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
                var response = hft.Orders.DeleteOrder(id, ApiKey);
                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.NotFound, HttpStatusCode.BadRequest));
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
                var response = hft.Orders.DeleteOrder(id, "789-987-951");
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class GetOrderBooksInOrderBookStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksPlacedStatusTest()
            {
                var limitRequest = new PlaceLimitOrderModel()
                { Price = 1.0, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 0.1 };

                var limitResponse = hft.Orders.PostOrdersLimitOrder(limitRequest, ApiKey);
                limitResponse.Validate.StatusCode(HttpStatusCode.OK);
                var id = limitResponse.GetResponseObject().Id.ToString();

                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Status, Is.EqualTo(OrderStatus.Placed));
            }
        }

        //not enough funds
        public class GetOrderBooksNotEnoughFundsStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            [Ignore("Removed OrderStatus.NotEnoughFunds")]
            public void GetOrderBooksNotEnoughFundsStatusTest()
            {
                var limitRequest = new PlaceLimitOrderModel()
                { Price = 10000000.0, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 10000 };

                var limitResponse = hft.Orders.PostOrdersLimitOrder(limitRequest, ApiKey);
                limitResponse.Validate.StatusCode(HttpStatusCode.BadRequest);
                var id = limitResponse.JObject["Result"]["Id"].ToString();

                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                //Assert.That(response.GetResponseObject().Status, Is.EqualTo(OrderStatus.NotEnoughFunds));
            }
        }

        //unknow asset ?? {"Error":{"Code":"UnknownAsset","Field":null,"Message":"Unknown asset"}}
        public class GetOrderBooksUnknowAssetStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksUnknowAssetStatusTest()
            {
                var limitRequest = new PlaceLimitOrderModel()
                { Price = 1.0, AssetPairId = "BLRBTS", OrderAction = OrderAction.Buy, Volume = 0.1 };

                var limitResponse = hft.Orders.PostOrdersLimitOrder(limitRequest, ApiKey);
                Assert.That(limitResponse.StatusCode, Is.AnyOf(HttpStatusCode.NotFound, HttpStatusCode.BadRequest));
            }
        }

        // Lead to negative spread
        public class GetOrderBooksLeadToNegativeSpreadStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            [Ignore("Removed status OrderStatus.LeadToNegativeSpread")]
            public void GetOrderBooksLeadToNegativeSpreadStatusTest()
            {
                var limitRequest1 = new PlaceLimitOrderModel()
                { Price = 1.2, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 0.01 };

                var limitRequest2 = new PlaceLimitOrderModel()
                { Price = 0.8, AssetPairId = AssetPair, OrderAction = OrderAction.Sell, Volume = 0.01 };

                var limitResponse1 = hft.Orders.PostOrdersLimitOrder(limitRequest1, ApiKey);
                var limitResponse2 = hft.Orders.PostOrdersLimitOrder(limitRequest2, ApiKey);

                limitResponse2.Validate.StatusCode(HttpStatusCode.BadRequest);

                var id = limitResponse2.JObject["Result"]["Id"].ToString();

                var response = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                //Assert.That(() => hft.Orders.GetOrderById(id, ApiKey).GetResponseObject().Status, Is.EqualTo(OrderStatus.LeadToNegativeSpread).After(1 * 60 * 1000, 2 * 1000));
            }
        }

        //cancel 
        public class GetOrderBooksCancelStatus : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksCancelStatusTest()
            {
                var request = new PlaceLimitOrderModel() { Price = 0.01, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = 0.5 };

                var limit = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                limit.Validate.StatusCode(HttpStatusCode.OK);

                var id = limit.GetResponseObject().Id.ToString();

                var response = hft.Orders.DeleteOrder(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var responseOrder = hft.Orders.GetOrderById(id, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(() => hft.Orders.GetOrderById(id, ApiKey).GetResponseObject().Status, Is.EqualTo(OrderStatus.Cancelled).After(30 * 1000, 1 * 1000));
            }
        }

        public class PostOrdersStopLimit : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            [TestCase(OrderAction.Buy)]
            [TestCase(OrderAction.Sell)]
            public void PostOrdersStopLimitBuySellTest(OrderAction orderAction)
            {
                var request = new PlaceStopLimitOrderModel()
                {
                    AssetPairId = AssetPair,
                    OrderAction = orderAction,
                    Volume = 0.1,
                    LowerLimitPrice = 45,
                    LowerPrice = 46,
                    UpperLimitPrice = 77,
                    UpperPrice = 78
                };

                var response = hft.Orders.PostOrdersStopLimitOrder(request, ApiKey);
                var orderId = response.GetResponseObject().Id.ToString();
                var stopLimitOrder = hft.Orders.GetOrderById(orderId, ApiKey);
                var stopLimitObj = stopLimitOrder.GetResponseObject();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(stopLimitOrder.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(stopLimitObj.AssetPairId, Is.EqualTo(AssetPair));
                Assert.That(stopLimitObj.CreatedAt, Is.EqualTo(DateTime.Now).Within(3).Minutes);
                Assert.That(stopLimitObj.Type, Is.EqualTo(OrderType.StopLimit));
                Assert.That(stopLimitObj.Volume, Is.EqualTo(orderAction == OrderAction.Buy ? request.Volume : -request.Volume));
                Assert.That(stopLimitObj.LowerLimitPrice, Is.EqualTo(request.LowerLimitPrice));
                Assert.That(stopLimitObj.LowerPrice, Is.EqualTo(request.LowerPrice));
                Assert.That(stopLimitObj.UpperLimitPrice, Is.EqualTo(request.UpperLimitPrice));
                Assert.That(stopLimitObj.UpperPrice, Is.EqualTo(request.UpperPrice));
            }
        }

        public class PostOrdersStopLimitNegativeTests : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            [TestCase(true, OrderAction.Buy, 0.1, 45, 46, 77, 78, "InvalidApiKey$%10", ExpectedResult = HttpStatusCode.Unauthorized)]
            [TestCase(true, OrderAction.Buy, 0.1, 45, 46, 77, 78, "Test$%€§10", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(false, OrderAction.Buy, 0.1, 45, 46, 77, 78, "ApiKey", ExpectedResult = HttpStatusCode.NotFound)]
            // TODO: remove this comment when this issue is fixed 
            // Current response is InternalServerError
            // [TestCase(true, (OrderAction)10, 0.1, 45, 46, 77, 78, "ApiKey", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(true, OrderAction.Buy, 0, 45, 46, 77, 78, "ApiKey", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(true, OrderAction.Buy, 0.1, 0, 46, 77, 78, "ApiKey", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(true, OrderAction.Buy, 0.1, 45, 0, 77, 78, "ApiKey", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(true, OrderAction.Buy, 0.1, 45, 46, 0, 78, "ApiKey", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(true, OrderAction.Buy, 0.1, 45, 46, 77, 0, "ApiKey", ExpectedResult = HttpStatusCode.BadRequest)]
            public HttpStatusCode PostOrdersStopLimitNegativeTest(bool correctAssetPair, OrderAction orderAction, double volume, double lowerLimitPrice, double lowerPrice, double upperLimitPrice, double upperPrice, string apiKey )
            {
                var assetPairToUse = correctAssetPair ? AssetPair : "Test$%€§10";
                var keyToUse = apiKey == "ApiKey" ? ApiKey : apiKey;

                var request = new PlaceStopLimitOrderModel()
                {
                    AssetPairId = assetPairToUse,
                    OrderAction = orderAction,
                    Volume = volume,
                    LowerLimitPrice = lowerLimitPrice,
                    LowerPrice = lowerPrice,
                    UpperLimitPrice = upperLimitPrice,
                    UpperPrice = upperPrice
                };

                return hft.Orders.PostOrdersStopLimitOrder(request, keyToUse).StatusCode;
            }
        }
    }
}
