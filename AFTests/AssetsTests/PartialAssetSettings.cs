using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
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
        [Fact(Skip = "Test will fail due to mismatch in data types")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetSettings")]
        [Trait("Category", "AssetSettingsGet")]
        public async void GetAllAssetSettings()
        {
            string url = fixture.ApiEndpointNames["assetSettings"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AllAssetSettingsDTO parsedRseponse = JsonUtils.DeserializeJson<AllAssetSettingsDTO>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetSettingsFromDB.Count; i++)
            {
                fixture.AllAssetSettingsFromDB[i].ShouldBeEquivalentTo(
                    parsedRseponse.Items.Where(p => p.Id == fixture.AllAssetSettingsFromDB[i].Id).FirstOrDefault(),
                    o => o.ExcludingMissingMembers());
            }
        }

        [Fact(Skip = "Test will fail due to mismatch in data types")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetSettings")]
        [Trait("Category", "AssetSettingsGet")]
        public async void GetSingleAssetSettings()
        {
            string url = fixture.ApiEndpointNames["assetSettings"] + "/" + fixture.TestAssetSettings.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetSettingsDTO parsedRseponse = JsonUtils.DeserializeJson<AssetSettingsDTO>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetSettingsFromDB.Count; i++)
            {
                fixture.AllAssetSettingsFromDB[i].ShouldBeEquivalentTo(parsedRseponse, o => o
                .ExcludingMissingMembers());
            }
        }
    }
}
