using FirstXUnitTest.DTOs.Assets;
using FirstXUnitTest.Fixtures;
using FluentAssertions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;
using XUnitTestCommon.Utils;

namespace AFTests.AssetsTests
{
    public class AssetAttributesTest : IClassFixture<AssetAttributesTestDataFixture>
    {
        private AssetAttributesTestDataFixture fixture;
        private Dictionary<string, string> emptyDict = new Dictionary<string, string>();

        private string endpointBaseName = "/api/v2/asset-attributes";

        public AssetAttributesTest(AssetAttributesTestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetAllAssetAttributes()
        {
            string url = endpointBaseName;
            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetAttributesReturnDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetAttributesReturnDTO>>(response.ResponseJson);

            fixture.AllAssetAttributesFromDB.Should().HaveSameCount(parsedResponse);

            for (int i = 0; i < fixture.AllAssetAttributesFromDB.Count; i++)
            {
                fixture.AllAssetAttributesFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttributes()
        {
            string url = endpointBaseName + "/" + fixture.TestAssetAttribute.AssetId;
            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            fixture.TestAssetAttribute.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttribute()
        {
            string url = endpointBaseName + "/" + fixture.TestAssetAttribute.AssetId + "/" + fixture.TestAssetAttribute.Key;
            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributeDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributeDTO>(response.ResponseJson);

            Assert.True(fixture.TestAssetAttribute.Value == parsedResponse.Value);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsAttributesPost")]
        [Trait("Category", "AssetsAttributesPut")]
        [Trait("Category", "AssetsAttributesDelete")]
        public async void CreateUpdateDeleteAssetAttribute()
        {
            string newKey = fixture.TestAssetAttribute.Key + "_AutoTest";
            string newValue = "autotest";
            string updateValue = newValue + "_autotest";
            string createUrl = endpointBaseName + "/" + fixture.TestAssetAttribute.AssetId;
            string deleteUrl = endpointBaseName + "/" + fixture.TestAssetAttribute.AssetId + "/" + newKey;
            string createParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = newKey, Value = newValue });
            string updateParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = newKey, Value = updateValue });

            //create asset attribute
            var response = await fixture.Consumer.ExecuteRequest(null, createUrl, emptyDict, createParameter, Method.POST);
            Assert.True(response.Status == HttpStatusCode.Created);

            var checkDb = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttribute.AssetId, newKey);
            Assert.True(checkDb.Value == newValue);

            //create asset attribute
            var updateResponse = await fixture.Consumer.ExecuteRequest(null, createUrl, emptyDict, updateParameter, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            var checkDbUpdated = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttribute.AssetId, newKey);
            Assert.True(checkDbUpdated.Value == updateValue);

            //delte the new attribute
            var deleteResponse = await fixture.Consumer.ExecuteRequest(null, deleteUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var checkDbDeleted = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttribute.AssetId, newKey);
            Assert.Null(checkDbDeleted);
        }

        //[Fact]
        //[Trait("Category", "Smoke")]
        //[Trait("Category", "AssetsAttributesPut")]
        //public async void EditAssetAttribute()
        //{
        //}
    }
}
