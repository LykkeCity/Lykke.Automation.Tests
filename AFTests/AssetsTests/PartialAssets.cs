using AssetsData.DTOs;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Xunit;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using XUnitTestData.Entitites.ApiV2.Assets;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsGet")]
        public async void GetAllAssets()
        {
            // Get all assets
            string url = ApiPaths.ASSETS_BASE_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetDTO>>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetsFromDB.Count; i++)
            {
                fixture.AllAssetsFromDB[i].ShouldBeEquivalentTo(
                    parsedResponse.Where(a => a.Id == fixture.AllAssetsFromDB[i].Id).FirstOrDefault(),
                    o => o.ExcludingMissingMembers().Excluding(m => m.PartnerIds));
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsGet")]
        public async void GetSingleAsset()
        {
            string url = ApiPaths.ASSETS_BASE_PATH + "/" + fixture.TestAsset.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            fixture.TestAsset.ShouldBeEquivalentTo(parsedResponse, options => options
                .ExcludingMissingMembers()
                .Excluding(a => a.PartnerIds));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsGet")]
        public async void CheckIfAssetExists()
        {
            string url = ApiPaths.ASSETS_V2_BASE_PATH + "/" + fixture.TestAsset.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsGet")]
        public async void GetDefault()
        {
            string url = ApiPaths.ASSETS_DEFAULT_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            foreach (PropertyInfo pi in parsedResponse.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = pi.GetValue(parsedResponse).As<string>();
                    if (pi.Name == "Blockchain")
                    {
                        Assert.True(value == "None");
                    }
                    else
                    {
                        Assert.Null(value);
                    }
                }
                else if (pi.PropertyType == typeof(int))
                    Assert.True(pi.GetValue(parsedResponse).As<int>() == 0);
                else if (pi.PropertyType == typeof(bool))
                    Assert.True(pi.GetValue(parsedResponse).As<bool>() == false);
                else if (pi.PropertyType == typeof(List<string>))
                    Assert.True(pi.GetValue(parsedResponse).As<List<string>>().Count == 0);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsPost")]
        public async void EnableDisableAsset()
        {
            string disableUrl = ApiPaths.ASSETS_V2_BASE_PATH + "/" + fixture.TestAsset.Id + "/disable";
            string enableUrl = ApiPaths.ASSETS_V2_BASE_PATH + "/" + fixture.TestAsset.Id + "/enable";
            string parameter = JsonUtils.SerializeObject(new { id = fixture.TestAsset.Id });
            string url;

            if (fixture.TestAsset.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, parameter, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetEntity entity = await fixture.AssetManager.TryGetAsync(fixture.TestAsset.Id) as AssetEntity;
            Assert.True(entity.IsDisabled != fixture.TestAsset.IsDisabled);

            if (entity.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var responseAfter = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, parameter, Method.POST);
            Assert.True(responseAfter.Status == HttpStatusCode.NoContent);

            AssetEntity entityAfter = await fixture.AssetManager.TryGetAsync(fixture.TestAsset.Id) as AssetEntity;
            Assert.True(entityAfter.IsDisabled == fixture.TestAsset.IsDisabled);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsPost")]
        public async void CreateAsset()
        {
            AssetDTO createdAsset = await fixture.CreateTestAsset();
            Assert.NotNull(createdAsset);

            AssetEntity entity = await fixture.AssetManager.TryGetAsync(createdAsset.Id) as AssetEntity;
            entity.ShouldBeEquivalentTo(createdAsset, o => o
            .ExcludingMissingMembers());

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsPut")]
        public async void UpdateAsset()
        {
            string url = ApiPaths.ASSETS_V2_BASE_PATH;
            AssetDTO updateParamAsset = fixture.TestAssetUpdate;
            updateParamAsset.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTestEdit;
            updateParamAsset.DefinitionUrl += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest ;

            string updateParam = JsonUtils.SerializeObject(updateParamAsset);

            var updateResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            AssetEntity entityUpdateed = await fixture.AssetManager.TryGetAsync(updateParamAsset.Id) as AssetEntity;
            entityUpdateed.ShouldBeEquivalentTo(updateParamAsset, o => o
            .ExcludingMissingMembers());


        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsDelete")]
        public async void DeleteAsset()
        {
            string url = ApiPaths.ASSETS_V2_BASE_PATH + "/" + fixture.TestAssetDelete.Id;
            string deleteParam = JsonUtils.SerializeObject(new { id = fixture.TestAssetDelete.Id });

            var deleteResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, deleteParam, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            AssetEntity entityDeleted = await fixture.AssetManager.TryGetAsync(fixture.TestAssetDelete.Id) as AssetEntity;
            Assert.Null(entityDeleted);
        }
    }
}
