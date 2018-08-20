using System;
using System.Collections.Generic;
using System.Globalization;
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
        public class LOSellCandle : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void LOBuyCandleTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };

                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var requestBuy = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

                var responseBuy = hft.Orders.PostOrdersLimitOrder(requestBuy, SecondWalletApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
               {
                   Assert.That(candlesTrades.History.Select(c => c.Close), Does.Contain(newMinSellPrice), "Close price does not contain new min sell price");
                   Assert.That(candlesTrades.History.Select(c => c.TradingVolume), Does.Contain(tradingVolume), "does not contain trading volume");
                   Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingOppositeVolume)), Does.Contain((Decimal(tradingVolume) * (Decimal(newMinSellPrice)))), "does not contain trading volume * sell price");
               });
            }
        }

        public class LOBuyCandle : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void LOSellCandleTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                var requestBuy = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

                var responseBuy = hft.Orders.PostOrdersLimitOrder(requestBuy, ApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };

                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, SecondWalletApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => c.Close), Does.Contain(newMinSellPrice), "Close price does not contain new min sell price");
                    Assert.That(candlesTrades.History.Select(c => c.TradingVolume), Does.Contain(tradingVolume), "does not contain trading volume");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingOppositeVolume)), Does.Contain((Decimal(tradingVolume) * Decimal(newMinSellPrice))), "does not contain trading volume * sell price");
                });
            }
        }

        public class MOBuyInvertedCandle : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void MOBuyInvertedCandleTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                (var marketSell, var marketBuy) = currentMinMaxPrices();

                var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = 0.1 };

                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var requestBuy = new PlaceMarketOrderModel() {  AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume /2, Asset = SecondAssetId };

                var responseBuy = hft.Orders.PostOrdersMarket(requestBuy, SecondWalletApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.Close)), Does.Contain(Decimal(marketBuy)), "Close price does not contain new min sell price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingOppositeVolume)), Does.Contain(Decimal(tradingVolume/2)), "does not contain trading volume");
                });
            }
        }

        public class MOBuyCandle : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void MOBuyTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                (var marketSell, var marketBuy) = currentMinMaxPrices();

                var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };

                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));
                // MO
                var requestBuy = new PlaceMarketOrderModel() { AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume / 2, Asset = FirstAssetId };

                var responseBuy = hft.Orders.PostOrdersMarket(requestBuy, SecondWalletApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => c.Close), Does.Contain(newMinSellPrice), "Close price does not contain new min sell price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingVolume)), Does.Contain(Decimal(tradingVolume / 2)), "does not contain trading volume");
                });
            }
        }

        public class MOSellCandle : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void MOSellTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                (var marketSell, var marketBuy) = currentMinMaxPrices();

                var requestBuy = new PlaceLimitOrderModel() { Price = newMaxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

                var responseBuy = hft.Orders.PostOrdersLimitOrder(requestBuy, ApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));
                // MO
                var requestSell = new PlaceMarketOrderModel() { AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume / 2, Asset = FirstAssetId };

                var responseSell = hft.Orders.PostOrdersMarket(requestSell, SecondWalletApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => c.Close), Does.Contain(newMaxBuyPrice), "Close price does not contain new min sell price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingVolume)), Does.Contain(Decimal(tradingVolume / 2)), "does not contain trading volume");
                });
            }
        }

        public class MOSellInvertedCandle : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void MOSellInvertedTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                (var marketSell, var marketBuy) = currentMinMaxPrices();

                //
                var candlesAsk1 = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid1 = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();
                //

                var requestSell1user = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = 0.1 };

                var responseSell1user = hft.Orders.PostOrdersLimitOrder(requestSell1user, ApiKey);
                responseSell1user.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));
 
                var requestSell = new PlaceMarketOrderModel() { AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume / 2, Asset = SecondAssetId };

                var responseSell = hft.Orders.PostOrdersMarket(requestSell, SecondWalletApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var prices = new List<decimal>();
                candlesTrades.History.ForEach(p => {
                    prices.Add(Decimal(p.Close));
                    prices.Add(Decimal(p.Open));
                    prices.Add(Decimal(p.High));
                    prices.Add(Decimal(p.Low));
                });

                Assert.Multiple(() =>
                {
                    Assert.That(prices, Does.Contain(Decimal(newMinSellPrice)), "Close price does not contain new min sell price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingOppositeVolume)), Does.Contain(Decimal(tradingVolume / 2)), "does not contain trading volume");
                });
            }
        }

        //
        public class LOSellPartiallyExecutionCandle : CandlesHistoryBaseTest
        {
            int partialCount = 10;
            [Test]
            [Category("CandleHistory")]
            public void LOSellPartiallyExecutionTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume * partialCount };

                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var requestBuy = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

                for(var i=0; i < partialCount; i++) { 
                    var responseBuy = hft.Orders.PostOrdersLimitOrder(requestBuy, SecondWalletApiKey);
                    responseBuy.Validate.StatusCode(HttpStatusCode.OK);
                }
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.Close)), Does.Contain(Decimal(newMinSellPrice)), "Close price does not contain new min sell price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingVolume)), Does.Contain(Decimal(tradingVolume) * Decimal(partialCount)), "does not contain trading volume");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingOppositeVolume)), Does.Contain(Decimal(tradingVolume) * Decimal(newMinSellPrice) * Decimal(partialCount)), "does not contain trading volume * sell price");
                });
            }
        }

        public class LOSellPartiallyMarketOrderExecutionCandle : CandlesHistoryBaseTest
        {
            int partialCount = 10;
            [Test]
            [Category("CandleHistory")]
            public void LOSellPartiallyMarketOrderExecutionCandleTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();
                var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume * partialCount };

                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var requestBuy = new PlaceMarketOrderModel() {AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume, Asset = FirstAssetId };

                for (var i = 0; i < partialCount; i++)
                {
                    var responseBuy = hft.Orders.PostOrdersMarket(requestBuy, SecondWalletApiKey);
                    responseBuy.Validate.StatusCode(HttpStatusCode.OK);
                }
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.Close)), Does.Contain(Decimal(newMinSellPrice)), "Close price does not contain new min sell price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingVolume)), Does.Contain(Decimal(tradingVolume) * Decimal(partialCount)), "does not contain trading volume");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingOppositeVolume)), Does.Contain((Decimal(tradingVolume) * Decimal(newMinSellPrice) * partialCount)), "does not contain trading volume * sell price");
                });
            }
        }

        public class LONumerousTradesCandle : CandlesHistoryBaseTest
        {
            int partialCount = 10;
            [Test]
            [Category("CandleHistory")]
            public void LONumerousTradesCandleTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();

                for (var i = 0; i < partialCount; i++)
                {
                    var requestBuy = new PlaceLimitOrderModel() { Price = newMaxBuyPrice + i/Math.Pow(10, 5), AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };
                    var responseBuy = hft.Orders.PostOrdersLimitOrder(requestBuy, ApiKey);
                    responseBuy.Validate.StatusCode(HttpStatusCode.OK);
                }

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                hft.OrderBooks.GetOrderBooks(AssetPairId);

                var requestSell = new PlaceMarketOrderModel() { AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume * partialCount, Asset = FirstAssetId };
                
                var responseSell = hft.Orders.PostOrdersMarket(requestSell, SecondWalletApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);
                
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.High)), Does.Contain(Decimal(newMaxBuyPrice) + Decimal(((partialCount-1) / Math.Pow(10, 5)))), "Unexpected max price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.Low)), Does.Contain(Decimal(newMaxBuyPrice)), "Unexpected low price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingVolume)), Does.Contain(Decimal(tradingVolume) * Decimal(partialCount)), "does not contain trading volume");
                });
            }
        }

        public class MONumerousTradesCandle : CandlesHistoryBaseTest
        {
            int partialCount = 10;
            [Test]
            [Category("CandleHistory")]
            public void MONumerousTradesCandleTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();

                for (var i = 0; i < partialCount; i++)
                {
                    var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice + i / Math.Pow(10, 5), AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };
                    var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                    responseSell.Validate.StatusCode(HttpStatusCode.OK);
                }

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                hft.OrderBooks.GetOrderBooks(AssetPairId);

                var requestBuy = new PlaceMarketOrderModel() { AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume * partialCount, Asset = FirstAssetId };

                var responseBuy = hft.Orders.PostOrdersMarket(requestBuy, SecondWalletApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesTrades = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Trades, CandleTimeInterval.Minute, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.High)), Does.Contain(Decimal(newMinSellPrice) + Decimal((partialCount - 1) / Math.Pow(10, 5))), "Unexpected max price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.Low)), Does.Contain(Decimal(newMinSellPrice)), "Unexpected low price");
                    Assert.That(candlesTrades.History.Select(c => Decimal(c.TradingVolume)), Does.Contain(Decimal(tradingVolume) * Decimal(partialCount)), "does not contain trading volume");
                });
            }
        }

        public class BidBuyLOSpot : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void BidBuyLOSpotTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();

                var requestSell = new PlaceLimitOrderModel() { Price = newMaxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };
                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesBid.History.Select(c => c.Low), Does.Contain(newMaxBuyPrice), "Low is not as in order");
                    Assert.That(candlesBid.History.Select(c => c.High), Does.Contain(newMaxBuyPrice), "High is not as in order");
                    Assert.That(candlesBid.History.Select(c => c.Open), Does.Contain(newMaxBuyPrice), "Open is not as in order");
                });
            }
        }

        public class AskSellLOSpot : CandlesHistoryBaseTest
        {
            [Test]
            [Category("CandleHistory")]
            public void AskSellLOSpotTest()
            {
                (var newMinSellPrice, var newMaxBuyPrice) = newMinMaxPrices();

                var requestSell = new PlaceLimitOrderModel() { Price = newMinSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };
                var responseSell = hft.Orders.PostOrdersLimitOrder(requestSell, ApiKey);
                responseSell.Validate.StatusCode(HttpStatusCode.OK);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));

                var candlesBid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Bid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesAsk = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Ask, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                var candlesMid = lykkeApi.CandleHistory.GetCandleHistory(AssetPairId, CandlePriceType.Mid, CandleTimeInterval.Sec, fromMoment, DateTime.Now.ToUniversalTime()).GetResponseObject();

                Assert.Multiple(() =>
                {
                    Assert.That(candlesAsk.History.Select(c => c.Low), Does.Contain(newMinSellPrice), "Low is not as in order");
                    Assert.That(candlesAsk.History.Select(c => c.High), Does.Contain(newMinSellPrice), "High is not as in order");
                    Assert.That(candlesAsk.History.Select(c => c.Open), Does.Contain(newMinSellPrice), "Open is not as in order");
                });    
            }
        }
    }
}
