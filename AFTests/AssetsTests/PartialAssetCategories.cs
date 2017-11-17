using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesGet")]
        public async void GetAllAssetCategories()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetCategoryDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetCategoryDTO>>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetCategoriesFromDB.Count; i++)
            {
                fixture.AllAssetCategoriesFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == fixture.AllAssetCategoriesFromDB[i].Id).FirstOrDefault()
                , o => o.ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesGet")]
        public async void GetSingleAssetCategory()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH + "/" + fixture.TestAssetCategory.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetCategoryDTO parsedResponse = JsonUtils.DeserializeJson<AssetCategoryDTO>(response.ResponseJson);

            AssetCategoryEntity entity = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategory.Id);
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesPost")]
        public async void CreateAssetCategory()
        {
            AssetCategoryDTO createdCategory = await fixture.CreateTestAssetCategory();
            Assert.NotNull(createdCategory);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbCreated = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(createdCategory.Id);
            checkDbCreated.ShouldBeEquivalentTo(createdCategory, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesPost")]
        public async void UpdateAssetCategory()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH;

            AssetCategoryDTO updateCategory = new AssetCategoryDTO()
            {
                Id = fixture.TestAssetCategoryUpdate.Id,
                Name = fixture.TestAssetCategoryUpdate.Name,
                AndroidIconUrl = fixture.TestAssetCategoryUpdate.AndroidIconUrl + "_autotest",
                IosIconUrl = fixture.TestAssetCategoryUpdate.IosIconUrl,
                SortOrder = fixture.TestAssetCategoryUpdate.SortOrder
            };
            string updateParam = JsonUtils.SerializeObject(updateCategory);

            var updateResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbUpdated = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategoryUpdate.Id);
            checkDbUpdated.ShouldBeEquivalentTo(updateCategory, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesDelete")]
        public async void DeleteAssetCategory()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH + "/" + fixture.TestAssetCategoryDelete.Id;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbDeleted = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategoryDelete.Id);
            Assert.Null(checkDbDeleted);
        }
    }
}
