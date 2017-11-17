using ApiV2Data.Fixtures;
using AssetsData.DTOs;
using AssetsData.DTOs.Assets;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using System.Threading.Tasks;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests : ApiV2TestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("Settings")]
        [Category("SettingsGet")]
        public async Task GetBaseAsset()
        {
            string url = fixture.ApiEndpointNames["AssetsBaseAsset"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            var baseAssetId = JsonUtils.DeserializeJson<BaseAssetDTO>(response.ResponseJson).BaseAssetId;
            Assert.NotNull(baseAssetId);
            
        }

        [Test]
        [Category("Smoke")]
        [Category("Settings")]
        [Category("SettingsPost")]
        public async Task SetBaseAsset()
        {
           
            string url = fixture.ApiEndpointNames["AssetsBaseAsset"];
            string testID = "LKK_TEST";
            BaseAssetDTO body = new BaseAssetDTO(testID);
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);

            Assert.True(response.Status == HttpStatusCode.OK);
            string newBaseAssetId = JsonUtils.DeserializeJson<string>(response.ResponseJson);

            //check if the ID has changed
            Assert.Null(newBaseAssetId);
            Assert.True(newBaseAssetId == testID);
        }
    }
}
