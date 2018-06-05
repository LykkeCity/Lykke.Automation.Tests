using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFTests.PrivateApiTests;
using Lykke.Client.AutorestClient.Models;
using MoreLinq;
using NUnit.Framework;
using XUnitTestCommon.Tests;

namespace AFTests.CandlexHistory
{
    class CandlesHistoryTest
    {
        public class CandleHistorySecond : CandlesHistoryBaseTest
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
                //to start from the minute
                if (DateTime.Now.Second > 10)
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(60 - DateTime.Now.Second));

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

                maxBuyPrice = Make5numberAfterDot(maxBuyPrice);
                minSellPrice = Make5numberAfterDot(minSellPrice);

                fromMoment = DateTime.Now.ToUniversalTime();

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
            public void CandleHistorySecondTest()
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

                newMinSellPrice = Make5numberAfterDot(newMinSellPrice);
                newMaxBuyPrice = Make5numberAfterDot(newMaxBuyPrice);

                var request = new LimitOrderRequest() { Price = newMaxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = 0.1 };

                var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var request1 = new LimitOrderRequest() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = 0.1 };

                var response1 = hft.Orders.PostOrdersLimitOrder(request1, ApiKey);
                response1.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));

                //check order in Candles
                
                //check candles
                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesMid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Mid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var expectedCloseMid = (candlesAsk.History.First().Close + candlesBid.History.First().Close) / 2;
                var expectedOpenMid = (candlesAsk.History.First().Open + candlesBid.History.First().Open) /2;
                var expectedLowMid = (candlesAsk.History.First().Low + candlesBid.History.First().Low) /2;

                expectedLowMid = Make5numberAfterDot(expectedLowMid);
                expectedOpenMid = Make5numberAfterDot(expectedOpenMid);
                expectedCloseMid = Make5numberAfterDot(expectedCloseMid);


                Assert.That(candlesAsk.History.Any(c => { return c.Close == newMinSellPrice; }), Is.True, $"Sell price {newMinSellPrice} is not present in Candles");
                Assert.That(candlesBid.History.Any(c => { return c.Close == newMaxBuyPrice; }), Is.True, $"Buy price {newMaxBuyPrice} is not present in Candles");

                Assert.That(candlesMid.History.First().Open, Is.EqualTo(expectedOpenMid), "Unexpected mid value");
                Assert.That(candlesMid.History.First().Close, Is.EqualTo(expectedCloseMid), "Unexpected mid value");
                Assert.That(candlesMid.History.First().Low, Is.EqualTo(expectedLowMid), "Unexpected mid value");
            }
        }

        public class CandleHistoryDifferentPeriod : CandlesHistoryBaseTest
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
                //to start from the minute
                if (DateTime.Now.Second > 10)
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(60 - DateTime.Now.Second));

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

                if (minSellPrice == double.MaxValue && maxBuyPrice == double.MinValue)
                {
                    maxBuyPrice = 1.0;
                    minSellPrice = 1.3;
                }

                // accuracy = 5

                maxBuyPrice = Make5numberAfterDot(maxBuyPrice);
                minSellPrice = Make5numberAfterDot(minSellPrice);

                fromMoment = DateTime.Now.ToUniversalTime();

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
            public void CandleHistoryDifferentPeriodTest()
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
                var newMinSellPrice = minSellPrice - (minSellPrice - middle) / 2;
                var newMaxBuyPrice = maxBuyPrice + (middle - maxBuyPrice) / 2;

                newMinSellPrice = Make5numberAfterDot(newMinSellPrice);

                newMaxBuyPrice = Make5numberAfterDot(newMaxBuyPrice);

                var request = new LimitOrderRequest() { Price = newMaxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = 0.1 };

                var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var request1 = new LimitOrderRequest() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = 0.1 };

                var response1 = hft.Orders.PostOrdersLimitOrder(request1, ApiKey);
                response1.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));

                //cancel orders
                var take = "100";
                var skip = "0";

                var orders = hft.Orders.GetOrders(OrderStatus.InOrderBook, skip, take, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                orders.GetResponseObject().ForEach(o => hft.Orders.PostOrdersCancelOrder(o.Id.ToString(), ApiKey));
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));

                //check order in Candles

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.That(candlesAsk.History.Any(c => { return c.Close == newMinSellPrice; }), Is.False, $"Sell price {newMinSellPrice} is Close price after cancelling order");
                Assert.That(candlesBid.History.Any(c => { return c.Close == newMaxBuyPrice; }), Is.False, $"Buy price {newMaxBuyPrice} is Close price after cancelling order");

                Assert.That(candlesAsk.History.Any(c => { return c.Close == minSellPrice; }), Is.True, $"Start Sell price {minSellPrice} is not Close price after cancelling order");
                Assert.That(candlesBid.History.Any(c => { return c.Close == maxBuyPrice; }), Is.True, $"Start Buy price {maxBuyPrice} is not Close price after cancelling order");

                // 
            }
        }

        // two trade wallet
        public class CandleHistoryTradeType : CandlesHistoryBaseTest
        {
            string ApiKey = "92ca97e5-93ff-4847-ae6e-aee488c3ca35";
            string SecondWalletApiKey = "1606b4dd-fe22-4425-92ea-dccd5fffcce8";
            string AssetPairId = "chfDEB";
            string SecondAssetId = "DEB";
            DateTime fromMoment;
            double tradingVolume = 0.1;

            /// <summary>
            /// set sell and buy prices to test candles
            /// </summary>
            [SetUp]
            public void SetUp()
            {
                //to start from the minute
                if (DateTime.Now.Second > 10)
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(60 - DateTime.Now.Second));

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

                if (minSellPrice == double.MaxValue && maxBuyPrice == double.MinValue)
                {
                    maxBuyPrice = 1.0;
                    minSellPrice = 1.3;
                }

                // accuracy = 5

                maxBuyPrice = Make5numberAfterDot(maxBuyPrice);
                minSellPrice = Make5numberAfterDot(minSellPrice);

                fromMoment = DateTime.Now.ToUniversalTime();

                var limitOrderRequestBuy = new LimitOrderRequest() { Price = maxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

                var response = hft.Orders.PostOrdersLimitOrder(limitOrderRequestBuy, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var limitOrderRequestSell = new LimitOrderRequest() { Price = minSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };

                var response1 = hft.Orders.PostOrdersLimitOrder(limitOrderRequestSell, ApiKey);
                response1.Validate.StatusCode(HttpStatusCode.OK);

                //wait to appear in orderbook
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));
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
            public void CandleHistoryTradeTypeTest()
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
                var newMinSellPrice = minSellPrice - (minSellPrice - middle) / 2;
                var newMaxBuyPrice = maxBuyPrice + (middle - maxBuyPrice) / 2;

                newMinSellPrice = Make5numberAfterDot(newMinSellPrice);
                newMaxBuyPrice = Make5numberAfterDot(newMaxBuyPrice);

                var request = new LimitOrderRequest() { Price = newMaxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

                var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var request1 = new LimitOrderRequest() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };

                var response1 = hft.Orders.PostOrdersLimitOrder(request1, ApiKey);
                response1.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                //check order in Candles

                //check candles
                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.That(candlesTrades.History.Count, Is.EqualTo(0), "There were trades in test period. Unexpected");

                //make trades
                var requestSecondWallet = new LimitOrderRequest() { Price = newMaxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };

                var responseSecondWallet = hft.Orders.PostOrdersLimitOrder(requestSecondWallet, SecondWalletApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var request1SecondWallet = new LimitOrderRequest() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

                var response1SecondWallet = hft.Orders.PostOrdersLimitOrder(request1SecondWallet, SecondWalletApiKey);
                response1.Validate.StatusCode(HttpStatusCode.OK);

                // validate trade candle
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.That(candlesTrades.History.Any(c => c.Low == newMaxBuyPrice), Is.True, "Low value is not expected");
                Assert.That(candlesTrades.History.Any(c => c.High == newMinSellPrice), Is.True, "High value is not expected");
                double realTradingVolume = 0;

                candlesTrades.History.ForEach(c => realTradingVolume += c.TradingVolume);

                Assert.That(realTradingVolume, Is.EqualTo(tradingVolume *2), "Unexpected trading volume value");
            }
        }

        static double Make5numberAfterDot(double input)
        {
            return (Math.Floor((input * Math.Pow(10, 5)) % Math.Pow(10, 5))) / Math.Pow(10, 5);
        }
    }
}
