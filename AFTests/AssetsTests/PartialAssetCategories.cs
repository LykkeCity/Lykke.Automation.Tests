using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest : AssetsTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesGet")]
        public async void GetAllAssetCategories()
        {
            string url = fixture.ApiEndpointNames["assetCategories"];
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

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesGet")]
        public async void GetSingleAssetCategory()
        {
            string url = fixture.ApiEndpointNames["assetCategories"] + "/" + fixture.TestAssetCategory.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetCategoryDTO parsedResponse = JsonUtils.DeserializeJson<AssetCategoryDTO>(response.ResponseJson);

            AssetCategoryEntity entity = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategory.Id);
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesPost")]
        public async void CreateAssetCategory()
        {
            AssetCategoryDTO createdCategory = await fixture.CreateTestAssetCategory();
            Assert.NotNull(createdCategory);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbCreated = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(createdCategory.Id);
            checkDbCreated.ShouldBeEquivalentTo(createdCategory, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesPost")]
        public async void UpdateAssetCategory()
        {
            string url = fixture.ApiEndpointNames["assetCategories"];

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

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesDelete")]
        public async void DeleteAssetCategory()
        {
            string deleteUrl = fixture.ApiEndpointNames["assetCategories"] + "/" + fixture.TestAssetCategoryDelete.Id;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbDeleted = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategoryDelete.Id);
            Assert.Null(checkDbDeleted);
        }
    }
}
