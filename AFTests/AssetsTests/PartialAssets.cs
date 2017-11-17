using AssetsData.DTOs;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;
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
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task GetAllAssets()
        {
            // Get all assets
            string url = fixture.ApiEndpointNames["assets"];
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

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task GetSingleAsset()
        {
            string url = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            fixture.TestAsset.ShouldBeEquivalentTo(parsedResponse, options => options
                .ExcludingMissingMembers()
                .Excluding(a => a.PartnerIds));
        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task CheckIfAssetExists()
        {
            string url = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task GetDefault()
        {
            string url = fixture.ApiEndpointNames["assets"] + "/default";
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

        //[Test]
        //[Category("Smoke")]
        //[Category("AssetsPut")]
        //public async Task GetAssetSpecification()
        //{
        //    throw new NotImplementedException();
        //}

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsPost")]
        public async Task EnableDisableAsset()
        {
            string disableUrl = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id + "/disable";
            string enableUrl = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id + "/enable";
            string parameter = JsonUtils.SerializeObject(new { id = fixture.TestAsset.Id });
            string url;

            if (fixture.TestAsset.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, parameter, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entity = await fixture.AssetManager.TryGetAsync(fixture.TestAsset.Id) as AssetEntity;
            Assert.True(entity.IsDisabled != fixture.TestAsset.IsDisabled);

            if (entity.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var responseAfter = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, parameter, Method.POST);
            Assert.True(responseAfter.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityAfter = await fixture.AssetManager.TryGetAsync(fixture.TestAsset.Id) as AssetEntity;
            Assert.True(entityAfter.IsDisabled == fixture.TestAsset.IsDisabled);

        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsPost")]
        public async Task CreateAsset()
        {
            AssetDTO createdAsset = await fixture.CreateTestAsset();
            Assert.NotNull(createdAsset);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entity = await fixture.AssetManager.TryGetAsync(createdAsset.Id) as AssetEntity;
            entity.ShouldBeEquivalentTo(createdAsset, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsPut")]
        public async Task UpdateAsset()
        {
            string updateUrl = fixture.ApiEndpointNames["assets"];
            AssetDTO updateParamAsset = fixture.TestAssetUpdate;
            updateParamAsset.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTestEdit";
            updateParamAsset.DefinitionUrl += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";

            string updateParam = JsonUtils.SerializeObject(updateParamAsset);

            var updateResponse = await fixture.Consumer.ExecuteRequest(updateUrl, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityUpdateed = await fixture.AssetManager.TryGetAsync(updateParamAsset.Id) as AssetEntity;
            entityUpdateed.ShouldBeEquivalentTo(updateParamAsset, o => o
            .ExcludingMissingMembers());


        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsDelete")]
        public async Task DeleteAsset()
        {
            string deleteUrl = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAssetDelete.Id;
            string deleteParam = JsonUtils.SerializeObject(new { id = fixture.TestAssetDelete.Id });

            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, deleteParam, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityDeleted = await fixture.AssetManager.TryGetAsync(fixture.TestAssetDelete.Id) as AssetEntity;
            Assert.Null(entityDeleted);
        }
    }
}
