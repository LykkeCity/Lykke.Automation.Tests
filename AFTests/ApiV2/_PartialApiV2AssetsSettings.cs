using ApiV2Data.Fixtures;
using AssetsData.DTOs.Assets;
using RestSharp;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using System.Threading.Tasks;
using XUnitTestCommon.Reports;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests
    {
        [Test]
        [Category("Smoke")]
        [Category("Settings")]
        [Category("SettingsGet")]
        public async Task GetBaseAsset()
        {
            Logger.WriteLine("This test fails due to a Typo in POST /api/assets/baseAsset BaseAsssetId -> BaseAssetId");

            string url = ApiPaths.ASSETS_BASEASSET_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

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
            Logger.WriteLine("This test fails due to a Typo in POST /api/assets/baseAsset BaseAsssetId -> BaseAssetId");

            string url = ApiPaths.ASSETS_BASEASSET_PATH;
            string testID = "LKK_TEST";
            BaseAssetDTO body = new BaseAssetDTO(testID);
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);

            Assert.True(response.Status == HttpStatusCode.OK);
            string newBaseAssetId = JsonUtils.DeserializeJson<string>(response.ResponseJson);

            //check if the ID has changed
            Assert.Null(newBaseAssetId);
            Assert.True(newBaseAssetId == testID);
        }
    }
}
