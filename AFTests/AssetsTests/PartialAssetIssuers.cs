using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using XUnitTestCommon.Utils;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersGet")]
        public async void GetAllIssuers()
        {
            string url = fixture.ApiEndpointNames["assetIssuers"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetIssuerDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetIssuerDTO>>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetIssuersFromDB.Count; i++)
            {
                fixture.AllAssetIssuersFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == fixture.AllAssetIssuersFromDB[i].Id),
                o => o.ExcludingMissingMembers());
            }
        }
    }
}
