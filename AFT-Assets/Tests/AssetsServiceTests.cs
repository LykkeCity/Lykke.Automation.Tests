using XUnitTestCommon;
using FirstXUnitTest.DTOs;
using FirstXUnitTest.DTOs.Assets;
using XUnitTestCommon.Utils;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Xunit;
using FirstXUnitTest.Fixtures;
using XUnitTestData.Repositories.Assets;
using XUnitTestData.Domains.Assets;
using System.Threading.Tasks;

namespace FirstXUnitTest
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "Assets")]
    public class AssetsServiceTests :IClassFixture<AssetsTestDataFixture>
    {
        private AssetsTestDataFixture fixture;

        private string urlPreffix = "assets";
        private Dictionary<string, string> emptyDict = new Dictionary<string, string>();

        public AssetsServiceTests(AssetsTestDataFixture fixture)
        {
            this.fixture = fixture;
        }


        #region All/Multiple assets
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MultipleAssets")]
        public async void GetAllAssets()
        {
            // Get all assets
            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets", emptyDict, null, Method.GET, urlPreffix);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetDTO>>(response.ResponseJson);

            fixture.AllAssetsFromDB.Should().HaveSameCount(parsedResponse);

            for (int i = 0; i < fixture.AllAssetsFromDB.Count; i++)
            {
                fixture.AllAssetsFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], options => options
                .ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MultipleAssets")]
        public async void GetAllAssetsExtended()
        {
            // Get all assets extended
            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/extended", emptyDict, null, Method.GET, urlPreffix);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            AssetExtendedReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetExtendedReturnDTO>(response.ResponseJson);

            foreach (AssetExtendedDTO assetExtended in parsedResponse.Assets)
            {
                Assert.True(await TestSingleAssetExtended(assetExtended));
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MultipleAssets")]
        public async void GetAllAssetsCategories()
        {
            // Get all assets categories
            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/categories", emptyDict, null, Method.GET, urlPreffix);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetCategoryDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetCategoryDTO>>(response.ResponseJson);

            var allCategoriesFromDB = await fixture.AssetCategoryManager.GetAllAsync();
            List<AssetCategoryEntity> categoryEntities = allCategoriesFromDB.Cast<AssetCategoryEntity>().ToList();
            Assert.NotNull(categoryEntities);

            for(int i = 0; i < parsedResponse.Count; i++)
            {
                categoryEntities[i].ShouldBeEquivalentTo(parsedResponse[i], options => options
                .ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MultipleAssets")]
        public async void GetAllAssetsDescription()
        {
            // Get all assets description
            AssetDescriptionBodyParamDTO bodyParam = new AssetDescriptionBodyParamDTO();
            foreach (AssetDescriptionEntity entity in fixture.AllAssetDescriptionsFromDB)
            {
                bodyParam.Ids.Add(entity.Id);
            }

            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/description", emptyDict, 
                JsonUtils.SerializeObject(bodyParam), Method.POST, urlPreffix);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetDescriptionDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetDescriptionDTO>>(response.ResponseJson);

            foreach (AssetDescriptionDTO desc in parsedResponse)
            {
                AssetDescriptionEntity dbDesc = fixture.AllAssetDescriptionsFromDB.Where(m => m.Id == desc.Id).FirstOrDefault();
                Assert.NotNull(dbDesc);

                dbDesc.ShouldBeEquivalentTo(desc, options => options
                .ExcludingMissingMembers()
                .Excluding(m => m.IssuerName));
            }

        }
        #endregion

        #region Single asset
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "SingleAsset")]
        public async void GetSingleAsset()
        {
            
            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/" + fixture.TestAsset.Id,
                emptyDict, null, Method.GET, urlPreffix);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            fixture.TestAsset.ShouldBeEquivalentTo(parsedResponse, options => options
                .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "SingleAsset")]
        public async void GetSingleAssetAttributes()
        {
            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/" + fixture.TestAssetAttribute.AssetId + "/attributes", 
                emptyDict, null, Method.GET, urlPreffix);

            // Assert the status code is OK
            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            fixture.TestAssetAttribute.ShouldBeEquivalentTo(parsedResponse, options => options
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "SingleAsset")]
        public async void GetSingleAssetAttributesKey()
        {
            var testDbAttribute = fixture.TestAssetAttribute.Attributes.FirstOrDefault();
            Assert.NotNull(testDbAttribute);

            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/" + fixture.TestAssetAttribute.AssetId + "/attributes/" + testDbAttribute.Key,
                emptyDict, null, Method.GET, urlPreffix);

            // Assert the status code is OK
            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            var testServiceAttribute = parsedResponse.Attributes.Where(attr => attr.Key == testDbAttribute.Key).FirstOrDefault();
            Assert.NotNull(testServiceAttribute);

            testDbAttribute.ShouldBeEquivalentTo(testServiceAttribute);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "SingleAsset")]
        public async void GetSingleAssetCategories()
        {
            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/" + fixture.TestAsset.Id + "/categories", 
                emptyDict, null, Method.GET, urlPreffix);

            // Assert the status code is OK
            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetCategoryDTO parsedResponse = JsonUtils.DeserializeJson<AssetCategoryDTO>(response.ResponseJson);

            var testCategory = await fixture.AssetCategoryManager.TryGetAsync(parsedResponse.Id);
            AssetCategoryEntity categoryEntity = (AssetCategoryEntity)testCategory;

            categoryEntity.ShouldBeEquivalentTo(parsedResponse, options => options
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "SingleAsset")]
        public async void GetSingleAssetExtended()
        {
            var response = await fixture.Consumer.ExecuteRequest(null, "/api/assets/" + fixture.TestAsset.Id + "/extended", 
                emptyDict, null, Method.GET, urlPreffix);

            // Assert the status code is OK
            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetExtendedReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetExtendedReturnDTO>(response.ResponseJson);

            Assert.True(parsedResponse.Assets.Count == 1);
            AssetExtendedDTO assetExtended = parsedResponse.Assets[0];

            Assert.True(await TestSingleAssetExtended(assetExtended));
        }

        private async Task<bool> TestSingleAssetExtended(AssetExtendedDTO assetExtended)
        {
            AssetEntity dbAsset = fixture.AllAssetsFromDB.Where(a => a.Id == assetExtended.Asset.Id).FirstOrDefault();
            Assert.NotNull(dbAsset);

            dbAsset.ShouldBeEquivalentTo(assetExtended.Asset, options => options
            .ExcludingMissingMembers());

            if (assetExtended.Attributes.Attributes.Count > 1)
            {
                AssetAttributesEntity assetAttributeFromDB = fixture.AllAssetAttributesFromDB
                    .Where(a => a.AssetId == assetExtended.Asset.Id).FirstOrDefault();
                
                if (assetAttributeFromDB != null)
                {
                    assetAttributeFromDB.ShouldBeEquivalentTo(assetExtended.Attributes, options => options
                    .ExcludingMissingMembers());
                }
            }

            if (assetExtended.Description.Id != null)
            {
                AssetDescriptionEntity assetDescriptionTest = fixture.AllAssetDescriptionsFromDB.Where(a => a.AssetId == assetExtended.Asset.Id).FirstOrDefault();
                assetDescriptionTest.ShouldBeEquivalentTo(assetExtended.Description, options => options
                .ExcludingMissingMembers()
                .Excluding(m => m.IssuerName));
            }

            if (assetExtended.Category.Id != null)
            {
                AssetCategoryEntity assetCategoryTest = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(assetExtended.Category.Id);
                assetCategoryTest.ShouldBeEquivalentTo(assetExtended.Category, options => options
                .ExcludingMissingMembers());
            }

            return true;
        }

        #endregion
    }
}
