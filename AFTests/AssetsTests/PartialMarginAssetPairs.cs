using AssetsData.Fixtures;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon;
using AssetsData.DTOs.Assets;
using XUnitTestCommon.Utils;
using FluentAssertions;
using System.Linq;
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
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsGet")]
        public async Task GetAllMarginAssetPairs()
        {
            string url = ApiPaths.MARGIN_ASSET_PAIRS_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<MarginAssetPairDTO> parsedResponse = JsonUtils.DeserializeJson<List<MarginAssetPairDTO>>(response.ResponseJson);

            foreach (MarginAssetPairsEntity entity in this.AllMarginAssetPairsFromDB)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == entity.Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsGet")]
        public async Task GetSingleMarginAssetPairs()
        {
            string url = ApiPaths.MARGIN_ASSET_PAIRS_PATH + "/" + this.TestMarginAssetPair.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginAssetPairDTO parsedResponse = JsonUtils.DeserializeJson<MarginAssetPairDTO>(response.ResponseJson);

            this.TestMarginAssetPair.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsGet")]
        public async Task CheckIfMarginAssetPairExists()
        {
            string url = ApiPaths.MARGIN_ASSET_PAIRS_PATH + "/" + this.TestMarginAssetPair.Id + "/exists";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsPost")]
        public async Task CreateMarginAssetPair()
        {
            MarginAssetPairDTO createdDTO = await this.CreateTestMarginAssetPair();
            Assert.NotNull(createdDTO);

            MarginAssetPairsEntity entity = await this.MarginAssetPairManager.TryGetAsync(createdDTO.Id) as MarginAssetPairsEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(createdDTO, o => o.ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsDelete")]
        public async Task UpdateMarginAssetPair()
        {
            string url = ApiPaths.MARGIN_ASSET_PAIRS_PATH;
            MarginAssetPairDTO updateDTO = new MarginAssetPairDTO()
            {
                Id = this.TestMarginAssetPairUpdate.Id,
                Accuracy = this.TestMarginAssetPairUpdate.Accuracy + Helpers.Random.Next(1,4),
                InvertedAccuracy = this.TestMarginAssetPairUpdate.InvertedAccuracy,
                Name = this.TestMarginAssetPairUpdate.Name + "_AutoTest",
                BaseAssetId = this.TestMarginAssetPairUpdate.BaseAssetId,
                QuotingAssetId = this.TestMarginAssetPairUpdate.QuotingAssetId
            };

            string editParam = JsonUtils.SerializeObject(updateDTO);
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            MarginAssetPairsEntity entity = await this.MarginAssetPairManager.TryGetAsync(updateDTO.Id) as MarginAssetPairsEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(updateDTO, o => o.ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsDelete")]
        public async Task DeleteMarginAssetPair()
        {
            string url = ApiPaths.MARGIN_ASSET_PAIRS_PATH + "/" + this.TestMarginAssetPairDelete.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            MarginAssetPairsEntity entity = await this.MarginAssetPairManager.TryGetAsync(this.TestMarginAssetPairDelete.Id) as MarginAssetPairsEntity;
            Assert.Null(entity);

        }
    }
}
