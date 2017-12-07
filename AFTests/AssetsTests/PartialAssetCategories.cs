using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using System.Threading.Tasks;
using XUnitTestData.Entities.Assets;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesGet")]
        public async Task GetAllAssetCategories()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetCategoryDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetCategoryDTO>>(response.ResponseJson);

            for (int i = 0; i < this.AllAssetCategoriesFromDB.Count; i++)
            {
                this.AllAssetCategoriesFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == this.AllAssetCategoriesFromDB[i].Id).FirstOrDefault()
                , o => o.ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesGet")]
        public async Task GetSingleAssetCategory()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH + "/" + this.TestAssetCategory.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetCategoryDTO parsedResponse = JsonUtils.DeserializeJson<AssetCategoryDTO>(response.ResponseJson);

            AssetCategoryEntity entity = (AssetCategoryEntity)await this.AssetCategoryManager.TryGetAsync(this.TestAssetCategory.Id);
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesPost")]
        public async Task CreateAssetCategory()
        {
            AssetCategoryDTO createdCategory = await this.CreateTestAssetCategory();
            Assert.NotNull(createdCategory);

            AssetCategoryEntity checkDbCreated = (AssetCategoryEntity)await this.AssetCategoryManager.TryGetAsync(createdCategory.Id);
            checkDbCreated.ShouldBeEquivalentTo(createdCategory, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesPost")]
        public async Task UpdateAssetCategory()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH;

            AssetCategoryDTO TestAssetCategoryUpdate = await CreateTestAssetCategory();

            AssetCategoryDTO updateCategory = new AssetCategoryDTO()
            {
                Id = TestAssetCategoryUpdate.Id,
                Name = TestAssetCategoryUpdate.Name,
                AndroidIconUrl = TestAssetCategoryUpdate.AndroidIconUrl + "_autotest",
                IosIconUrl = TestAssetCategoryUpdate.IosIconUrl,
                SortOrder = TestAssetCategoryUpdate.SortOrder
            };
            string updateParam = JsonUtils.SerializeObject(updateCategory);

            var updateResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            AssetCategoryEntity checkDbUpdated = (AssetCategoryEntity)await this.AssetCategoryManager.TryGetAsync(TestAssetCategoryUpdate.Id);
            checkDbUpdated.ShouldBeEquivalentTo(updateCategory, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetCategories")]
        [Category("AssetCategoriesDelete")]
        public async Task DeleteAssetCategory()
        {
            AssetCategoryDTO TestAssetCategoryDelete = await CreateTestAssetCategory();

            string url = ApiPaths.ASSET_CATEGORIES_PATH + "/" + TestAssetCategoryDelete.Id;
            var deleteResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            AssetCategoryEntity checkDbDeleted = (AssetCategoryEntity)await this.AssetCategoryManager.TryGetAsync(TestAssetCategoryDelete.Id);
            Assert.Null(checkDbDeleted);
        }
    }
}
