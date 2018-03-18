using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace AFTests.HftTests
{
    class HistoryTests
    {

        public class GetHistoryTradesForAllAssets : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetHistoryTradesForAllAssetsTest()
            {
                var assets = hft.AssetPairs.GetAssetPairs();
                var take = "10";
                var skip = "0";

                List<HistoryTradeModel> notNullHistoryResponse = null;
                assets.GetResponseObject().ForEach(pair =>
                {
                    Assert.That(hft.History.GetHistory(pair.BaseAssetId, skip, take, ApiKey).StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Unexpected Status code for asset {pair.BaseAssetId}");    
                });
            }
        }

        public class GetHistoryTradeById : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetHistoryTradeByIdTest()
            {
                var assetId = "BTC";
                var skip = "0";
                var take = "500";

                var responseBefore = hft.History.GetHistory(assetId, skip, take, ApiKey);
                responseBefore.Validate.StatusCode(HttpStatusCode.OK);

                var request = new MarketOrderRequest()
                { Asset = "CHF", AssetPairId = "BTCCHF", OrderAction = OrderAction.Sell, Volume = 0.2 };

                var response = hft.Orders.PostOrdersMarket(request, ApiKey);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var amount = 0.009;
                //buy
                var requestBuy = new MarketOrderRequest()
                { Asset = "BTC", AssetPairId = "BTCCHF", OrderAction = OrderAction.Buy, Volume = amount };

                var responseBuy = hft.Orders.PostOrdersMarket(requestBuy, ApiKey);
                responseBuy.Validate.StatusCode(HttpStatusCode.OK);
                var summ = responseBuy.GetResponseObject().Result;

                var responseAfter = hft.History.GetHistory(assetId, skip, take, ApiKey);
                responseAfter.Validate.StatusCode(HttpStatusCode.OK);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed < TimeSpan.FromMinutes(5))
                {
                    var currentHistory = hft.History.GetHistory(assetId, skip, take, ApiKey);
                    if(currentHistory.StatusCode == HttpStatusCode.OK)
                    { 
                        if (responseBefore.GetResponseObject().First().Id != hft.History.GetHistory(assetId, skip, take, ApiKey).GetResponseObject().First().Id)
                            break;
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                }
                sw.Stop();

                var last = hft.History.GetHistory(assetId, skip, take, ApiKey);
                Assert.That(responseBefore.GetResponseObject().First().Id, Does.Not.EqualTo(last.GetResponseObject().First().Id), "Orders are not present in response(they havnt been finished in 5 minutes?)");

                Assert.That(() => hft.History.GetHistory(assetId, skip, take, ApiKey).GetResponseObject().Find(t => t.Amount == amount).Price, Is.EqualTo(summ).After(5*60*1000, 2*1000));
            }
        }
    }
}
