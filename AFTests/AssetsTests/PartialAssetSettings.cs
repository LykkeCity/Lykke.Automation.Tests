using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Linq;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using System;
using System.Threading.Tasks;
using XUnitTestData.Entities.Assets;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsGet")]
        public async Task GetAllAssetSettings()
        {
            string url = ApiPaths.ASSET_SETTINGS_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AllAssetSettingsDTO parsedRseponse = JsonUtils.DeserializeJson<AllAssetSettingsDTO>(response.ResponseJson);

            foreach (AssetSettingsEntity entity in this.AllAssetSettingsFromDB)
            {
                AssetSettingsDTO parsedSettings = this.mapper.Map<AssetSettingsDTO>(entity);
                AssetSettingsDTO responseItem = parsedRseponse.Items.Where(s => s.Id == parsedSettings.Id).FirstOrDefault();
                responseItem.NormalizeNumberStrings(parsedSettings);

                parsedSettings.ShouldBeEquivalentTo(responseItem);
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsGet")]
        public async Task GetSingleAssetSettings()
        {
            string url = ApiPaths.ASSET_SETTINGS_PATH + "/" + this.TestAssetSettings.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetSettingsDTO parsedRseponse = JsonUtils.DeserializeJson<AssetSettingsDTO>(response.ResponseJson);
            AssetSettingsDTO parsedEntity = this.mapper.Map<AssetSettingsDTO>(this.TestAssetSettings);
            parsedRseponse.NormalizeNumberStrings(parsedEntity);

            parsedEntity.ShouldBeEquivalentTo(parsedRseponse);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsGet")]
        public async Task CheckIfAssetSettingsExists()
        {
            string url = ApiPaths.ASSET_SETTINGS_PATH + "/" + this.TestAssetSettings.Id + "/exists";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedRseponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedRseponse);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsPost")]
        public async Task CreateAssetSettings()
        {
            AssetSettingsDTO createdDTO = await this.CreateTestAssetSettings();
            Assert.NotNull(createdDTO);

            AssetSettingsEntity entity = await this.AssetSettingsManager.TryGetAsync(createdDTO.Asset) as AssetSettingsEntity;
            AssetSettingsDTO parsedEntity = this.mapper.Map<AssetSettingsDTO>(entity);
            createdDTO.NormalizeNumberStrings(parsedEntity);

            parsedEntity.ShouldBeEquivalentTo(createdDTO);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsPut")]
        public async Task UpdateAssetSettings()
        {
            string url = ApiPaths.ASSET_SETTINGS_PATH;

            AssetSettingsDTO TestAssetSettingsUpdate = await CreateTestAssetSettings();

            AssetSettingsCreateDTO updateDTO = new AssetSettingsCreateDTO()
            {
                Asset = TestAssetSettingsUpdate.Asset,
                CashinCoef = Helpers.Random.Next(1,10),
                ChangeWallet = TestAssetSettingsUpdate.ChangeWallet,
                Dust = Math.Round(Helpers.Random.NextDouble(), 10),
                HotWallet = TestAssetSettingsUpdate.HotWallet,
                MaxBalance = Helpers.Random.Next(100,1000),
                MaxOutputsCount = Helpers.Random.Next(1, 100),
                MaxOutputsCountInTx = Helpers.Random.Next(1, 100),
                MinBalance = Helpers.Random.Next(1, 100),
                MinOutputsCount = Helpers.Random.Next(1, 100),
                OutputSize = Helpers.Random.Next(100, 10000),
                PrivateIncrement = 0
            };
            string updateParam = JsonUtils.SerializeObject(updateDTO);

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetSettingsDTO parsedUpdateDTO = this.mapper.Map<AssetSettingsDTO>(updateDTO);

            AssetSettingsEntity entity = await this.AssetSettingsManager.TryGetAsync(TestAssetSettingsUpdate.Asset) as AssetSettingsEntity;
            AssetSettingsDTO parsedEntity = this.mapper.Map<AssetSettingsDTO>(entity);
            parsedUpdateDTO.NormalizeNumberStrings(parsedEntity);

            parsedEntity.ShouldBeEquivalentTo(parsedUpdateDTO);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsDelete")]
        public async Task DeleteAssetSettings()
        {
            AssetSettingsDTO TestAssetSettingsDelete = await CreateTestAssetSettings();

            string url = ApiPaths.ASSET_SETTINGS_PATH + "/" + TestAssetSettingsDelete.Asset;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetSettingsEntity entity = await this.AssetSettingsManager.TryGetAsync(TestAssetSettingsDelete.Asset) as AssetSettingsEntity;
            Assert.Null(entity);
        }
    }
}
