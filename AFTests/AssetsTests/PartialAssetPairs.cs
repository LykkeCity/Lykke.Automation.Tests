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
        [Category("AssetPairs")]
        [Category("AsestPairsGet")]
        public async Task GetAllAssetPairs()
        {
            string url = ApiPaths.ASSET_PAIRS_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetPairDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetPairDTO>>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetPairsFromDB.Count; i++)
            {
                fixture.AllAssetPairsFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == fixture.AllAssetPairsFromDB[i].Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetPairs")]
        [Category("AsestPairsGet")]
        public async Task GetSingleAssetPair()
        {
            string url = ApiPaths.ASSET_PAIRS_PATH + "/" + fixture.TestAssetPair.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetPairDTO parsedResponse = JsonUtils.DeserializeJson<AssetPairDTO>(response.ResponseJson);
            fixture.TestAssetPair.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetPairs")]
        [Category("AsestPairsGet")]
        public async Task CheckIfAssetPairExists()
        {
            string url = ApiPaths.ASSET_PAIRS_PATH + "/" + fixture.TestAssetPair.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetPairs")]
        [Category("AsestPairsPost")]
        public async Task CreateAssetPair()
        {
            AssetPairDTO newAssetPair = await fixture.CreateTestAssetPair();
            Assert.NotNull(newAssetPair);

            AssetPairEntity entity = await fixture.AssetPairManager.TryGetAsync(newAssetPair.Id) as AssetPairEntity;
            entity.ShouldBeEquivalentTo(newAssetPair, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetPairs")]
        [Category("AsestPairsPut")]
        public async Task UpdateAssetPair()
        {
            string url = ApiPaths.ASSET_PAIRS_PATH;
            AssetPairDTO updateAssetPair = new AssetPairDTO()
            {
                Accuracy = Helpers.Random.Next(2, 8),
                BaseAssetId = fixture.TestAssetPairUpdate.BaseAssetId,
                Id = fixture.TestAssetPairUpdate.Id,
                InvertedAccuracy = fixture.TestAssetPairUpdate.InvertedAccuracy,
                IsDisabled = fixture.TestAssetPairUpdate.IsDisabled,
                Name = fixture.TestAssetPairUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                QuotingAssetId = fixture.TestAssetPairUpdate.QuotingAssetId,
                Source = fixture.TestAssetPairUpdate.Source,
                Source2 = fixture.TestAssetPairUpdate.Source2
            };
            string updateParam = JsonUtils.SerializeObject(updateAssetPair);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetPairEntity entity = await fixture.AssetPairManager.TryGetAsync(updateAssetPair.Id) as AssetPairEntity;
            entity.ShouldBeEquivalentTo(updateAssetPair, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetPairs")]
        [Category("AsestPairsDelete")]
        public async Task DeleteAssetPair()
        {
            string url = ApiPaths.ASSET_PAIRS_PATH + "/" + fixture.TestAssetPairDelete.Id;
            var result = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.NotNull(result);
            Assert.True(result.Status == HttpStatusCode.NoContent);

            AssetPairEntity entity = await fixture.AssetPairManager.TryGetAsync(fixture.TestAssetPairDelete.Id) as AssetPairEntity;
            Assert.Null(entity);
        }

        //GET /api/v2/asset-pairs/__default
    }
}
