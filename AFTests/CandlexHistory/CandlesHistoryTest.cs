using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFTests.PrivateApiTests;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using XUnitTestCommon.Tests;

namespace AFTests.CandlexHistory
{
    class CandlesHistoryTest
    {
        public class CandleHistory : PrivateApiBaseTest
        {
            string ApiKey = "92ca97e5-93ff-4847-ae6e-aee488c3ca35";
            string AssetPairId = "chfDEB";
            string SecondAssetId = "DEB";
            DateTime fromMoment;

            /// <summary>
            /// set sell and buy prices to test candles
            /// </summary>
            [SetUp]
            public void SetUp()
            {
                var orderBooks = hft.OrderBooks.GetOrderBooks(AssetPairId).GetResponseObject();

                var minSellPrice = Double.MaxValue;
                var maxBuyPrice = Double.MinValue;

                orderBooks.FindAll(o => o.IsBuy == true).ForEach(o =>
                {
                    o.Prices.ToList().ForEach(p =>
                    {
                        if (p.Price > maxBuyPrice)
                            maxBuyPrice = p.Price;
                    });
                });

                orderBooks.FindAll(o => o.IsBuy == false).ForEach(o =>
                {
                    o.Prices.ToList().ForEach(p =>
                    {
                        if (p.Price < minSellPrice)
                            minSellPrice = p.Price;
                    });
                });

                if (maxBuyPrice == double.MinValue && minSellPrice != double.MaxValue)
                    maxBuyPrice = 0.9 * minSellPrice;

                if (minSellPrice == double.MaxValue && maxBuyPrice != double.MinValue)
                    minSellPrice = 1.1 * maxBuyPrice;

                if(minSellPrice == double.MaxValue && maxBuyPrice == double.MinValue)
                {
                    maxBuyPrice = 1.0;
                    minSellPrice = 1.3;
                }

                // accuracy = 5

                maxBuyPrice = ((maxBuyPrice * Math.Pow(10, 5)) % Math.Pow(10, 5) ) / Math.Pow(10, 5);
                minSellPrice = ((minSellPrice * Math.Pow(10, 5)) % Math.Pow(10, 5)) / Math.Pow(10, 5);

                fromMoment = DateTime.Now.AddSeconds(-3).ToUniversalTime();

                var limitOrderRequestBuy = new LimitOrderRequest() { Price = maxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = 0.1 };

                var response = hft.Orders.PostOrdersLimitOrder(limitOrderRequestBuy, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var limitOrderRequestSell = new LimitOrderRequest() { Price = minSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = 0.1 };

                var response1 = hft.Orders.PostOrdersLimitOrder(limitOrderRequestSell, ApiKey);
                response1.Validate.StatusCode(HttpStatusCode.OK);

                //wait to appear in orderbook
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            [TearDown]
            public void TearDown()
            {
                var take = "100";
                var skip = "0";

                var response = hft.Orders.GetOrders(OrderStatus.InOrderBook, skip, take, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                response.GetResponseObject().ForEach(o => hft.Orders.PostOrdersCancelOrder(o.Id.ToString(), ApiKey));
            }

            [Test]
            [Category("CandleHistory")]
            public void CandleHistoryTest()
            {
                var orderBooks = hft.OrderBooks.GetOrderBooks(AssetPairId).GetResponseObject();

                var minSellPrice = Double.MaxValue;
                var maxBuyPrice = Double.MinValue;

                orderBooks.FindAll(o => o.IsBuy == true).ForEach(o =>
                {
                    o.Prices.ToList().ForEach(p =>
                    {
                        if (p.Price > maxBuyPrice)
                            maxBuyPrice = p.Price;
                    });
                });

                orderBooks.FindAll(o => o.IsBuy == false).ForEach(o =>
                {
                    o.Prices.ToList().ForEach(p =>
                    {
                        if (p.Price < minSellPrice)
                            minSellPrice = p.Price;
                    });
                });

                var middle = (maxBuyPrice + minSellPrice) / 2;

                //move sell price down and buy price up
                var newMinSellPrice = minSellPrice - (minSellPrice - middle ) / 2;
                var newMaxBuyPrice = maxBuyPrice + (middle - maxBuyPrice) / 2;

                newMinSellPrice = (Math.Floor((newMinSellPrice * Math.Pow(10, 5)) % Math.Pow(10, 5))) / Math.Pow(10, 5);
                newMaxBuyPrice = (Math.Floor((newMaxBuyPrice * Math.Pow(10, 5)) % Math.Pow(10, 5))) / Math.Pow(10, 5);

                var request = new LimitOrderRequest() { Price = newMaxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = 0.1 };

                var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var request1 = new LimitOrderRequest() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = 0.1 };

                var response1 = hft.Orders.PostOrdersLimitOrder(request1, ApiKey);
                response1.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));

                //check order in Candles

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.That(candlesAsk.History.Any(c => { return c.Close == newMinSellPrice; }), Is.True, $"Sell price {newMinSellPrice} is not present in Candles");
                Assert.That(candlesBid.History.Any(c => { return c.Close == newMaxBuyPrice; }), Is.True, $"Buy price {newMaxBuyPrice} is not present in Candles");
            }
        }
    }
}
