using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoGet")]
        public async void GetAllAssetExtendedInfos()
        {
            string url = ApiPaths.ASSET_EXTENDED_INFO_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetExtendedInfoDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetExtendedInfoDTO>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            for (int i = 0; i < fixture.AllAssetExtendedInfosFromDB.Count; i++)
            {
                fixture.AllAssetExtendedInfosFromDB[i].ShouldBeEquivalentTo(
                    parsedResponse.Where(a => a.Id == fixture.AllAssetExtendedInfosFromDB[i].Id).FirstOrDefault()
                    , o => o.ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoGet")]
        public async void GetSingleAssetExtendedInfo()
        {
            string url = ApiPaths.ASSET_EXTENDED_INFO_PATH + "/" + fixture.TestAssetExtendedInfo.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetExtendedInfoDTO parsedResponse = JsonUtils.DeserializeJson<AssetExtendedInfoDTO>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            fixture.TestAssetExtendedInfo.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoGet")]
        public async void CheckIfAssetExtendedInfoExists()
        {
            string url = ApiPaths.ASSET_EXTENDED_INFO_PATH + "/" + fixture.TestAssetExtendedInfo.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.NotNull(parsedResponse);
            Assert.True(parsedResponse);

            string badUrl = ApiPaths.ASSET_EXTENDED_INFO_PATH + "/AutoTestAssetThatDoesntExist/exists";
            var badResponse = await fixture.Consumer.ExecuteRequest(badUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(badResponse);
            bool badParsedResponse = JsonUtils.DeserializeJson<bool>(badResponse.ResponseJson);
            Assert.NotNull(badParsedResponse);
            Assert.False(badParsedResponse);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoPost")]
        public async void CreateAssetExtendedInfo()
        {
            AssetExtendedInfoDTO createdInfo = await fixture.CreateTestAssetExtendedInfo();
            Assert.NotNull(createdInfo);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbCreated = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(createdInfo.Id);
            checkDbCreated.ShouldBeEquivalentTo(createdInfo, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoPut")]
        public async void UpdateAssetExtendedInfo()
        {
            string url = ApiPaths.ASSET_EXTENDED_INFO_PATH;

            AssetExtendedInfoDTO updateExtendedInfo = new AssetExtendedInfoDTO()
            {
                Id = fixture.TestAssetExtendedInfoUpdate.Id,
                AssetClass = fixture.TestAssetExtendedInfoUpdate.AssetClass,
                AssetDescriptionUrl = fixture.TestAssetExtendedInfoUpdate.AssetDescriptionUrl,
                Description = fixture.TestAssetExtendedInfoUpdate.Description,
                FullName = fixture.TestAssetExtendedInfoUpdate.FullName + "_autotestt",
                MarketCapitalization = fixture.TestAssetExtendedInfoUpdate.MarketCapitalization,
                NumberOfCoins = fixture.TestAssetExtendedInfoUpdate.NumberOfCoins,
                PopIndex = fixture.TestAssetExtendedInfoUpdate.PopIndex
            };
            string updateParam = JsonUtils.SerializeObject(updateExtendedInfo);

            var updateResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbUpdated = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(fixture.TestAssetExtendedInfoUpdate.Id);
            checkDbUpdated.ShouldBeEquivalentTo(updateParam, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoDelete")]
        public async void DeleteAssetExtendedInfo()
        {
            string deleteUrl = ApiPaths.ASSET_EXTENDED_INFO_PATH + "/" + fixture.TestAssetExtendedInfoDelete.Id;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbDeleted = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(fixture.TestAssetExtendedInfoDelete.Id);
            Assert.Null(checkDbDeleted);
        }

        // /api/v2/asset-extended-infos/__default
    }
}
