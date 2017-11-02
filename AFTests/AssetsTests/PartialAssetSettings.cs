using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Linq;
using System.Net;
using Xunit;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using XUnitTestData.Repositories.Assets;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
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

            foreach (AssetSettingsEntity entity in fixture.AllAssetSettingsFromDB)
            {
                AssetSettingsDTO parsedSettings = fixture.mapper.Map<AssetSettingsDTO>(entity);
                AssetSettingsDTO responseItem = parsedRseponse.Items.Where(s => s.Id == parsedSettings.Id).FirstOrDefault();
                responseItem.NormalizeNumberStrings(parsedSettings);

                parsedSettings.ShouldBeEquivalentTo(responseItem);
            }
        }

        [Fact]
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
            AssetSettingsDTO parsedSettings = fixture.mapper.Map<AssetSettingsDTO>(fixture.TestAssetSettings);
            parsedRseponse.NormalizeNumberStrings(parsedSettings);

            parsedSettings.ShouldBeEquivalentTo(parsedRseponse);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetSettings")]
        [Trait("Category", "AssetSettingsGet")]
        public async void CheckIfAssetSettingsExists()
        {
            string url = fixture.ApiEndpointNames["assetSettings"] + "/" + fixture.TestAssetSettings.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedRseponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedRseponse);

        }
    }
}
