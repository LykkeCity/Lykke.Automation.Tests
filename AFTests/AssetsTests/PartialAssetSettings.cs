﻿using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Linq;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using XUnitTestData.Repositories.Assets;
using System;
using System.Threading.Tasks;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest : AssetsTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsGet")]
        public async Task GetAllAssetSettings()
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

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsGet")]
        public async Task GetSingleAssetSettings()
        {
            string url = fixture.ApiEndpointNames["assetSettings"] + "/" + fixture.TestAssetSettings.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetSettingsDTO parsedRseponse = JsonUtils.DeserializeJson<AssetSettingsDTO>(response.ResponseJson);
            AssetSettingsDTO parsedEntity = fixture.mapper.Map<AssetSettingsDTO>(fixture.TestAssetSettings);
            parsedRseponse.NormalizeNumberStrings(parsedEntity);

            parsedEntity.ShouldBeEquivalentTo(parsedRseponse);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsGet")]
        public async Task CheckIfAssetSettingsExists()
        {
            string url = fixture.ApiEndpointNames["assetSettings"] + "/" + fixture.TestAssetSettings.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
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
            AssetSettingsDTO createdDTO = await fixture.CreateTestAssetSettings();
            Assert.NotNull(createdDTO);

            await fixture.AssetSettingsManager.UpdateCacheAsync();
            AssetSettingsEntity entity = await fixture.AssetSettingsManager.TryGetAsync(createdDTO.Asset) as AssetSettingsEntity;
            AssetSettingsDTO parsedEntity = fixture.mapper.Map<AssetSettingsDTO>(entity);
            createdDTO.NormalizeNumberStrings(parsedEntity);

            parsedEntity.ShouldBeEquivalentTo(createdDTO);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsPut")]
        public async Task UpdateAssetSettings()
        {
            string url = fixture.ApiEndpointNames["assetSettings"];
            AssetSettingsCreateDTO updateDTO = new AssetSettingsCreateDTO()
            {
                Asset = fixture.TestAssetSettingsUpdate.Asset,
                CashinCoef = Helpers.Random.Next(1,10),
                ChangeWallet = fixture.TestAssetSettingsUpdate.ChangeWallet,
                Dust = Math.Round(Helpers.Random.NextDouble(), 10),
                HotWallet = fixture.TestAssetSettingsUpdate.HotWallet,
                MaxBalance = Helpers.Random.Next(100,1000),
                MaxOutputsCount = Helpers.Random.Next(1, 100),
                MaxOutputsCountInTx = Helpers.Random.Next(1, 100),
                MinBalance = Helpers.Random.Next(1, 100),
                MinOutputsCount = Helpers.Random.Next(1, 100),
                OutputSize = Helpers.Random.Next(100, 10000),
                PrivateIncrement = 0
            };
            string updateParam = JsonUtils.SerializeObject(updateDTO);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetSettingsDTO parsedUpdateDTO = fixture.mapper.Map<AssetSettingsDTO>(updateDTO);

            await fixture.AssetSettingsManager.UpdateCacheAsync();
            AssetSettingsEntity entity = await fixture.AssetSettingsManager.TryGetAsync(fixture.TestAssetSettingsUpdate.Asset) as AssetSettingsEntity;
            AssetSettingsDTO parsedEntity = fixture.mapper.Map<AssetSettingsDTO>(entity);
            parsedUpdateDTO.NormalizeNumberStrings(parsedEntity);

            parsedEntity.ShouldBeEquivalentTo(parsedUpdateDTO);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetSettings")]
        [Category("AssetSettingsDelete")]
        public async Task DeleteAssetSettings()
        {
            string url = fixture.ApiEndpointNames["assetSettings"] + "/" + fixture.TestAssetSettingsDelete.Asset;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.AssetSettingsManager.UpdateCacheAsync();
            AssetSettingsEntity entity = await fixture.AssetSettingsManager.TryGetAsync(fixture.TestAssetSettingsDelete.Asset) as AssetSettingsEntity;
            Assert.Null(entity);
        }
    }
}
