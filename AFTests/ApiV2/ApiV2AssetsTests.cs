using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using MoreLinq;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2AssetsTests 
    {
        public class GetAssets : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetsTest()
            {
                Step("Make GET /assets request. Assert that Status code is OK", () => 
                {
                    var assets = apiV2.Assets.GetAssets();
                    Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(assets.GetResponseObject(), Is.Not.Null);
                });   
            }
        }

        public class GetAssetsById : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetsByIdTest()
            {
                var assetId = "";

                Step("Make GET /assets request. Take id of first asset in response", () =>
                {
                    var assets = apiV2.Assets.GetAssets();
                    Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    assetId = assets.GetResponseObject().Assets[0].Id;
                });

                Step($"Make GET /assets/{assetId}. Validate response", () => 
                {
                    var asset = apiV2.Assets.GetAssetsById(assetId);
                    Assert.That(asset.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(asset.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetAssetAttributesById : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetAttributesByIdTest()
            {
                var assetId = "";

                Step("Make GET /assets request. Take id of first asset in response", () =>
                {
                    var assets = apiV2.Assets.GetAssets();
                    Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    assetId = assets.GetResponseObject().Assets[0].Id;
                });

                Step($"Make GET /api/assets/{assetId}/attributes. Validate response", () => 
                {
                    var attr = apiV2.Assets.GetAssetAttributesByid(assetId);
                    Assert.That(attr.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(attr.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetAssetAttributeKey : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetAttributeKeyTest()
            {
                var assetId = "";
                var assetAttributeKey = "";

                Step("Make GET /assets and find asset with non-empty attributes", () => 
                {
                    var assets = apiV2.Assets.GetAssets();
                    Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                    assetId = assets.GetResponseObject().Assets.ToList().Find( a => 
                    {
                        var attr = apiV2.Assets.GetAssetAttributesByid(a.Id).GetResponseObject().Attrbuttes;
                        if (attr.Count > 0)
                            return true;
                        return false;
                    })?.Id;
                });

                if (assetId == "")
                    Assert.Ignore("Current environment does not have asset with attributes");

                Step($"Make GET /api/assets/{assetId}/attributes and take attribute key", () => 
                {
                    var attr = apiV2.Assets.GetAssetAttributesByid(assetId);
                    Assert.That(attr.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    assetAttributeKey = attr.GetResponseObject().Attrbuttes[0].Key;
                });

                Step($"GET /api/assets/{assetId}/attributes/{assetAttributeKey}", () => 
                {
                    var attributeValue = apiV2.Assets.GetAssetAttributeKeyById(assetId, assetAttributeKey);
                    Assert.That(attributeValue.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(attributeValue.GetResponseObject().Key, Is.EqualTo(assetAttributeKey));
                });
            }
        }

        public class GetAssetsDescription : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetsDescriptionTest()
            {
                Step("Make GET /api/assets/description and Validate response", () => 
                {
                    var assetsDescription = apiV2.Assets.GetAssetsDescription();
                    Assert.That(assetsDescription.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(assetsDescription.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetAssetDescriptionById : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetDescriptionByIdTest()
            {
                var assetId = "";

                Step("Make GET /assets request. Take id of first asset in response", () => 
                {
                    var assets = apiV2.Assets.GetAssets();
                    Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    assetId = assets.GetResponseObject().Assets[0].Id;
                });

                Step($"Make GET /api/assets/{assetId}/description and validate response", () => 
                {
                    var assetDescription = apiV2.Assets.GetAssetDescription(assetId);
                    Assert.That(assetDescription.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(assetDescription.GetResponseObject(), Is.Not.Null);
                    Assert.That(assetDescription.GetResponseObject().Id, Is.EqualTo(assetId));
                });
            }
        }

        public class GetAssetsCategories : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetsCategoriesTest()
            {
                Step("Make GET /api/assets/categories and Valiate response", () => 
                {
                    var categories = apiV2.Assets.GetAssetsCategories();
                    Assert.That(categories.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(categories.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetAssetsCategoryById : ApiV2BaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetsCategoryByIdTest()
            {
                var assetId = "";

                Step("Make GET /assets request. Take id of first asset in response", () =>
                {
                    var assets = apiV2.Assets.GetAssets();
                    Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    assetId = assets.GetResponseObject().Assets[0].Id;
                });

                Step($"Make GET /api/assets/categories/{assetId}", () => 
                {
                    var category = apiV2.Assets.GetAssetsCategoriesId(assetId);
                    Assert.That(category.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(category.GetResponseObject(), Is.Not.Null);
                });
            }
        }

        public class GetBaseAsset : ApiV2TokenBaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetBaseAssetTest()
            {
                Step($"Make GET /api/assets/baseAsset and validate response", () => 
                {
                    var response = apiV2.Assets.GetBaseAsset(token);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject(), Is.Not.Null);
                    Assert.That(response.GetResponseObject().BaseAssetId, Is.Not.Null.Or.Empty);
                });
            }
        }

        public class PostBaseAsset : ApiV2TokenBaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void PostBaseAssetTest()
            {
                Step("Make POST /api/assets/baseAsset and validate response", () => 
                {
                    var model = new BaseAssetUpdateModel
                    {
                        BaseAssetId = "BTC",
                        BaseAsssetId = "BTC"
                    };
                    var response = apiV2.Assets.PostAssetBaseAsset(model, token);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                });
            }
        }

        public class GetAssetsAvalable : ApiV2TokenBaseTest
        {
            [Test]
            [Category("ApiV2")]
            public void GetAssetsAvalableTest()
            {
                Step("Make GET /api/assets/available", () => 
                {
                    var response = apiV2.Assets.GetAssetsAvailable(token);
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.GetResponseObject().AssetIds, Is.Not.Null);
                });
            }
        }
    }
}
