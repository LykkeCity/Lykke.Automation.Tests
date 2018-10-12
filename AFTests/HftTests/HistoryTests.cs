namespace AFTests.HftTests
{
    using Lykke.Client.AutorestClient.Models;
    using NUnit.Framework;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    class HistoryTests
    {
        [NonParallelizable]
        public class GetHistoryTradesForAllAssets : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetHistoryTradesTest()
            {
                var volume = 0.1;
                var skip = 0;
                var take = 5;
                var startHistory = hft.History.GetHistory(FirstAssetId, AssetPair, skip, take, ApiKey)
                    .Validate.StatusCode(HttpStatusCode.OK);

                void ValidateHistoryTrade(HistoryTradeModel historyTrade, double tradePrice, bool isBuy)
                {
                    Assert.That(historyTrade.Timestamp.ToUniversalTime(), Is.EqualTo(DateTime.UtcNow).Within(5).Minutes);
                    Assert.That(historyTrade.BaseVolume, Is.EqualTo(isBuy ? volume : -volume));
                    Assert.That(historyTrade.BaseAssetId, Is.EqualTo(FirstAssetId));
                    Assert.That(historyTrade.QuotingAssetId, Is.EqualTo(SecondAssetId));
                    Assert.That(historyTrade.AssetPairId, Is.EqualTo(AssetPair));
                    Assert.That(historyTrade.Price, Is.EqualTo(tradePrice));
                    Assert.That(historyTrade.Fee.Type, Is.Not.Null);
                }

                // Create Limit Order - Sell (SecondApiKey)
                var responseLOSell = CreateAndValidateLimitOrder(100, AssetPair, OrderAction.Sell, volume, SecondWalletApiKey);
                var sellOrderId = responseLOSell.ResponseObject.Id;

                // Create Market Order - Buy (FirstApiKey)
                var responseMOBuy = CreateAndValidateMarketOrder(FirstAssetId, AssetPair, OrderAction.Buy, volume, ApiKey);
                var price = responseMOBuy.ResponseObject.Price;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (sw.Elapsed < TimeSpan.FromMinutes(5))
                {
                    var historyAfterUpdate = hft.History.GetHistory(FirstAssetId, AssetPair, skip, take, ApiKey);

                    if (historyAfterUpdate.StatusCode == HttpStatusCode.OK)
                    {
                        if (startHistory.ResponseObject.First().Id != historyAfterUpdate.ResponseObject.First().Id)
                            break;
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                }
                sw.Stop();

                var lastHistory = hft.History.GetHistory(FirstAssetId, AssetPair, skip, take, ApiKey)
                    .Validate.StatusCode(HttpStatusCode.OK);
                var lastHistorySecondApiKey = hft.History.GetHistory(FirstAssetId, AssetPair, skip, take, SecondWalletApiKey)
                    .Validate.StatusCode(HttpStatusCode.OK);

                // Trade's history for the first wallet (ApiKey)
                var lastTradeBuy = lastHistory.ResponseObject.First();

                Assert.That(startHistory.ResponseObject.First().Id, Does.Not.EqualTo(lastTradeBuy.Id), "Orders are not present in response (they haven't been finished in 5 minutes?)");

                ValidateHistoryTrade(lastTradeBuy, price, isBuy: true);

                // Trade's history for the second wallet (SecondWalletApiKey)
                var lastTradeSell = lastHistorySecondApiKey.ResponseObject.First();

                Assert.That(lastTradeSell.OrderId, Is.EqualTo(sellOrderId));

                ValidateHistoryTrade(lastTradeSell, price, isBuy: false);
            }
        }

        public class GetHistoryTradesNegative : HftBaseTest
        {
            private const string CorrectApiKey = "ApiKey";

            [Test]
            [Category("HFT")]
            [TestCase("!@^&*(%€§", null, 0, 1, CorrectApiKey, ExpectedResult = HttpStatusCode.NotFound)]
            [TestCase(null, "!@^&*(%€§", 0, 1, CorrectApiKey, ExpectedResult = HttpStatusCode.NotFound)]
            [TestCase("!@^&*(%€§", "!@^&*(%€§", 0, 1, CorrectApiKey, ExpectedResult = HttpStatusCode.NotFound)]
            [TestCase(null, null, -1, 1, CorrectApiKey, ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(null, null, 0, -1, CorrectApiKey, ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(null, null, 0, 1, "!@^&*(%€§", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase(null, null, 0, 1, "invalidApiKey", ExpectedResult = HttpStatusCode.Unauthorized)]
            [TestCase(null, null, 0, 1, "-125.45", ExpectedResult = HttpStatusCode.Unauthorized)]
            [TestCase(null, null, 0, 1, " ", ExpectedResult = HttpStatusCode.Unauthorized)]
            public HttpStatusCode GetHistoryTradesNegativeTest(string assetId, string assetPairId, int skip, int take, string apiKey)
            {
                var keyToUse = apiKey == CorrectApiKey ? ApiKey : apiKey;
                return hft.History.GetHistory(assetId, assetPairId, skip, take, keyToUse).StatusCode;
            }
        }
    }
}
