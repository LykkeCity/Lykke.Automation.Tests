using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsGet")]
        public async void GetAllAssetGroups()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH;

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetGroupDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetGroupDTO>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            for (int i = 0; i < fixture.AllAssetGroupsFromDB.Count; i++)
            {
                fixture.AllAssetGroupsFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(g => g.Name == fixture.AllAssetGroupsFromDB[i].Name).FirstOrDefault(),
                    o => o.ExcludingMissingMembers());
            }

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsGet")]
        public async void GetSingleAssetGroups()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestAssetGroup.Id;

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetGroupDTO parsedResponse = JsonUtils.DeserializeJson<AssetGroupDTO>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            fixture.TestAssetGroup.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsGet")]
        public async void GetAssetGroupAssetIDs()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestAssetGroup.Id + "/asset-ids";

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            var entities = await fixture.AssetGroupsRepository.GetAllAsync($"AssetLink_{fixture.TestAssetGroup.Id}");
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            for (int i = 0; i < assetIds.Count; i++)
            {
                Assert.True(assetIds[i] == parsedResponse[i]);
            }


        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsGet")]
        public async void GetAssetGroupClientIDs()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestAssetGroup.Id + "/client-ids";

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            var entities = await fixture.AssetGroupsRepository.GetAllAsync($"ClientGroupLink_{fixture.TestAssetGroup.Id}");
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            for (int i = 0; i < assetIds.Count; i++)
            {
                Assert.True(assetIds[i] == parsedResponse[i]);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPost")]
        public async void CreateAssetGroup()
        {
            AssetGroupDTO createdGroup = await fixture.CreateTestAssetGroup();
            Assert.NotNull(createdGroup);

            AssetGroupEntity entity = await fixture.AssetGroupsManager.TryGetAsync(createdGroup.Name) as AssetGroupEntity;
            entity.ShouldBeEquivalentTo(createdGroup, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPut")]
        public async void UpdateAssetGroup()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH;

            AssetGroupDTO editGroup = new AssetGroupDTO()
            {
                Name = fixture.TestAssetGroupUpdate.Name,
                IsIosDevice = !fixture.TestAssetGroupUpdate.IsIosDevice,
                ClientsCanCashInViaBankCards = fixture.TestAssetGroupUpdate.ClientsCanCashInViaBankCards,
                SwiftDepositEnabled = fixture.TestAssetGroupUpdate.SwiftDepositEnabled
            };
            string editParam = JsonUtils.SerializeObject(editGroup);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetGroupEntity entity = await fixture.AssetGroupsManager.TryGetAsync(editGroup.Name) as AssetGroupEntity;
            entity.ShouldBeEquivalentTo(editGroup, o => o
            .ExcludingMissingMembers());

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsDelete")]
        public async void DeleteAssetGroup()
        {
            string deleteUrl = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestAssetGroupDelete.Name;
            var response = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetGroupEntity entity = await fixture.AssetGroupsManager.TryGetAsync(fixture.TestAssetGroupDelete.Name) as AssetGroupEntity;
            Assert.Null(entity);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPost")]
        public async void AddAssetToAssetGroup()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestGroupForGroupRelationAdd.Name + "/assets/" + fixture.TestAssetForGroupRelationAdd.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetAllAsync($"AssetLink_{fixture.TestGroupForGroupRelationAdd.Name}");
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            Assert.True(assetIds.Count == 1);
            Assert.True(assetIds[0] == fixture.TestAssetForGroupRelationAdd.Id);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPost")]
        public async void RemoveAssetFromAssetGroup()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestGroupForGroupRelationDelete.Name + "/assets/" + fixture.TestAssetForGroupRelationDelete.Id;
            var createResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(createResponse.Status == HttpStatusCode.NoContent);

            var deleteResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetAllAsync($"AssetLink_{fixture.TestGroupForGroupRelationDelete.Name}");
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            Assert.True(assetIds.Count == 0);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPost")]
        public async void AddClientToAssetGroup()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestGroupForClientRelationAdd.Name + "/clients/" + fixture.TestAccountId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetAllAsync($"ClientGroupLink_{fixture.TestGroupForClientRelationAdd.Name}");
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            Assert.True(assetIds.Count == 1);
            Assert.True(assetIds[0] == fixture.TestAccountId);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPost")]
        public async void RemoveClientFromAssetGroup()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + fixture.TestGroupForClientRelationDelete.Name + "/clients/" + fixture.TestAccountId;
            var createResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(createResponse.Status == HttpStatusCode.NoContent);

            var deleteResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetAllAsync($"ClientGroupLink_{fixture.TestGroupForClientRelationDelete.Name}");
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            Assert.True(assetIds.Count == 0);
        }
    }
}
