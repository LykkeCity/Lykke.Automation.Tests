using AssetsData.Fixtures;
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using XUnitTestCommon;
using RestSharp;
using XUnitTestData.Repositories.Assets;
using FluentAssertions;
using AssetsData.DTOs.Assets;
using XUnitTestCommon.Utils;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest : AssetsTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsGet")]
        public async void GetAllMarginAssets()
        {
            string url = fixture.ApiEndpointNames["marginAssets"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<MarginAssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<MarginAssetDTO>>(response.ResponseJson);

            foreach (MarginAssetEntity entity in fixture.AllMarginAssetsFromDB)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(a => a.Id == entity.Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsGet")]
        public async void GetSingleMarginAssets()
        {
            string url = fixture.ApiEndpointNames["marginAssets"] + "/" + fixture.TestMarginAsset.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginAssetDTO parsedResponse = JsonUtils.DeserializeJson<MarginAssetDTO>(response.ResponseJson);

            fixture.TestMarginAsset.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsGet")]
        public async void CheckIfMarginAssetsExists()
        {
            string url = fixture.ApiEndpointNames["marginAssets"] + "/" + fixture.TestMarginAsset.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsPost")]
        public async void CreateMarginAsset()
        {
            MarginAssetDTO createdDTO = await fixture.CreateTestMarginAsset();
            Assert.NotNull(createdDTO);

            await fixture.MarginAssetManager.UpdateCacheAsync();
            MarginAssetEntity entity = await fixture.MarginAssetManager.TryGetAsync(createdDTO.Id) as MarginAssetEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(createdDTO, o => o.ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsPut")]
        public async void UpdateMarginAsset()
        {
            string url = fixture.ApiEndpointNames["marginAssets"];
            MarginAssetDTO updateDTO = new MarginAssetDTO()
            {
                Id = fixture.TestMarginAssetUpdate.Id,
                Name = fixture.TestMarginAssetUpdate.Name,
                IdIssuer = fixture.TestMarginAssetUpdate.IdIssuer,
                Symbol = fixture.TestMarginAssetUpdate.Symbol,
                Accuracy = Helpers.Random.Next(2,8),
                DustLimit = Helpers.Random.NextDouble(),
                Multiplier = Helpers.Random.NextDouble()
            };

            string updateParam = JsonUtils.SerializeObject(updateDTO);
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.MarginAssetManager.UpdateCacheAsync();
            MarginAssetEntity entity = await fixture.MarginAssetManager.TryGetAsync(updateDTO.Id) as MarginAssetEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(updateDTO, o => o.ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssets")]
        [Category("MarginAssetsDelete")]
        public async void DeleteMarginAsset()
        {
            string url = fixture.ApiEndpointNames["marginAssets"] + "/" + fixture.TestMarginAssetDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.MarginAssetManager.UpdateCacheAsync();
            MarginAssetEntity entity = await fixture.MarginAssetManager.TryGetAsync(fixture.TestMarginAssetDelete.Id) as MarginAssetEntity;
            Assert.Null(entity);
        }
    }
}
