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
                fixture.AllAssetsFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers()
                .Excluding(m => m.PartnerIds));
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

        [Fact(Skip = "Skip due to problems with creating lots of assets")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Assets")]
        [Trait("Category", "AssetsPost")]
        [Trait("Category", "AssetsPut")]
        [Trait("Category", "AssetsDelete")]
        public async void CreateUpdateDeleteAsset()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AssetEntity, AssetDTO>();
            });

            AssetDTO testAsset = Mapper.Map<AssetDTO>(fixture.TestAsset);
            AssetDTO testAssetUpdated = testAsset;
            testAsset.Id = testAsset.Id + "_AutoTest";
            //testAsset.id
            testAssetUpdated.Name = testAssetUpdated.Name + "_AutoTest";

            string createUrl = fixture.ApiEndpointNames["assets"];
            string deleteUrl = fixture.ApiEndpointNames["assets"] + "/" + testAsset.Id;

            string createParam = JsonUtils.SerializeObject(testAsset);
            string updateParam = JsonUtils.SerializeObject(testAssetUpdated);
            string deleteParam = JsonUtils.SerializeObject(new { id = testAsset.Id });

            //create asset
            var response = await fixture.Consumer.ExecuteRequest(createUrl, emptyDict, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.Created);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entity = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
            entity.ShouldBeEquivalentTo(testAsset, o => o
            .ExcludingMissingMembers());

            //Update assset
            var updateResponse = await fixture.Consumer.ExecuteRequest(fixture.ApiEndpointNames["assets"], emptyDict, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityUpdateed = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
            entityUpdateed.ShouldBeEquivalentTo(testAsset, o => o
            .ExcludingMissingMembers());

            //delete asset
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, deleteParam, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetManager.UpdateCacheAsync();
            AssetEntity entityDeleted = await fixture.AssetManager.TryGetAsync(testAsset.Id) as AssetEntity;
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

        [Fact(Skip = "Skip due to problems with creating lots of assets")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetAttributes")]
        [Trait("Category", "AssetsAttributesPost")]
        [Trait("Category", "AssetsAttributesPut")]
        [Trait("Category", "AssetsAttributesDelete")]
        public async void CreateUpdateDeleteAssetAttribute()
        {
            string newKey = fixture.TestAssetAttribute.Key + "_AutoTest";
            string newValue = "autotest";
            string updateValue = newValue + "_autotest";
            string createUrl = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId;
            string deleteUrl = fixture.ApiEndpointNames["assetAttributes"] + "/" + fixture.TestAssetAttribute.AssetId + "/" + newKey;
            string createParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = newKey, Value = newValue });
            string updateParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = newKey, Value = updateValue });

            //create asset attribute
            var response = await fixture.Consumer.ExecuteRequest(createUrl, emptyDict, createParameter, Method.POST);
            Assert.True(response.Status == HttpStatusCode.Created);

            var checkDb = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttribute.AssetId, newKey);
            Assert.True(checkDb.Value == newValue);

            //create asset attribute
            var updateResponse = await fixture.Consumer.ExecuteRequest(createUrl, emptyDict, updateParameter, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            var checkDbUpdated = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttribute.AssetId, newKey);
            Assert.True(checkDbUpdated.Value == updateValue);

            //delte the new attribute
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var checkDbDeleted = await fixture.AssetAttributesRepository.TryGetAsync(fixture.TestAssetAttribute.AssetId, newKey);
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

        [Fact(Skip = "Skip due to problems with creating lots of assets")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetCategories")]
        [Trait("Category", "AssetCategoriesGet")]
        public async void CreateUpdateDeleteAssetCategory()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AssetCategoryEntity, AssetCategoryDTO>();
            });

            string url = fixture.ApiEndpointNames["assetCategories"];

            AssetCategoryDTO newCategory = Mapper.Map<AssetCategoryDTO>(fixture.TestAssetCategory);
            newCategory.Id += "_AutoTest";
            string createParam = JsonUtils.SerializeObject(newCategory);

            AssetCategoryDTO updateCategory = new AssetCategoryDTO()
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                AndroidIconUrl = newCategory.AndroidIconUrl + "_autotest",
                IosIconUrl = newCategory.IosIconUrl,
                SortOrder = newCategory.SortOrder
            };
            string updateParam = JsonUtils.SerializeObject(updateCategory);


            //create category
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.Created);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbCreated = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(newCategory.Id);
            checkDbCreated.ShouldBeEquivalentTo(newCategory, o => o
            .ExcludingMissingMembers());

            //update category
            var updateResponse = await fixture.Consumer.ExecuteRequest(url, emptyDict, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbUpdated = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(newCategory.Id);
            checkDbUpdated.ShouldBeEquivalentTo(updateCategory, o => o
            .ExcludingMissingMembers());

            //delete category
            string deleteUrl = fixture.ApiEndpointNames["assetCategories"] + "/" + newCategory.Id;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetCategoryManager.UpdateCacheAsync();
            AssetCategoryEntity checkDbDeleted = (AssetCategoryEntity)await fixture.AssetCategoryManager.TryGetAsync(newCategory.Id);
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
                fixture.AllAssetExtendedInfosFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers());
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

        [Fact(Skip = "Skip due to problems with creating lots of assets")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetExtendedInfos")]
        [Trait("Category", "AssetExtendedInfoPost")]
        [Trait("Category", "AssetExtendedInfoPut")]
        [Trait("Category", "AssetExtendedInfoDelete")]
        public async void CreateUpdateDeleteAssetExtendedInfo()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AssetExtendedInfosEntity, AssetExtendedInfoDTO>();
            });

            string url = fixture.ApiEndpointNames["assetExtendedInfos"];

            AssetExtendedInfoDTO newExtendedInfo = Mapper.Map<AssetExtendedInfoDTO>(fixture.TestAssetExtendedInfo);
            newExtendedInfo.Id += "_AutoTest";
            string createParam = JsonUtils.SerializeObject(newExtendedInfo);

            AssetExtendedInfoDTO updateExtendedInfo = new AssetExtendedInfoDTO()
            {
                Id = newExtendedInfo.Id,
                AssetClass = newExtendedInfo.AssetClass,
                AssetDescriptionUrl = newExtendedInfo.AssetDescriptionUrl,
                Description = newExtendedInfo.Description,
                FullName = newExtendedInfo.FullName + "_autotest",
                MarketCapitalization = newExtendedInfo.MarketCapitalization,
                NumberOfCoins = newExtendedInfo.NumberOfCoins,
                PopIndex = newExtendedInfo.PopIndex
            };
            string updateParam = JsonUtils.SerializeObject(updateExtendedInfo);

            //create extended info
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.Created);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbCreated = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(newExtendedInfo.Id);
            checkDbCreated.ShouldBeEquivalentTo(createParam, o => o
            .ExcludingMissingMembers());

            //update extended info
            var updateResponse = await fixture.Consumer.ExecuteRequest(url, emptyDict, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbUpdated = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(newExtendedInfo.Id);
            checkDbUpdated.ShouldBeEquivalentTo(updateParam, o => o
            .ExcludingMissingMembers());

            //delete extended info
            string deleteUrl = fixture.ApiEndpointNames["assetExtendedInfos"] + "/" + newExtendedInfo.Id;
            var deleteResponse = await fixture.Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            await fixture.AssetExtendedInfosManager.UpdateCacheAsync();
            AssetExtendedInfosEntity checkDbDeleted = (AssetExtendedInfosEntity)await fixture.AssetExtendedInfosManager.TryGetAsync(newExtendedInfo.Id);
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
