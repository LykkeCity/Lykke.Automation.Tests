using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2MarketTests
    {
        public class PostMarketConverter : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void PostMarketConverterTest()
            {
                Step("Make POST /api/market/converter with valid parameters and validate response", () => 
                {
                    var assetsFrom = new List<AssetWithAmount>
                        {
                            new AssetWithAmount {Amount = 10, AssetId = "BTC" },
                            new AssetWithAmount { Amount = 10, AssetId = "LKK" }
                        };

                    ConvertionRequest model = new ConvertionRequest
                    {
                        AssetsFrom = assetsFrom,
                        BaseAssetId = "USD",
                        OrderAction = "Sell"
                    };

                    var response = apiV2.Market.PostMarketConvertor(model);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject().Converted.Count, Is.EqualTo(assetsFrom.Count));
                });
            }
        }

        public class PostMarketConverterNoAmount : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void PostMarketConverterNoAmountTest()
            {
                Step("Make POST /api/market/converter without Amount and validate response", () => 
                {
                    var assetsFrom = new List<AssetWithAmount>
                        {  
                        };

                    ConvertionRequest model = new ConvertionRequest
                    {
                        AssetsFrom = assetsFrom,
                        BaseAssetId = "USD",
                        OrderAction = "Sell"
                    };

                    var response = apiV2.Market.PostMarketConvertor(model);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject().Converted.Count, Is.EqualTo(assetsFrom.Count));
                });
            }
        }

        public class PostMarketConverterNoBaseAsset : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void PostMarketConverterNoBaseAssetTest()
            {
                Step("Make POST /api/market/converter without BaseAssetId and validate response", () =>
                {
                    var assetsFrom = new List<AssetWithAmount>
                        {
                            new AssetWithAmount {Amount = 10, AssetId = "BTC" },
                            new AssetWithAmount { Amount = 10, AssetId = "LKK" }
                        };

                    ConvertionRequest model = new ConvertionRequest
                    {
                        AssetsFrom = assetsFrom,
                        OrderAction = "Sell"
                    };

                    var response = apiV2.Market.PostMarketConvertor(model);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }

        public class PostMarketConverterNoOrderAction : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void PostMarketConverterNoOrderActionTest()
            {
                Step("Make POST /api/market/converter without OrderAction and validate response", () =>
                {
                    var assetsFrom = new List<AssetWithAmount>
                        {
                            new AssetWithAmount {Amount = 10, AssetId = "BTC" },
                            new AssetWithAmount { Amount = 10, AssetId = "LKK" }
                        };

                    ConvertionRequest model = new ConvertionRequest
                    {
                        AssetsFrom = assetsFrom,
                        BaseAssetId = "USD"
                    };

                    var response = apiV2.Market.PostMarketConvertor(model);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }

        public class PostMarketConverterEmptyConverterRequest : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void PostMarketConverterEmptyConverterRequestTest()
            {
                Step("Make POST /api/market/converter with empty request body and validate response", () => 
                {
                    ConvertionRequest model = new ConvertionRequest();

                    var response = apiV2.Market.PostMarketConvertor(model);

                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                });
            }
        }
    }
}
