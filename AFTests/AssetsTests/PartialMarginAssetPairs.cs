using AssetsData.Fixtures;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Xunit;
using XUnitTestCommon;
using AssetsData.DTOs.Assets;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;
using FluentAssertions;
using System.Linq;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MarginAssetPairs")]
        [Trait("Category", "MarginAssetPairsGet")]
        public async void GetAllMarginAssetPairs()
        {
            string url = fixture.ApiEndpointNames["marginAssetPairs"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<MarginAssetPairDTO> parsedResponse = JsonUtils.DeserializeJson<List<MarginAssetPairDTO>>(response.ResponseJson);

            foreach (MarginAssetPairsEntity entity in fixture.AllMarginAssetPairsFromDB)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == entity.Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MarginAssetPairs")]
        [Trait("Category", "MarginAssetPairsGet")]
        public async void GetSingleMarginAssetPairs()
        {
            string url = fixture.ApiEndpointNames["marginAssetPairs"] + "/" + fixture.TestMarginAssetPair.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginAssetPairDTO parsedResponse = JsonUtils.DeserializeJson<MarginAssetPairDTO>(response.ResponseJson);

            fixture.TestMarginAssetPair.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }
    }
}
