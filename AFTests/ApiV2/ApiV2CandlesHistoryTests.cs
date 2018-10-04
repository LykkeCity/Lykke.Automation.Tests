using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;
using System.Linq;

namespace AFTests.ApiV2
{
    public class ApiV2CandlesHistoryTests
    {
        public class GetCandlesHistoryPositiveMT : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetCandlesHistoryPositiveMTTest()
            {
                var marketType = MarketType.Mt;
                var assetPairId = "BTCUSD";
                CandlePriceType priceType = CandlePriceType.Ask;
                CandleTimeInterval timeInterval = CandleTimeInterval.Hour;
                DateTime fromMoment = DateTime.Now.AddHours(-12).ToUniversalTime();
                DateTime toMoment = DateTime.Now.ToUniversalTime();

                Step($"Make GET /api/candlesHistory with parameters: marketType: {marketType}, assetPairId: {assetPairId}, priceType: {priceType}, timeInterval: {timeInterval}, fromMoment: {fromMoment}, toMoment: {toMoment}", () => 
                {
                    var response = apiV2.CandlesHistory.GetCandlesHistory(marketType, assetPairId, priceType, timeInterval, fromMoment, toMoment);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetCandlesHistoryPositiveSpot : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetCandlesHistoryPositiveSpotTest()
            {
                var marketType = MarketType.Spot;
                var assetPairId = "BTCUSD";
                CandlePriceType priceType = CandlePriceType.Ask;
                CandleTimeInterval timeInterval = CandleTimeInterval.Hour;
                DateTime fromMoment = DateTime.Now.AddHours(-12).ToUniversalTime();
                DateTime toMoment = DateTime.Now.ToUniversalTime();

                Step($"Make GET /api/candlesHistory with parameters: marketType: {marketType}, assetPairId: {assetPairId}, priceType: {priceType}, timeInterval: {timeInterval}, fromMoment: {fromMoment}, toMoment: {toMoment}", () =>
                {
                    var response = apiV2.CandlesHistory.GetCandlesHistory(marketType, assetPairId, priceType, timeInterval, fromMoment, toMoment);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetCandlesHistoryAllPriceTypes : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetCandlesHistoryAllPriceTypesTest()
            {
                var marketType = MarketType.Spot;
                var assetPairId = "BTCUSD";
                CandleTimeInterval timeInterval = CandleTimeInterval.Hour;
                DateTime fromMoment = DateTime.Now.AddHours(-12).ToUniversalTime();
                DateTime toMoment = DateTime.Now.ToUniversalTime();

                Step("Validate Candles with all PriceTypes", () => 
                {
                    var values = Enum.GetValues(typeof(CandlePriceType)).OfType<CandlePriceType>().ToList();
                    values.Remove(CandlePriceType.Unspecified);
                    foreach (var value in values)
                    {
                        Step($"Make GET /api/candlesHistory with parameters: marketType: {marketType}, assetPairId: {assetPairId}, priceType: {value}, timeInterval: {timeInterval}, fromMoment: {fromMoment}, toMoment: {toMoment}", () => 
                        {
                            var response = apiV2.CandlesHistory.GetCandlesHistory(marketType, assetPairId, value, timeInterval, fromMoment, toMoment);
                            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                        }, false);
                    }
                });
            }
        }

        public class GetCandlesHistoryAllTimeIntervalTypes : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetCandlesHistoryAllTimeIntervalTypesTest()
            {
                var marketType = MarketType.Spot;
                var assetPairId = "BTCUSD";
                CandlePriceType priceType = CandlePriceType.Ask;
                DateTime fromMoment = DateTime.Now.AddHours(-12).ToUniversalTime();
                DateTime toMoment = DateTime.Now.ToUniversalTime();

                Step("Validate Candles with all PriceTypes", () =>
                {
                    var values = Enum.GetValues(typeof(CandleTimeInterval)).OfType<CandleTimeInterval>().ToList();
                    values.Remove(CandleTimeInterval.Unspecified);
                    foreach (var value in values)
                    {
                        Step($"Make GET /api/candlesHistory with parameters: marketType: {marketType}, assetPairId: {assetPairId}, priceType: {value}, timeInterval: {priceType}, fromMoment: {fromMoment}, toMoment: {toMoment}", () =>
                        {
                            var response = apiV2.CandlesHistory.GetCandlesHistory(marketType, assetPairId, priceType, value, fromMoment, toMoment);
                            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                        }, false);
                    }
                });
            }
        }

        public class GetCandlesHistoryValidateParameters : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetCandlesHistoryValidateParametersTest()
            {
                var assetPairId = "BTCUSD";
                CandlePriceType priceType = CandlePriceType.Ask;
                CandleTimeInterval timeInterval = CandleTimeInterval.Hour;
                DateTime fromMoment = DateTime.Now.AddHours(-12).ToUniversalTime();
                DateTime toMoment = DateTime.Now.ToUniversalTime();

                MarketType nullType = MarketType.NULL;

                Step("Make GET /api/candlesHistory without marketType and validate response", () => 
                {
                    var response = apiV2.CandlesHistory.GetCandlesHistory(nullType, assetPairId, priceType, timeInterval, fromMoment, toMoment);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }

        public class GetCandlesHistoryValidateParametersNullAssetPairId : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetCandlesHistoryValidateParametersNullAssetPairIdTest()
            {
                CandlePriceType priceType = CandlePriceType.Ask;
                CandleTimeInterval timeInterval = CandleTimeInterval.Hour;
                DateTime fromMoment = DateTime.Now.AddHours(-12).ToUniversalTime();
                DateTime toMoment = DateTime.Now.ToUniversalTime();

                MarketType marketType = MarketType.Mt;

                Step("Make GET /api/candlesHistory without assetPairId and validate response", () =>
                {
                    var response = apiV2.CandlesHistory.GetCandlesHistory(marketType, null, priceType, timeInterval, fromMoment, toMoment);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }
    }
}
