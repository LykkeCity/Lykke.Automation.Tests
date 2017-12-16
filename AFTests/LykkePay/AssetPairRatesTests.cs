using LykkePay.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using TestsCore.AzureUtils;
using TestsCore.TestsData;

namespace LykkePay.Tests
{
    public class AssetPairRatesTests
    {
        public class AssetPairRatesBaseTest : BaseTest
        {
            protected const string testAsset = "BTCTEST";

            [SetUp]
            public void BeforeTest()
            {
                var expectedVersion = Environment.GetEnvironmentVariable("ApiVersion");

                if (expectedVersion != null)
                {
                    var actual = lykkePayApi.assetPairRates.GetIsAlive();
                    if (actual.Version != expectedVersion)
                        Assert.Ignore($"actual service version:{actual.Version}  is not as expected: {expectedVersion}");
                }
            }

            public double ExpectedAsk(double percent, int pips, string assetPair = "BTCUSD")
            {
                var assetPairRates = lykkePayApi.assetPairRates.GetAssetPairRatesModel(assetPair);

                var ask = assetPairRates.ask;
                var deltaSpread = new Decimal(new AzureUtils(Environment.GetEnvironmentVariable("AzureDeltaSpread"))
                    .GetCloudTable("Merchants")
                    .GetSearchResult("ApiKey", "BILETTERTESTKEY")
                    .GetCellByKnowRowKeyAndKnownCellValue("DeltaSpread", "bitteller.test.1").DoubleValue.Value);

                decimal newAsk = ask + (deltaSpread * ask) / 100;
                decimal spread = newAsk * decimal.Parse(percent.ToString(), CultureInfo.InvariantCulture) / 100;
                decimal lpm = newAsk * 0.1m;
                var expectedAsk = newAsk * (1 + decimal.Parse(percent.ToString(), CultureInfo.InvariantCulture) / 100 + 0.1m) + pips / (decimal)Math.Pow(10, assetPairRates.accuracy);
                var a1 = expectedAsk - Math.Floor(expectedAsk); // fraction
                var a2 = a1 * 1000 - Math.Floor(a1 * 1000); // 0.000XXX fraction
                expectedAsk = Math.Floor((Math.Floor(expectedAsk) + a1) * 1000) / 1000;
                if ((a1 - a2 / 1000) > 0)
                    expectedAsk += +0.001m; //move this to 10^curr

                return (double)expectedAsk;
            }

            public double ExpectedBid(double percent, int pips, string assetPair = "BTCUSD")
            {
                var assetPairRates = lykkePayApi.assetPairRates.GetAssetPairRatesModel(assetPair);

                var bid = assetPairRates.bid;
                var deltaSpread = new Decimal(new AzureUtils(Environment.GetEnvironmentVariable("AzureDeltaSpread"))
                    .GetCloudTable("Merchants")
                    .GetSearchResult("ApiKey", "BILETTERTESTKEY")
                    .GetCellByKnowRowKeyAndKnownCellValue("DeltaSpread", "bitteller.test.1").DoubleValue.Value);

                decimal newBid = bid - deltaSpread * bid / 100;
                var tempBid = (newBid * (1 - decimal.Parse(percent.ToString(), CultureInfo.InvariantCulture) / 100 - 0.1m) - pips / (decimal)Math.Pow(10, assetPairRates.accuracy));

                var expectedBid = Math.Floor((tempBid) * (decimal)Math.Pow(10, assetPairRates.accuracy))/ (decimal)Math.Pow(10, assetPairRates.accuracy);
                return expectedBid >= 0 ? (double)expectedBid : 0;
            }
        }

