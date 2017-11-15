using ApiV2Data.Fixtures;
using AssetsData.DTOs;
using AssetsData.DTOs.Assets;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Settings")]
        [Trait("Category", "SettingsGet")]
        public async void GetBaseAsset()
        {
            string url = _fixture.ApiEndpointNames["AssetsBaseAsset"];
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            var baseAssetId = JsonUtils.DeserializeJson<BaseAssetDTO>(response.ResponseJson).BaseAssetId;
            Assert.NotNull(baseAssetId);
            
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Settings")]
        [Trait("Category", "SettingsPost")]
        public async void SetBaseAsset()
        {
           
            string url = _fixture.ApiEndpointNames["AssetsBaseAsset"];
            string testID = "LKK_TEST";
            BaseAssetDTO body = new BaseAssetDTO(testID);
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);

            Assert.True(response.Status == HttpStatusCode.OK);
            string newBaseAssetId = JsonUtils.DeserializeJson<string>(response.ResponseJson);

            //check if the ID has changed
            Assert.Null(newBaseAssetId);
            Assert.True(newBaseAssetId == testID);
        }
    }
}
