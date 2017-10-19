using AutoMapper;
using FirstXUnitTest.DTOs;
using FirstXUnitTest.Fixtures;
using FluentAssertions;
using FluentAssertions.Equivalency;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using Xunit;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;

namespace AFTests.AssetsTests
{
    public class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        private AssetsTestDataFixture fixture;
        private Dictionary<string, string> emptyDict = new Dictionary<string, string>();

        private string endpointBaseName = "/api/v2/assets";

        public AssetsTest(AssetsTestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsGet")]
        public async void GetAllAssets()
        {
            // Get all assets
            string url = endpointBaseName;
            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetDTO>>(response.ResponseJson);

            fixture.AllAssetsFromDB.Should().HaveSameCount(parsedResponse);

            for (int i = 0; i < fixture.AllAssetsFromDB.Count; i++)
            {
                fixture.AllAssetsFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers()
                .Excluding(m => m.PartnerIds));
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsGet")]
        public async void GetSingleAsset()
        {
            string url = endpointBaseName + "/" + fixture.TestAsset.Id;
            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            fixture.TestAsset.ShouldBeEquivalentTo(parsedResponse, options => options
                .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsGet")]
        public async void CheckIfAssetExists()
        {
            string url = endpointBaseName + "/" + fixture.TestAsset.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsGet")]
        public async void GetDefault()
        {
            string url = endpointBaseName + "/default";
            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, null, Method.GET);

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

        //[Fact]
        //[Trait("Category", "Smoke")]
        //[Trait("Category", "AssetsPut")]
        //public async void GetAssetSpecification()
        //{
        //    throw new NotImplementedException();
        //}

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsPost")]
        public async void EnableDisableAsset()
        {
            string disableUrl = endpointBaseName + "/" + fixture.TestAsset.Id + "/disable";
            string enableUrl = endpointBaseName + "/" + fixture.TestAsset.Id + "/enable";
            string parameter = JsonUtils.SerializeObject(new { id = fixture.TestAsset.Id });
            string url;

            if (fixture.TestAsset.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var response = await fixture.Consumer.ExecuteRequest(null, url, emptyDict, parameter, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entity = await fixture.AssetManager.TryGetAsync(fixture.TestAsset.Id) as AssetEntity;
            Assert.True(entity.IsDisabled != fixture.TestAsset.IsDisabled);

            if (entity.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var responseAfter = await fixture.Consumer.ExecuteRequest(null, enableUrl, emptyDict, parameter, Method.POST);
            Assert.True(responseAfter.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityAfter = await fixture.AssetManager.TryGetAsync(fixture.TestAsset.Id) as AssetEntity;
            Assert.True(entityAfter.IsDisabled == fixture.TestAsset.IsDisabled);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetsPost")]
        [Trait("Category", "AssetsPut")]
        [Trait("Category", "AssetsDelete")]
        public async void CreateUpdateDeleteAsset()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<AssetEntity, AssetDTO>();
            });

            AssetDTO testAsset = Mapper.Map<AssetDTO>(fixture.TestAsset);
            AssetDTO testAssetUpdateed = testAsset;
            testAsset.Id = testAsset.Id + "_AutoTest";
            testAssetUpdateed.Name = testAssetUpdateed.Name + "_AutoTest";

            string createUrl = endpointBaseName;
            string deleteUrl = endpointBaseName + "/" + testAsset.Id;

            string createParam = JsonUtils.SerializeObject(testAsset);
            string updateParam = JsonUtils.SerializeObject(testAssetUpdateed);
            string deleteParam = JsonUtils.SerializeObject(new { id = testAsset.Id });

            //create asset
            var response = await fixture.Consumer.ExecuteRequest(null, createUrl, emptyDict, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.Created);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entity = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
            entity.ShouldBeEquivalentTo(testAsset, o => o
            .ExcludingMissingMembers());

            //Update assset
            var updateResponse = await fixture.Consumer.ExecuteRequest(null, endpointBaseName, emptyDict, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityUpdateed = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
            entityUpdateed.ShouldBeEquivalentTo(testAsset, o => o
            .ExcludingMissingMembers());

            //delete asset
            var deleteResponse = await fixture.Consumer.ExecuteRequest(null, deleteUrl, emptyDict, deleteParam, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityDeleted = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
            Assert.Null(entityDeleted);
        }

        //[Fact]
        //[Trait("Category", "Smoke")]
        //[Trait("Category", "AssetsPut")]
        //public async void UpdateAsset()
        //{
        //    Mapper.Initialize(cfg => {
        //        cfg.CreateMap<AssetEntity, AssetDTO>();
        //    });

        //    AssetDTO testAsset = Mapper.Map<AssetDTO>(fixture.TestAsset);
        //    string originalName = testAsset.Name;
        //    testAsset.Name = testAsset.Name + "_AutoTest";

        //    string UpdateParam = JsonUtils.SerializeObject(testAsset);

        //    //Update asset
        //    var response = await fixture.Consumer.ExecuteRequest(null, endpointBaseName, emptyDict, UpdateParam, Method.PUT);
        //    //Assert.True(response.Status == HttpStatusCode.Created);

        //    await fixture.AssetManager.UpdateCacheAsync();
        //    AssetEntity entity = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
        //    entity.ShouldBeEquivalentTo(testAsset, o => o
        //    .ExcludingMissingMembers());


        //    //Update asset to original state
        //    testAsset.Name = originalName;
        //    UpdateParam = JsonUtils.SerializeObject(testAsset);
        //    var sdcondResponse = await fixture.Consumer.ExecuteRequest(null, endpointBaseName, emptyDict, UpdateParam, Method.PUT);
        //    //Assert.True(response.Status == HttpStatusCode.Created);

        //    await fixture.AssetManager.UpdateCacheAsync();
        //    entity = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
        //    entity.ShouldBeEquivalentTo(fixture.TestAsset, o => o
        //    .ExcludingMissingMembers()
        //    .Excluding(e => e.Timestamp)
        //    .Excluding(e => e.ETag));
        //}
    }
}