        #region GET Tests
        public class GetAssetPairEmptyPair : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void GetAssetPairEmptyPairTest()
            {
                var response = lykkePayApi.assetPairRates.GetAssetPairRates("");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected status code for empty assetPair");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Unexpected response content");
            }
        }

        public class GetAssetPairUSDBTCPair : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void GetAssetPairUSDBTCPairTest()
            {
                var response = lykkePayApi.assetPairRates.GetAssetPairRates("USDBTC");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected status code for empty assetPair");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Unexpected response content");
            }
        }

        public class GetAssetPairValidIdPair : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void GetAssetPairValidIdPairTest()
            {
                string assetPair = "BTCUSD";
                var response = lykkePayApi.assetPairRates.GetAssetPairRatesModel(assetPair);
                Assert.That(response, Is.Not.Null, "Unexpected response model for valid assetPair");
                Assert.That(assetPair, Is.EqualTo(response.assetPair), "Unexpected asset Pair");
            }
        }

        public class GetAssetPairTextPair : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void GetAssetPairTextPairTest()
            {
                var text = TestData.GenerateLetterString(8);
                
                var response = lykkePayApi.assetPairRates.GetAssetPairRates(text);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected status code for text assetPair");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Unexpected response content");
            }
        }

        public class GetAssetPairNumbersPair : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void GetAssetPairNumbersPairTest()
            {
                var numbers = TestData.GenerateNumbers(6);

                var response = lykkePayApi.assetPairRates.GetAssetPairRates(numbers);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected status code for numbers assetPair");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Unexpected response content");
            }
        }
        #endregion

        #region Post Tests

        public class PostAssetPairEmptyParametersBody : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairEmptyParametersBodyTest()
            {
                var assetPair = "BTCUSD";
                
                string markUp = "{\"markup\": { \"percent\":, \"pips\":}}";
                var merchant = new MerchantModel(markUp);

                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True);
            }
        }

        public class PostAssetPairSpacesBody : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairSpacesBodyTest()
            {
                var assetPair = "BTCUSD";
                
                string markUp = "{\"markup\": { \"percent\": , \"pips\": }}";
                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Unexpected response content");
            }
        }

        public class PostAssetPairEmptyBody : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairSpacesEmptyTest()
            {
                var assetPair = "BTCUSD";
                
                MarkupModel markUp = null;
                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.UnsupportedMediaType), "Unexpected status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True, "Unexpected response content");
            }
        }

        public class PostAssetPairPercentEmpty : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairPercentEmptyTest()
            {
                var assetPair = "BTCUSD";
                int pips = 20;

                string markUp = $"{{\"markup\": {{ \"percent\": , \"pips\": {pips} }}}}";
                var merchant = new MerchantModel(markUp);

                var expectedAsk = ExpectedAsk(0f, pips);
                var expectedBid = ExpectedBid(0f, pips);

                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True);
            }
        }

        public class PostAssetPairWithoutPercent : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairWithoutPercentTest()
            {
                int pips = 20;
                string markUp = $"{{\"markup\": {{\"pips\": {pips} }}}}";

                var expectedAsk = ExpectedAsk(0, pips, testAsset);
                var expectedBid = ExpectedBid(0, pips, testAsset);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(testAsset, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");

                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);

                Assert.Multiple(() =>
                {
                    Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");
                    Assert.That(postModel.ask, Is.EqualTo(expectedAsk), "Actual ask is not equal to expected");
                    Assert.That(postModel.bid, Is.EqualTo(expectedBid), "Actual bid is not equal to expected");
                });
            }
        }

        public class PostAssetPairPipsEmpty : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairPipsEmptyTest()
            {
                var assetPair = "BTCUSD";

                double percent = 25.0;

                var expectedAsk = ExpectedAsk(percent, 0);
                var expectedBid = ExpectedBid(percent, 0);

                var perc = percent.ToString("0.0", CultureInfo.InvariantCulture);

                string markUp = $"{{\"markup\": {{\"percent\":{perc} , \"pips\":}}}}";

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code");
                Assert.That(string.IsNullOrEmpty(response.Content), Is.True);
            }
        }

        public class PostAssetPairWithoutPips : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairWithoutPipsTest()
            {
                double percent = 20.0;

                string markUp = $"{{\"markup\": {{\"percent\": {percent}}}}}";
                var merchant = new MerchantModel(markUp);

                var expectedAsk = ExpectedAsk(percent, 0, testAsset);
                var expectedBid = ExpectedBid(percent, 0, testAsset);

                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(testAsset, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);

                Assert.Multiple(() =>
                {
                    Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");
                    Assert.That(postModel.ask, Is.EqualTo(expectedAsk), "Actual ask is not equal to expected");
                    Assert.That(postModel.bid, Is.EqualTo(expectedBid), "Actual bid is not equal to expected");
                });         
            }
        }

        public class PostAssetPairPercentDiffValuesPositive : AssetPairRatesBaseTest
        {
            [TestCase("0")]
            [TestCase("1")]
            [TestCase("25.5")]
            [TestCase("50.0")]
            [TestCase("100.0")]
            [TestCase("500.0")]            
            [Category("LykkePay")]
            public void PostAssetPairPercentDiffValuesPositiveTest(object percent)
            {
                var p = double.Parse(percent.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

                var doubleToString = p.ToString(); ;
                if (!doubleToString.Contains("."))
                    doubleToString = p.ToString() + ".0";

                string markUp = $"{{\"markup\": {{\"percent\":{doubleToString}, \"pips\": 0}}}}";

                var expectedAsk = ExpectedAsk(p, 0, testAsset);
                var expectedBid = ExpectedBid(p, 0, testAsset);

                var merchant = new MerchantModel(markUp);

                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(testAsset, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);

                Assert.Multiple(() =>
                {
                    Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");
                    Assert.That(postModel.ask, Is.EqualTo(expectedAsk), "Actual ask is not equal to expected");
                    Assert.That(postModel.bid, Is.EqualTo(expectedBid), "Actual bid is not equal to expected");
                });
            }
        }

        public class PostAssetPairPercentDiffValuesNegative : AssetPairRatesBaseTest
        {
            [TestCase("-1.0")]
            [TestCase("testtest")]
            [TestCase("25%")]
            [Category("LykkePay")]
            public void PostAssetPairPercentDiffValuesNegativeTest(object percent)
            {
                object p = percent;
                var assetPair = "BTCUSD";

                string markUp = $"{{\"markup\": {{\"percent\":{p}, \"pips\": 0}}}}";

                var merchant = new MerchantModel(markUp);

                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code");              
            }
        }

        public class PostAssetPairPipsDiffValuesPositive : AssetPairRatesBaseTest
        {
            [TestCase("0")]
            [TestCase("1")]
            [TestCase("50")]
            [TestCase("100")]
            [TestCase("500")]           
            [Category("LykkePay")]
            public void PostAssetPairPipsDiffValuesPositiveTest(object pips)
            {
                int p = int.Parse((string)pips);

                var expectedAsk = ExpectedAsk(0f, p, testAsset);
                var expectedBid = ExpectedBid(0f, p, testAsset);

                string markUp = $"{{\"markup\": {{\"percent\":0.0,\"pips\":{p}}}}}";
                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(testAsset, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.Multiple(() =>
                {
                    Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");
                    Assert.That(postModel.ask, Is.EqualTo(expectedAsk), "Actual ask is not equal to expected");
                    Assert.That(postModel.bid, Is.EqualTo(expectedBid), "Actual bid is not equal to expected");
                });
            }
        }

        public class PostAssetPairPipsDiffValuesNegative : AssetPairRatesBaseTest
        {
            [TestCase("-1")]
            [TestCase("3.5")]
            [TestCase("testtest")]
            [TestCase("3 pip")]
            [Category("LykkePay")]
            public void PostAssetPairPipsDiffValuesNegativeTest(object pips)
            {
                object p = pips;
                var assetPair = "BTCUSD";

                string markUp = $"{{\"markup\": {{\"percent\":0.0,\"pips\":{p}}}}}";
                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code");
            }
        }

        public class PostAssetPairPips300CharsNegative : AssetPairRatesBaseTest
        {
            [TestCase("5.0;300+")]
            [TestCase("300+;3.5")]
            [Category("LykkePay")]
            public void PostAssetPairPips300CharsNegativeTest(object parameters)
            {
                string pc = parameters.ToString().Split(";")[0];
                string pips = parameters.ToString().Split(";")[1];
                if (pc.Contains("300+"))
                    pc = TestData.GenerateNumbers(301);
                else
                    pips = TestData.GenerateNumbers(301);
                var assetPair = "BTCUSD";

                string markUp = $"{{\"markup\": {{\"percent\":{pc}.0,\"pips\":{pips}}}}}";
                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(assetPair, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code");
            }
        }

        public class PostAssetPairValidValues : AssetPairRatesBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostAssetPairValidValuesTest()
            {
                MarkupModel markUp = new MarkupModel(50, 30);

                var expectedAsk = ExpectedAsk(50.0, 30, testAsset);
                var expectedBid = ExpectedBid(50.0, 30, testAsset);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(testAsset, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.Multiple(() =>
                {
                    Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");
                    Assert.That(expectedAsk, Is.EqualTo(postModel.ask), "Actual ask is not equal to expected");
                    Assert.That(expectedBid, Is.EqualTo(postModel.bid), "Actual bid is not equal to expected");
                });
            }
        }
        #endregion

        #region rounding
        public class PostAssetPairPipsDiffValuesAskRounding : AssetPairRatesBaseTest
        {
            [TestCase(arg1: 9999.9992, arg2: 10000.000)]
            [TestCase(arg1: 8888.5648, arg2: 8888.565)]
            [Category("LykkePay")]
            public void PostAssetPairPipsDiffValuesAskRoundingTest(object expectedAsk, object roundedAsk)
            { 
                var assetPairRates = lykkePayApi.assetPairRates.GetAssetPairRatesModel(testAsset);

                var ask = assetPairRates.ask;
                var deltaSpread = new Decimal(new AzureUtils(Environment.GetEnvironmentVariable("AzureDeltaSpread"))
                    .GetCloudTable("Merchants")
                    .GetSearchResult("ApiKey", "BILETTERTESTKEY")
                    .GetCellByKnowRowKeyAndKnownCellValue("DeltaSpread", "bitteller.test.1").DoubleValue.Value);

                var newAsk = assetPairRates.ask + assetPairRates.ask * deltaSpread / 100;
                var percent = (decimal.Parse(expectedAsk.ToString()) - newAsk - newAsk*0.1m/*lykkays percent*/ - newAsk * 0 /*lykkays pips*/)*100/newAsk;

                var perc = percent.ToString(CultureInfo.InvariantCulture);
                string markUp = $"{{\"markup\": {{\"percent\":{perc}, \"pips\": 0}}}}";

                var merchant = new MerchantModel(markUp);

                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(testAsset, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);

                Assert.Multiple(() =>
                {
                    Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");
                    Assert.That(postModel.ask, Is.EqualTo(decimal.Parse(roundedAsk.ToString())), "Actual ask is not equal to expected");
                });
            }
        }

        public class PostAssetPairPipsDiffValuesBidRounding : AssetPairRatesBaseTest
        {
            [TestCase(arg1: 999.9999, arg2: 999.999)]
            [TestCase(arg1: 888.5648, arg2: 888.564)]
            [Category("LykkePay")]
            public void PostAssetPairPipsDiffValuesBidRoundingTest(object expectedBid, object roundedBid)
            {
                var assetPairRates = lykkePayApi.assetPairRates.GetAssetPairRatesModel(testAsset);

                var bid = assetPairRates.bid;
                var deltaSpread = new Decimal(new AzureUtils(Environment.GetEnvironmentVariable("AzureDeltaSpread"))
                    .GetCloudTable("Merchants")
                    .GetSearchResult("ApiKey", "BILETTERTESTKEY")
                    .GetCellByKnowRowKeyAndKnownCellValue("DeltaSpread", "bitteller.test.1").DoubleValue.Value);

                var newBid = assetPairRates.bid - assetPairRates.bid * deltaSpread / 100;
                var percent = (-decimal.Parse(expectedBid.ToString()) + newBid - newBid * 0.1m/*lykkays percent*/ - newBid * 0 /*lykkays pips*/) * 100 / newBid;
                var perc = percent.ToString(CultureInfo.InvariantCulture);

                string markUp = $"{{\"markup\": {{\"percent\":{perc}, \"pips\": 0}}}}";

                var merchant = new MerchantModel(markUp);

                var response = lykkePayApi.assetPairRates.PostAssetPairRatesWithJsonBody(testAsset, merchant, markUp);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);

                Assert.Multiple(() =>
                {
                    Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");
                    Assert.That(postModel.bid, Is.EqualTo(decimal.Parse(roundedBid.ToString())), "Actual bid is not equal to expected");
                });
            }
        }
        #endregion
    }
}
