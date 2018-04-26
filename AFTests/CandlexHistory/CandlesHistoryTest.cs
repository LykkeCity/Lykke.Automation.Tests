using System;
using System.Collections.Generic;
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
            [Test]
            [Category("CandleHistory")]
            public void CandleHistoryTest()
            {
                var list = lykkeApi.CandleHistory.GetAvailableAssetsPairs().GetResponseObject();


                var request2 = lykkeApi.CandleHistory.PostCandlesHistoryBatch(new GetCandlesHistoryBatchRequest() {
                    AssetPairs = new List<string>() { list[0]},
                    FromMoment = DateTime.Now.AddDays(-5),
                    TimeInterval = CandleTimeInterval.Hour,
                    ToMoment = DateTime.Now,
                    PriceType = CandlePriceType.Mid
                });

                var request3 = lykkeApi.CandleHistory.GetCandleHistory(list[0], CandlePriceType.Trades, CandleTimeInterval.Min30, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-3));
            }
        }
    }
}
