using XUnitTestCommon;
using System;
using System.Threading.Tasks;
using XUnitTestCommon.Utils;
using AutoMapper;
using AssetsData.DTOs;
using RestSharp;
using System.Net;
using AssetsData.DTOs.Assets;
using System.Collections.Generic;
using XUnitTestData.Entities.Assets;
using XUnitTestCommon.Tests;

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture : BaseTest
    {
        public async Task<AssetDTO> CreateTestAsset()
        {
            AssetDTO newAssetDTO = this.mapper.Map<AssetDTO>(EnumerableUtils.PickRandom(AllAssetsFromDB));

            newAssetDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            newAssetDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            newAssetDTO.BlockChainAssetId += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            newAssetDTO.BlockChainId += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            newAssetDTO.Accuracy = Helpers.Random.Next(2, 6);
            newAssetDTO.MultiplierPower = Helpers.Random.Next(6, 10);

            string url = ApiPaths.ASSETS_V2_BASE_PATH;
            string createParam = JsonUtils.SerializeObject(newAssetDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestAsset(newAssetDTO.Id));

            return newAssetDTO;
        }

        public async Task<bool> DeleteTestAsset(string id)
        {
            string url = ApiPaths.ASSETS_V2_BASE_PATH + "/" + id;
            string deleteParam = JsonUtils.SerializeObject(new { id = id });

            var deleteResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, deleteParam, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetAttributeIdentityDTO> CreateTestAssetAttribute()
        {
            AssetAttributesEntity testAssetAttr = EnumerableUtils.PickRandom(AllAssetAttributesFromDB);

            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + testAssetAttr.AssetId;
            string newKey = testAssetAttr.Key + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            string newValue = Helpers.Random.Next(1000, 9999).ToString() + "_autotest";

            AssetAttributeDTO createParameter = new AssetAttributeDTO() { Key = newKey, Value = newValue };

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(createParameter), Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AssetAttributeIdentityDTO returnModel = new AssetAttributeIdentityDTO()
            {
                AssetId = testAssetAttr.AssetId,
                Key = newKey,
                Value = newValue
            };

            AddOneTimeCleanupAction(async () => await DeleteTestAssetAttribute(returnModel.AssetId, returnModel.Key));

            return returnModel;
        }

        public async Task<bool> DeleteTestAssetAttribute(string assetId, string key)
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + assetId + "/" + key;

            var deleteResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }

        public async Task<AssetCategoryDTO> CreateTestAssetCategory()
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH;

            AssetCategoryDTO newCategory = this.mapper.Map<AssetCategoryDTO>(EnumerableUtils.PickRandom(AllAssetCategoriesFromDB));
            newCategory.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            newCategory.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            string createParam = JsonUtils.SerializeObject(newCategory);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestAssetCategory(newCategory.Id));

            return newCategory;
        }

        public async Task<bool> DeleteTestAssetCategory(string id)
        {
            string url = ApiPaths.ASSET_CATEGORIES_PATH + "/" + id;
            var deleteResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetExtendedInfoDTO> CreateTestAssetExtendedInfo(bool deleteWithDispose = true)
        {
            string url =ApiPaths.ASSET_EXTENDED_INFO_PATH;

            AssetExtendedInfoDTO newExtendedInfo = this.mapper.Map<AssetExtendedInfoDTO>(EnumerableUtils.PickRandom(AllAssetExtendedInfosFromDB));
            newExtendedInfo.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            newExtendedInfo.FullName += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            string createParam = JsonUtils.SerializeObject(newExtendedInfo);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestAssetExtendedInfo(newExtendedInfo.Id));

            return newExtendedInfo;
        }

        public async Task<bool> DeleteTestAssetExtendedInfo(string id)
        {
            string url = ApiPaths.ASSET_EXTENDED_INFO_PATH + "/" + id;
            var deleteResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetGroupDTO> CreateTestAssetGroup()
        {
            string url = ApiPaths.ASSET_GROUPS_PATH;

            AssetGroupDTO newAssetGroup = this.mapper.Map<AssetGroupDTO>(EnumerableUtils.PickRandom(AllAssetGroupsFromDB));
            newAssetGroup.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;

            string createParam = JsonUtils.SerializeObject(newAssetGroup);
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestAssetGroup(newAssetGroup.Name));

            return newAssetGroup;
        }

        public async Task<bool> AddClientToGroup(string clientId, string groupName)
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + groupName + "/clients/" + clientId;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            return response.Status == HttpStatusCode.NoContent;
        }

        public async Task<bool> AddAssetToGroup(string assetId, string groupName)
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + groupName + "/assets/" + assetId;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            return response.Status == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteTestAssetGroup(string name)
        {
            string url = ApiPaths.ASSET_GROUPS_PATH + "/" + name;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetPairDTO> CreateTestAssetPair()
        {
            string url = ApiPaths.ASSET_PAIRS_PATH;
            AssetPairEntity templateEntity = EnumerableUtils.PickRandom(AllAssetPairsFromDB);
            AssetPairDTO createDTO = this.mapper.Map<AssetPairDTO>(templateEntity);

            createDTO.Id = templateEntity.Id + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Accuracy = Helpers.Random.Next(2, 8);
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestAssetPair(createDTO.Id));

            return createDTO;
        }

        public async Task<bool> DeleteTestAssetPair(string id)
        {
            string url = ApiPaths.ASSET_PAIRS_PATH + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetIssuerDTO> CreateTestAssetIssuer()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH;
            AssetIssuersEntity templateEntity = EnumerableUtils.PickRandom(AllAssetIssuersFromDB);
            AssetIssuerDTO createDTO = this.mapper.Map<AssetIssuerDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.IconUrl += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestAssetIssuer(createDTO.Id));

            return createDTO;
        }

        public async Task<bool> DeleteTestAssetIssuer(string id)
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<MarginAssetPairDTO> CreateTestMarginAssetPair()
        {
            string url = ApiPaths.MARGIN_ASSET_PAIRS_PATH;
            MarginAssetPairsEntity templateEntity = EnumerableUtils.PickRandom(AllMarginAssetPairsFromDB);
            MarginAssetPairDTO createDTO = this.mapper.Map<MarginAssetPairDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Accuracy = Helpers.Random.Next(2, 8);
            createDTO.InvertedAccuracy = Helpers.Random.Next(2, 8);
            createDTO.BaseAssetId += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.QuotingAssetId += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestMarginAssetPair(createDTO.Id));

            return createDTO;
        }

        public async Task<bool> DeleteTestMarginAssetPair(string id)
        {
            string url = ApiPaths.MARGIN_ASSET_PAIRS_PATH + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<MarginAssetDTO> CreateTestMarginAsset()
        {
            string url = ApiPaths.MARGIN_ASSET_BASE_PATH;
            MarginAssetEntity templateEntity = EnumerableUtils.PickRandom(AllMarginAssetsFromDB);
            MarginAssetDTO createDTO = this.mapper.Map<MarginAssetDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.IdIssuer += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Accuracy = Helpers.Random.Next(2, 8);
            createDTO.DustLimit = Helpers.Random.NextDouble();
            createDTO.Multiplier = Helpers.Random.NextDouble();
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestMarginAsset(createDTO.Id));

            return createDTO;
        }

        public async Task<bool> DeleteTestMarginAsset(string id)
        {
            string url = ApiPaths.MARGIN_ASSET_BASE_PATH + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<MarginIssuerDTO> CreateTestMarginIssuer()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH;
            MarginIssuerEntity templateEntity = EnumerableUtils.PickRandom(AllMarginIssuersFromDB);
            MarginIssuerDTO createDTO = this.mapper.Map<MarginIssuerDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.IconUrl += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.OK) //HttpStatusCode.Created
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestMarginIssuer(createDTO.Id));

            return createDTO;
        }

        public async Task<bool> DeleteTestMarginIssuer(string id)
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<WatchListDTO> CreateTestWatchList(string clientId = null)
        {
            string url = ApiPaths.WATCH_LIST_BASE_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            bool readOnlyValue = true;
            if (clientId == null)
            {
                url += "/predefined";
            }
            else
            {
                url += "/custom";
                queryParams.Add("userId", clientId);
                readOnlyValue = false;
            }

            WatchListEntity tempalteEntity = EnumerableUtils.PickRandom(AllWatchListsFromDB);
            WatchListDTO createDTO = new WatchListDTO()
            {
                Id = tempalteEntity.Id,
                Name = tempalteEntity.Name,
                ReadOnly = tempalteEntity.ReadOnly,
                Order = tempalteEntity.Order,
                AssetIds = tempalteEntity.AssetIDsList
            };

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest;
            createDTO.ReadOnly = readOnlyValue;
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, queryParams, createParam, Method.POST);

            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AddOneTimeCleanupAction(async () => await DeleteTestWatchList(createDTO.Id, clientId));

            return createDTO;
        }

        public async Task<bool> DeleteTestWatchList(string watchListId, string clientId)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            string url = ApiPaths.WATCH_LIST_BASE_PATH;
            if (clientId == null)
            {
                url += "/predefined";
            }
            else
            {
                url += "/custom";
                queryParams.Add("userId", clientId);
            }
            url += "/" + watchListId;

            var response = await Consumer.ExecuteRequest(url, queryParams, null, Method.DELETE);
            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }

        public async Task<AssetSettingsDTO> CreateTestAssetSettings()
        {
            string url = ApiPaths.ASSET_SETTINGS_PATH;
            AssetSettingsCreateDTO createDTO = new AssetSettingsCreateDTO()
            {
                Asset = Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                CashinCoef = Helpers.Random.Next(1, 10),
                ChangeWallet = Guid.NewGuid().ToString() + GlobalConstants.AutoTest,
                Dust = Helpers.Random.NextDouble(),
                HotWallet = Guid.NewGuid().ToString() + GlobalConstants.AutoTest,
                MaxBalance = Helpers.Random.Next(100,1000),
                MaxOutputsCount = Helpers.Random.Next(1, 100),
                MaxOutputsCountInTx = Helpers.Random.Next(1, 100),
                MinBalance = Helpers.Random.Next(1, 100),
                MinOutputsCount = Helpers.Random.Next(1, 100),
                OutputSize = Helpers.Random.Next(100, 10000),
                PrivateIncrement = 0
            };
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AssetSettingsDTO parsedResponse = JsonUtils.DeserializeJson<AssetSettingsDTO>(response.ResponseJson);

            AddOneTimeCleanupAction(async () => await DeleteTestAssetSettings(parsedResponse.Asset));

            return parsedResponse;
        }

        public async Task<bool> DeleteTestAssetSettings(string id)
        {
            string url = ApiPaths.ASSET_SETTINGS_PATH + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }
    }
}
