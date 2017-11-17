﻿using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using System.Threading.Tasks;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest : AssetsTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesGet")]
        public async Task GetAllAssetAttributes()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"];
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

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesGet")]
        public async Task GetSingleAssetAttributes()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            fixture.TestAssetAttribute.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(p => p.Attributes));
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesGet")]
        public async Task GetSingleAssetAttribute()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId + "/" + fixture.TestAssetAttribute.Key;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributeDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributeDTO>(response.ResponseJson);

            Assert.True(fixture.TestAssetAttribute.Value == parsedResponse.Value);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesPost")]
        public async Task CreateAssetAttribute()
        {
            AssetAttributeIdentityDTO newAssetAttr = await fixture.CreateTestAssetAttribute();
            Assert.NotNull(newAssetAttr);

            var checkDb = await fixture.AssetAttributesRepository.TryGetAsync(newAssetAttr.AssetId, newAssetAttr.Key);
            Assert.True(checkDb.Value == newAssetAttr.Value);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesPut")]
        public async Task UpdateAssetAttribute()
        {
            string updateUrl = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttributeUpdate.AssetId;
            string updateValue = fixture.TestAssetAttributeUpdate.Value + "_AutoTestEdit";
            string updateParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = fixture.TestAssetAttributeUpdate.Key, Value = updateValue });
            var updateResponse = await fixture.Consumer.ExecuteRequest(updateUrl, Helpers.EmptyDictionary, updateParameter, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            var checkDbUpdated = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttributeUpdate.AssetId, fixture.TestAssetAttributeUpdate.Key);
            Assert.True(checkDbUpdated.Value == updateValue);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesDelete")]
        public async Task DeleteAssetAttribute()
        {
            string deleteUrl = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttributeDelete.AssetId + "/" + fixture.TestAssetAttributeDelete.Key;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var checkDbDeleted = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttributeDelete.AssetId, fixture.TestAssetAttributeDelete.Key);
            Assert.Null(checkDbDeleted);
        }
    }
}
