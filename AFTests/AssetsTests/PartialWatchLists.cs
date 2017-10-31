using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        #region predefined
        [Fact(Skip = "test will fail")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListGet")]
        public async void GetAllWatchListsPredefined()
        {
            string url = fixture.ApiEndpointNames["watchList"] + "/predefined";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<WatchListDTO> parsedResponse = JsonUtils.DeserializeJson<List<WatchListDTO>>(response.ResponseJson);

            foreach (WatchListEntity entity in fixture.AllWatchListsFromDBPredefined)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == entity.Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Fact(Skip = "test will fail")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListGet")]
        public async void GetSingleWatchListsPredefined()
        {
            string url = fixture.ApiEndpointNames["watchList"] + "/predefined/" + fixture.TestWatchListPredefined.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            WatchListDTO parsedResponse = JsonUtils.DeserializeJson<WatchListDTO>(response.ResponseJson);
            fixture.TestWatchListPredefined.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds));

            fixture.TestWatchListPredefined.AssetIDsList.ShouldBeEquivalentTo(parsedResponse.AssetIds);
        }

        #endregion
    }
}
