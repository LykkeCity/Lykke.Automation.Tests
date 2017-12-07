using AssetsData.Fixtures;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using XUnitTestCommon;
using RestSharp;
using FluentAssertions;
using AssetsData.DTOs.Assets;
using XUnitTestCommon.Utils;
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
        [Category("MarginAssets")]
        [Category("MarginAssetsGet")]
        public async Task GetAllMarginAssets()
        {
            string url = ApiPaths.MARGIN_ASSET_BASE_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<MarginAssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<MarginAssetDTO>>(response.ResponseJson);

            foreach (MarginAssetEntity entity in this.AllMarginAssetsFromDB)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(a => a.Id == entity.Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsGet")]
        public async Task GetSingleMarginAssets()
        {
            string url = ApiPaths.MARGIN_ASSET_BASE_PATH + "/" + this.TestMarginAsset.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginAssetDTO parsedResponse = JsonUtils.DeserializeJson<MarginAssetDTO>(response.ResponseJson);

            this.TestMarginAsset.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsGet")]
        public async Task CheckIfMarginAssetsExists()
        {
            string url = ApiPaths.MARGIN_ASSET_BASE_PATH + "/" + this.TestMarginAsset.Id + "/exists";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsPost")]
        public async Task CreateMarginAsset()
        {
            MarginAssetDTO createdDTO = await this.CreateTestMarginAsset();
            Assert.NotNull(createdDTO);

            MarginAssetEntity entity = await this.MarginAssetManager.TryGetAsync(createdDTO.Id) as MarginAssetEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(createdDTO, o => o.ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsPut")]
        public async Task UpdateMarginAsset()
        {
            string url = ApiPaths.MARGIN_ASSET_BASE_PATH;

            MarginAssetDTO TestMarginAssetUpdate = await CreateTestMarginAsset();

            MarginAssetDTO updateDTO = new MarginAssetDTO()
            {
                Id = TestMarginAssetUpdate.Id,
                Name = TestMarginAssetUpdate.Name,
                IdIssuer = TestMarginAssetUpdate.IdIssuer,
                Symbol = TestMarginAssetUpdate.Symbol,
                Accuracy = Helpers.Random.Next(2,8),
                DustLimit = Helpers.Random.NextDouble(),
                Multiplier = Helpers.Random.NextDouble()
            };

            string updateParam = JsonUtils.SerializeObject(updateDTO);
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            MarginAssetEntity entity = await this.MarginAssetManager.TryGetAsync(updateDTO.Id) as MarginAssetEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(updateDTO, o => o.ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsDelete")]
        public async Task DeleteMarginAsset()
        {
            MarginAssetDTO TestMarginAssetDelete = await CreateTestMarginAsset();

            string url = ApiPaths.MARGIN_ASSET_BASE_PATH + "/" + TestMarginAssetDelete.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            MarginAssetEntity entity = await this.MarginAssetManager.TryGetAsync(TestMarginAssetDelete.Id) as MarginAssetEntity;
            Assert.Null(entity);
        }
    }
}
