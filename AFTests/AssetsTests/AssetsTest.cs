using AssetsData.DTOs.Assets;
using AutoMapper;
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
using XUnitTestData.Repositories.Assets;

namespace AFTests.AssetsTests
{

    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        private AssetsTestDataFixture fixture;
        private Dictionary<string, string> emptyDict = new Dictionary<string, string>();

        public AssetsTest(AssetsTestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        #region assets

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsGet")]
        public async void GetAllAssets()
        {
            // Get all assets
            string url = fixture.ApiEndpointNames["assets"];
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetDTO>>(response.ResponseJson);

            fixture.AllAssetsFromDB.Should().HaveSameCount(parsedResponse);

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
            string url = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            fixture.TestAsset.ShouldBeEquivalentTo(parsedResponse, options => options
                .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsGet")]
        public async void CheckIfAssetExists()
        {
            string url = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

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
            string url = fixture.ApiEndpointNames["assets"] + "/default";
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

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
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsPost")]
        public async void EnableDisableAsset()
        {
            string disableUrl = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id + "/disable";
            string enableUrl = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id + "/enable";
            string parameter = JsonUtils.SerializeObject(new { id = fixture.TestAsset.Id });
            string url;

            if (fixture.TestAsset.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, parameter, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entity = await fixture.AssetManager.TryGetAsync(fixture.TestAsset.Id) as AssetEntity;
            Assert.True(entity.IsDisabled != fixture.TestAsset.IsDisabled);

            if (entity.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var responseAfter = await fixture.Consumer.ExecuteRequest(enableUrl, emptyDict, parameter, Method.POST);
            Assert.True(responseAfter.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
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

            await fixture.AssetManager.UpdateCacheAsync();
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
            string updateUrl = fixture.ApiEndpointNames["assets"];
            AssetDTO updateParamAsset = fixture.TestAssetUpdate;
            updateParamAsset.Name += "_AutoTestEdit";
            updateParamAsset.DefinitionUrl += "_AutoTest";

            string updateParam = JsonUtils.SerializeObject(updateParamAsset);

            var updateResponse = await fixture.Consumer.ExecuteRequest(updateUrl, emptyDict, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
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
            string deleteUrl = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAssetDelete.Id;
            string deleteParam = JsonUtils.SerializeObject(new { id = fixture.TestAssetDelete.Id });

            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, deleteParam, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityDeleted = await fixture.AssetManager.TryGetAsync(fixture.TestAssetDelete.Id) as AssetEntity;
            Assert.Null(entityDeleted);
        }
        #endregion

        #region asset attributes

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetAllAssetAttributes()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"];
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetAttributesReturnDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetAttributesReturnDTO>>(response.ResponseJson);

            fixture.AllAssetAttributesFromDB.Should().HaveSameCount(parsedResponse);

            for (int i = 0; i < fixture.AllAssetAttributesFromDB.Count; i++)
            {
                fixture.AllAssetAttributesFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttributes()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId;
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            fixture.TestAssetAttribute.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttribute()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId + "/" + fixture.TestAssetAttribute.Key;
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributeDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributeDTO>(response.ResponseJson);

            Assert.True(fixture.TestAssetAttribute.Value == parsedResponse.Value);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesPost")]
        public async void CreateAssetAttribute()
        {
            AssetAttributeIdentityDTO newAssetAttr = await fixture.CreateTestAssetAttribute();
            Assert.NotNull(newAssetAttr);

            var checkDb = await fixture.AssetAttributesRepository.TryGetAsync(newAssetAttr.AssetId, newAssetAttr.Key);
            Assert.True(checkDb.Value == newAssetAttr.Value);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesPut")]
        public async void UpdateAssetAttribute()
        {
            string updateUrl = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttributeUpdate.AssetId;
            string updateValue = fixture.TestAssetAttributeUpdate.Value + "_AutoTestEdit";
            string updateParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = fixture.TestAssetAttributeUpdate.Key, Value = updateValue });
            var updateResponse = await fixture.Consumer.ExecuteRequest(updateUrl, emptyDict, updateParameter, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            var checkDbUpdated = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttributeUpdate.AssetId, fixture.TestAssetAttributeUpdate.Key);
            Assert.True(checkDbUpdated.Value == updateValue);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesDelete")]
        public async void DeleteAssetAttribute()
        {
            string deleteUrl = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttributeDelete.AssetId + "/" + fixture.TestAssetAttributeDelete.Key;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var checkDbDeleted = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttributeDelete.AssetId, fixture.TestAssetAttributeDelete.Key);
            Assert.Null(checkDbDeleted);
        }
        #endregion

        #region asset categories

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesGet")]
        public async void GetAllAssetCategories()
        {
            string url = fixture.ApiEndpointNames["assetCategories"];
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetCategoryDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetCategoryDTO>>(response.ResponseJson);

            parsedResponse.Should().HaveSameCount(fixture.AllAssetCategoriesFromDB);

            for (int i = 0; i < fixture.AllAssetCategoriesFromDB.Count; i++)
            {
                fixture.AllAssetCategoriesFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == fixture.AllAssetCategoriesFromDB[i].Id).FirstOrDefault()
                , o => o.ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesGet")]
        public async void GetSingleAssetCategory()
        {
            string url = fixture.ApiEndpointNames["assetCategories"] + "/" + fixture.TestAssetCategory.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetCategoryDTO parsedResponse = JsonUtils.DeserializeJson<AssetCategoryDTO>(response.ResponseJson);

            AssetCategoryEntity entity = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategory.Id);
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesPost")]
        public async void CreateAssetCategory()
        {
            AssetCategoryDTO createdCategory = await fixture.CreateTestAssetCategory();
            Assert.NotNull(createdCategory);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbCreated = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(createdCategory.Id);
            checkDbCreated.ShouldBeEquivalentTo(createdCategory, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesPost")]
        public async void UpdateAssetCategory()
        {
            string url = fixture.ApiEndpointNames["assetCategories"];

            AssetCategoryDTO updateCategory = new AssetCategoryDTO()
            {
                Id = fixture.TestAssetCategoryUpdate.Id,
                Name = fixture.TestAssetCategoryUpdate.Name,
                AndroidIconUrl = fixture.TestAssetCategoryUpdate.AndroidIconUrl + "_autotest",
                IosIconUrl = fixture.TestAssetCategoryUpdate.IosIconUrl,
                SortOrder = fixture.TestAssetCategoryUpdate.SortOrder
            };
            string updateParam = JsonUtils.SerializeObject(updateCategory);

            var updateResponse = await fixture.Consumer.ExecuteRequest(url, emptyDict, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbUpdated = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategoryUpdate.Id);
            checkDbUpdated.ShouldBeEquivalentTo(updateCategory, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesDelete")]
        public async void DeleteAssetCategory()
        {
            string deleteUrl = fixture.ApiEndpointNames["assetCategories"] + "/" + fixture.TestAssetCategoryDelete.Id;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbDeleted = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(fixture.TestAssetCategoryDelete.Id);
            Assert.Null(checkDbDeleted);
        }
        #endregion

        #region asset extended infos

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoGet")]
        public async void GetAllAssetExtendedInfos()
        {
            string url = fixture.ApiEndpointNames["assetExtendedInfos"];
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetExtendedInfoDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetExtendedInfoDTO>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            fixture.AllAssetExtendedInfosFromDB.Should().HaveSameCount(parsedResponse);

            for (int i = 0; i < fixture.AllAssetExtendedInfosFromDB.Count; i++)
            {
                fixture.AllAssetExtendedInfosFromDB[i].ShouldBeEquivalentTo(
                    parsedResponse.Where(a => a.Id == fixture.AllAssetExtendedInfosFromDB[i].Id)
                    , o => o.ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoGet")]
        public async void GetSingleAssetExtendedInfo()
        {
            string url = fixture.ApiEndpointNames["assetExtendedInfos"] + "/" + fixture.TestAssetExtendedInfo.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetExtendedInfoDTO parsedResponse = JsonUtils.DeserializeJson<AssetExtendedInfoDTO>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            fixture.TestAssetExtendedInfo.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoGet")]
        public async void CheckIfAssetExtendedInfoExists()
        {
            string url = fixture.ApiEndpointNames["assetExtendedInfos"] + "/" + fixture.TestAssetExtendedInfo.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
            Assert.NotNull(response);
            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.NotNull(parsedResponse);
            Assert.True(parsedResponse);

            string badUrl = fixture.ApiEndpointNames["assetExtendedInfos"] + "/AutoTestAssetThatDoesntExist/exists";
            var badResponse = await fixture.Consumer.ExecuteRequest(badUrl, emptyDict, null, Method.GET);
            Assert.NotNull(badResponse);
            bool badParsedResponse = JsonUtils.DeserializeJson<bool>(badResponse.ResponseJson);
            Assert.NotNull(badParsedResponse);
            Assert.False(badParsedResponse);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoPost")]
        public async void CreateAssetExtendedInfo()
        {
            AssetExtendedInfoDTO createdInfo = await fixture.CreateTestAssetExtendedInfo();
            Assert.NotNull(createdInfo);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbCreated = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(createdInfo.Id);
            checkDbCreated.ShouldBeEquivalentTo(createdInfo, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoPut")]
        public async void UpdateAssetExtendedInfo()
        {
            string url = fixture.ApiEndpointNames["assetExtendedInfos"];

            AssetExtendedInfoDTO updateExtendedInfo = new AssetExtendedInfoDTO()
            {
                Id = fixture.TestAssetExtendedInfoUpdate.Id,
                AssetClass = fixture.TestAssetExtendedInfoUpdate.AssetClass,
                AssetDescriptionUrl = fixture.TestAssetExtendedInfoUpdate.AssetDescriptionUrl,
                Description = fixture.TestAssetExtendedInfoUpdate.Description,
                FullName = fixture.TestAssetExtendedInfoUpdate.FullName + "_autotestt",
                MarketCapitalization = fixture.TestAssetExtendedInfoUpdate.MarketCapitalization,
                NumberOfCoins = fixture.TestAssetExtendedInfoUpdate.NumberOfCoins,
                PopIndex = fixture.TestAssetExtendedInfoUpdate.PopIndex
            };
            string updateParam = JsonUtils.SerializeObject(updateExtendedInfo);

            var updateResponse = await fixture.Consumer.ExecuteRequest(url, emptyDict, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbUpdated = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(fixture.TestAssetExtendedInfoUpdate.Id);
            checkDbUpdated.ShouldBeEquivalentTo(updateParam, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoDelete")]
        public async void DeleteAssetExtendedInfo()
        {
            string deleteUrl = fixture.ApiEndpointNames["assetExtendedInfos"] + "/" + fixture.TestAssetExtendedInfoDelete.Id;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbDeleted = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(fixture.TestAssetExtendedInfoDelete.Id);
            Assert.Null(checkDbDeleted);
        }

        // /api/v2/asset-extended-infos/__default

        #endregion

        #region asset groups

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsGet")]
        public async void GetAllAssetGroups()
        {
            string url = fixture.ApiEndpointNames["assetGroups"];

            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetGroupDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetGroupDTO>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            fixture.AllAssetGroupsFromDB.Should().HaveSameCount(parsedResponse);
            for (int i = 0; i < fixture.AllAssetGroupsFromDB.Count; i++)
            {
                fixture.AllAssetGroupsFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers());
            }

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsGet")]
        public async void GetSingleAssetGroups()
        {
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestAssetGroup.Id;

            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
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
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestAssetGroup.Id + "/asset-ids";

            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            var entities = await fixture.AssetGroupsRepository.GetGroupAssetIDsAsync(fixture.TestAssetGroup.Id);
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            assetIds.Should().HaveSameCount(parsedResponse);

            for (int i = 0; i < assetIds.Count; i++)
            {
                Assert.True(assetIds[i] == parsedResponse[i]);
            }
            

        }

        //GetGroupAssetIDsAsync
        #endregion
    }
}
