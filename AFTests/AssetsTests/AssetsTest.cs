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
using XUnitTestCommon;
using System.Threading.Tasks;

namespace AFTests.AssetsTests
{

    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        private AssetsTestDataFixture fixture;

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
            string url = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id;
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
            string url = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAsset.Id + "/exists";
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

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsDelete")]
        public async void DeleteAsset()
        {
            string deleteUrl = fixture.ApiEndpointNames["assets"] + "/" + fixture.TestAssetDelete.Id;
            string deleteParam = JsonUtils.SerializeObject(new { id = fixture.TestAssetDelete.Id });

            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, deleteParam, Method.DELETE);
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
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetAttributesReturnDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetAttributesReturnDTO>>(response.ResponseJson);


            for (int i = 0; i < fixture.AllAssetAttributesFromDB.Count; i++)
            {
                fixture.AllAssetAttributesFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers()
                .Excluding(a => a.Attributes));
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttributes()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            fixture.TestAssetAttribute.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(p => p.Attributes));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesGet")]
        public async void GetSingleAssetAttribute()
        {
            string url = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId + "/" + fixture.TestAssetAttribute.Key;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

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
            var updateResponse = await fixture.Consumer.ExecuteRequest(updateUrl, Helpers.EmptyDictionary, updateParameter, Method.PUT);
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
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
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
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetCategoryDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetCategoryDTO>>(response.ResponseJson);

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
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

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

            var updateResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
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
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
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
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetExtendedInfoDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetExtendedInfoDTO>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

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
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
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
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.NotNull(parsedResponse);
            Assert.True(parsedResponse);

            string badUrl = fixture.ApiEndpointNames["assetExtendedInfos"] + "/AutoTestAssetThatDoesntExist/exists";
            var badResponse = await fixture.Consumer.ExecuteRequest(badUrl, Helpers.EmptyDictionary, null, Method.GET);
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

            var updateResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
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
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
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
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestAssetGroup.Id;

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
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestAssetGroup.Id + "/asset-ids";

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            var entities = await fixture.AssetGroupsRepository.GetGroupAssetIDsAsync(fixture.TestAssetGroup.Id);
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
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestAssetGroup.Id + "/client-ids";

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            Assert.NotNull(parsedResponse);

            var entities = await fixture.AssetGroupsRepository.GetGroupClientIDsAsync(fixture.TestAssetGroup.Id);
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

            await fixture.AssetGroupsManager.UpdateCacheAsync();
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
            string url = fixture.ApiEndpointNames["assetGroups"];

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

            await fixture.AssetGroupsManager.UpdateCacheAsync();
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
            string deleteUrl = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestAssetGroupDelete.Name;
            var response = await fixture.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.AssetGroupsManager.UpdateCacheAsync();
            AssetGroupEntity entity = await fixture.AssetGroupsManager.TryGetAsync(fixture.TestAssetGroupDelete.Name) as AssetGroupEntity;
            Assert.Null(entity);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPost")]
        public async void AddAssetToAssetGroup()
        {
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestGroupForGroupRelationAdd.Name + "/assets/" + fixture.TestAssetForGroupRelationAdd.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetGroupAssetIDsAsync(fixture.TestGroupForGroupRelationAdd.Name);
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
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestGroupForGroupRelationDelete.Name + "/assets/" + fixture.TestAssetForGroupRelationDelete.Id;
            var createResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(createResponse.Status == HttpStatusCode.NoContent);

            var deleteResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetGroupAssetIDsAsync(fixture.TestGroupForGroupRelationDelete.Name);
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            Assert.True(assetIds.Count == 0);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetGroups")]
        [Trait("Category", "AssetGroupsPost")]
        public async void AddClientToAssetGroup()
        {
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestGroupForClientRelationAdd.Name + "/clients/" + fixture.TestAccountId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetGroupClientIDsAsync(fixture.TestGroupForClientRelationAdd.Name);
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
            string url = fixture.ApiEndpointNames["assetGroups"] + "/" + fixture.TestGroupForClientRelationDelete.Name + "/clients/" + fixture.TestAccountId;
            var createResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(createResponse.Status == HttpStatusCode.NoContent);

            var deleteResponse = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var entities = await fixture.AssetGroupsRepository.GetGroupClientIDsAsync(fixture.TestGroupForClientRelationDelete.Name);
            List<string> assetIds = entities.Select(e => e.Id).ToList();

            Assert.True(assetIds.Count == 0);
        }


        #endregion

        #region asset pairs

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetPairs")]
        [Trait("Category", "AsestPairsGet")]
        public async void GetAllAssetPairs()
        {
            string url = fixture.ApiEndpointNames["assetPairs"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetPairDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetPairDTO>>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetPairsFromDB.Count; i++)
            {
                fixture.AllAssetPairsFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == fixture.AllAssetPairsFromDB[i].Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetPairs")]
        [Trait("Category", "AsestPairsGet")]
        public async void GetSingleAssetPair()
        {
            string url = fixture.ApiEndpointNames["assetPairs"] + "/" + fixture.TestAssetPair.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetPairDTO parsedResponse = JsonUtils.DeserializeJson<AssetPairDTO>(response.ResponseJson);
            fixture.TestAssetPair.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetPairs")]
        [Trait("Category", "AsestPairsGet")]
        public async void CheckIfAssetPairExists()
        {
            string url = fixture.ApiEndpointNames["assetPairs"] + "/" + fixture.TestAssetPair.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetPairs")]
        [Trait("Category", "AsestPairsPost")]
        public async void CreateAssetPair()
        {
            AssetPairDTO newAssetPair = await fixture.CreateTestAssetPair();
            Assert.NotNull(newAssetPair);

            await fixture.AssetPairManager.UpdateCacheAsync();
            AssetPairEntity entity = await fixture.AssetPairManager.TryGetAsync(newAssetPair.Id) as AssetPairEntity;
            entity.ShouldBeEquivalentTo(newAssetPair, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetPairs")]
        [Trait("Category", "AsestPairsPut")]
        public async void UpdateAssetPair()
        {
            string url = fixture.ApiEndpointNames["assetPairs"];
            AssetPairDTO updateAssetPair = new AssetPairDTO()
            {
                Accuracy = Helpers.Random.Next(2, 8),
                BaseAssetId = fixture.TestAssetPairUpdate.BaseAssetId,
                Id = fixture.TestAssetPairUpdate.Id,
                InvertedAccuracy = fixture.TestAssetPairUpdate.InvertedAccuracy,
                IsDisabled = fixture.TestAssetPairUpdate.IsDisabled,
                Name = fixture.TestAssetPairUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest",
                QuotingAssetId = fixture.TestAssetPairUpdate.QuotingAssetId,
                Source = fixture.TestAssetPairUpdate.Source,
                Source2 = fixture.TestAssetPairUpdate.Source2
            };
            string updateParam = JsonUtils.SerializeObject(updateAssetPair);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.AssetPairManager.UpdateCacheAsync();
            AssetPairEntity entity = await fixture.AssetPairManager.TryGetAsync(updateAssetPair.Id) as AssetPairEntity;
            entity.ShouldBeEquivalentTo(updateAssetPair, o => o
            .ExcludingMissingMembers());

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetPairs")]
        [Trait("Category", "AsestPairsDelete")]
        public async void DeleteAssetPair()
        {
            string url = fixture.ApiEndpointNames["assetPairs"] + "/" + fixture.TestAssetPairDelete.Id;
            var result = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.NotNull(result);
            Assert.True(result.Status == HttpStatusCode.NoContent);

            await fixture.AssetPairManager.UpdateCacheAsync();
            AssetPairEntity entity = await fixture.AssetPairManager.TryGetAsync(fixture.TestAssetPairDelete.Id) as AssetPairEntity;
            Assert.Null(entity);
        }

        //GET /api/v2/asset-pairs/__default
        #endregion

        #region Asset settings
        [Fact(Skip = "Test will fail due to mismatch in data types")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetSettings")]
        [Trait("Category", "AssetSettingsGet")]
        public async void GetAllAssetSettings()
        {
            string url = fixture.ApiEndpointNames["assetSettings"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AllAssetSettingsDTO parsedRseponse = JsonUtils.DeserializeJson<AllAssetSettingsDTO>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetSettingsFromDB.Count; i++)
            {
                fixture.AllAssetSettingsFromDB[i].ShouldBeEquivalentTo(
                    parsedRseponse.Items.Where(p => p.Id == fixture.AllAssetSettingsFromDB[i].Id).FirstOrDefault(),
                    o => o.ExcludingMissingMembers());
            }
        }

        [Fact(Skip = "Test will fail due to mismatch in data types")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetSettings")]
        [Trait("Category", "AssetSettingsGet")]
        public async void GetSingleAssetSettings()
        {
            string url = fixture.ApiEndpointNames["assetSettings"] + "/" + fixture.TestAssetSettings.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.NotNull(response);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetSettingsDTO parsedRseponse = JsonUtils.DeserializeJson<AssetSettingsDTO>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetSettingsFromDB.Count; i++)
            {
                fixture.AllAssetSettingsFromDB[i].ShouldBeEquivalentTo(parsedRseponse, o => o
                .ExcludingMissingMembers());
            }
        }

        #endregion

        #region clients
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetClients")]
        [Trait("Category", "AssetClientsGet")]
        public async void GetClientAssetIDs()
        {
            string url = fixture.ApiEndpointNames["assetClients"] + "/" + fixture.TestAccountIdForClientEndpoint + "/asset-ids";
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString();

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            parsedResponse.Should().HaveCount(1);
            Assert.True(parsedResponse[0] == fixture.TestAssetForClientEndpoint.Id);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetClients")]
        [Trait("Category", "AssetClientsGet")]
        public async void GetClientSwiftDepositOption()
        {
            string url = fixture.ApiEndpointNames["assetClients"] + "/" + fixture.TestAccountIdForClientEndpoint + "/swift-deposit-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString();

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == fixture.TestGroupForClientEndpoint.SwiftDepositEnabled);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetClients")]
        [Trait("Category", "AssetClientsGet")]
        public async void GetClientCashInBankOption()
        {
            string url = fixture.ApiEndpointNames["assetClients"] + "/" + fixture.TestAccountIdForClientEndpoint + "/cash-in-via-bank-card-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString();

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == fixture.TestGroupForClientEndpoint.ClientsCanCashInViaBankCards);

        }

        #endregion

        //[Fact]
        //public async void CleanUp()
        //{
        //    var allAutoTestAssets = fixture.AllAssetsFromDB.Where(a => a.Id.EndsWith("_AutoTest")).ToList();
        //    var allAutoTestAssetGroups = fixture.AllAssetGroupsFromDB.Where(a => a.Name.EndsWith("_AutoTest")).ToList();
        //    var allAutoTestAssetPairs = fixture.AllAssetPairsFromDB.Where(a => a.Name.EndsWith("_AutoTest")).ToList();

        //    List<Task<bool>> deleteTasks = new List<Task<bool>>();
        //    foreach (var asset in allAutoTestAssets) { deleteTasks.Add(fixture.DeleteTestAsset(asset.Id)); }
        //    foreach (var group in allAutoTestAssetGroups) { deleteTasks.Add(fixture.DeleteTestAssetGroup(group.Name)); }
        //    foreach (var pair in allAutoTestAssetPairs) { deleteTasks.Add(fixture.DeleteTestAssetPair(pair.Id)); }

        //    Task.WhenAll(deleteTasks).Wait();
        //}
    }
}
