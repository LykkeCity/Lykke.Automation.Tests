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
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetAllAssetAttributes()
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetAttributesReturnDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetAttributesReturnDTO>>(response.ResponseJson);


            for (int i = 0; i < fixture.AllAssetAttributesFromDB.Count; i++)
            {
                fixture.AllAssetAttributesFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers()
                .Excluding(a => a.Attributes));
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttributes()
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + fixture.TestAssetAttribute.AssetId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            fixture.TestAssetAttribute.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(p => p.Attributes));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttribute()
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + fixture.TestAssetAttribute.AssetId + "/" + fixture.TestAssetAttribute.Key;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributeDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributeDTO>(response.ResponseJson);

            Assert.True(fixture.TestAssetAttribute.Value == parsedResponse.Value);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesPost")]
        public async void CreateAssetAttribute()
        {
            AssetAttributeIdentityDTO newAssetAttr = await fixture.CreateTestAssetAttribute();
            Assert.NotNull(newAssetAttr);

            var checkDb = await fixture.AssetAttributesRepository.TryGetAsync(newAssetAttr.AssetId, newAssetAttr.Key);
            Assert.True(checkDb.Value == newAssetAttr.Value);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesPut")]
        public async void UpdateAssetAttribute()
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + fixture.TestAssetAttributeUpdate.AssetId;
            string updateValue = fixture.TestAssetAttributeUpdate.Value + "_AutoTestEdit";
            string updateParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = fixture.TestAssetAttributeUpdate.Key, Value = updateValue });
            var updateResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParameter, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            var checkDbUpdated = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttributeUpdate.AssetId, fixture.TestAssetAttributeUpdate.Key);
            Assert.True(checkDbUpdated.Value == updateValue);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesDelete")]
        public async void DeleteAssetAttribute()
        {
            string deleteUrl = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + fixture.TestAssetAttributeDelete.AssetId + "/" + fixture.TestAssetAttributeDelete.Key;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var checkDbDeleted = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttributeDelete.AssetId, fixture.TestAssetAttributeDelete.Key);
            Assert.Null(checkDbDeleted);
        }
    }
}
