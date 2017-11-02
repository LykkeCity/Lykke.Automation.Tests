using XUnitTestCommon;
using System;
using System.Threading.Tasks;
using XUnitTestData.Repositories.Assets;
using XUnitTestCommon.Utils;
using AutoMapper;
using AssetsData.DTOs;
using RestSharp;
using System.Net;
using AssetsData.DTOs.Assets;
using System.Collections.Generic;

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture : IDisposable
    {
        public async Task<AssetDTO> CreateTestAsset()
        {
            AssetDTO newAssetDTO = this.mapper.Map<AssetDTO>(EnumerableUtils.PickRandom(AllAssetsFromDB));

            newAssetDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            newAssetDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            newAssetDTO.BlockChainAssetId += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            newAssetDTO.BlockChainId += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";

            string createUrl = ApiEndpointNames["assets"];
            string createParam = JsonUtils.SerializeObject(newAssetDTO);

            var response = await Consumer.ExecuteRequest(createUrl, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AssetsToDelete.Add(newAssetDTO.Id);

            return newAssetDTO;
        }

        public async Task<bool> DeleteTestAsset(string id)
        {
            string deleteUrl = ApiEndpointNames["assets"] + "/" + id;
            string deleteParam = JsonUtils.SerializeObject(new { id = id });

            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, deleteParam, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetAttributeIdentityDTO> CreateTestAssetAttribute()
        {
            AssetAttributesEntity testAssetAttr = EnumerableUtils.PickRandom(AllAssetAttributesFromDB);

            string createUrl = ApiEndpointNames["assetAttributes"] + "/" + testAssetAttr.AssetId;
            string newKey = testAssetAttr.Key + Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            string newValue = Helpers.Random.Next(1000, 9999).ToString() + "_autotest";

            AssetAttributeDTO createParameter = new AssetAttributeDTO() { Key = newKey, Value = newValue };

            var response = await Consumer.ExecuteRequest(createUrl, Helpers.EmptyDictionary, JsonUtils.SerializeObject(createParameter), Method.POST);
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

            this.AssetAtributesToDelete.Add(returnModel);

            return returnModel;
        }

        public async Task<bool> DeleteTestAssetAttribute(string assetId, string key)
        {
            string deleteUrl = ApiEndpointNames["assetAttributes"] + "/" + assetId + "/" + key;

            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }

        public async Task<AssetCategoryDTO> CreateTestAssetCategory()
        {
            string url = ApiEndpointNames["assetCategories"];

            AssetCategoryDTO newCategory = this.mapper.Map<AssetCategoryDTO>(EnumerableUtils.PickRandom(AllAssetCategoriesFromDB));
            newCategory.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            newCategory.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            string createParam = JsonUtils.SerializeObject(newCategory);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            this.AssetCategoriesToDelete.Add(newCategory.Id);

            return newCategory;
        }

        public async Task<bool> DeleteTestAssetCategory(string id)
        {
            string deleteUrl = ApiEndpointNames["assetCategories"] + "/" + id;
            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetExtendedInfoDTO> CreateTestAssetExtendedInfo(bool deleteWithDispose = true)
        {
            string url = ApiEndpointNames["assetExtendedInfos"];

            AssetExtendedInfoDTO newExtendedInfo = this.mapper.Map<AssetExtendedInfoDTO>(EnumerableUtils.PickRandom(AllAssetExtendedInfosFromDB));
            newExtendedInfo.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            newExtendedInfo.FullName += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            string createParam = JsonUtils.SerializeObject(newExtendedInfo);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            this.AssetExtendedInfosToDelete.Add(newExtendedInfo.Id);

            return newExtendedInfo;
        }

        public async Task<bool> DeleteTestAssetExtendedInfo(string id)
        {
            string deleteUrl = ApiEndpointNames["assetExtendedInfos"] + "/" + id;
            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetGroupDTO> CreateTestAssetGroup()
        {
            string url = ApiEndpointNames["assetGroups"];

            AssetGroupDTO newAssetGroup = this.mapper.Map<AssetGroupDTO>(EnumerableUtils.PickRandom(AllAssetGroupsFromDB));
            newAssetGroup.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";

            string createParam = JsonUtils.SerializeObject(newAssetGroup);
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AssetGroupsToDelete.Add(newAssetGroup.Name);

            return newAssetGroup;
        }

        public async Task<bool> AddClientToGroup(string clientId, string groupName)
        {
            string url = ApiEndpointNames["assetGroups"] + "/" + groupName + "/clients/" + clientId;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            return response.Status == HttpStatusCode.NoContent;
        }

        public async Task<bool> AddAssetToGroup(string assetId, string groupName)
        {
            string url = ApiEndpointNames["assetGroups"] + "/" + groupName + "/assets/" + assetId;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            return response.Status == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteTestAssetGroup(string name)
        {
            string deleteUrl = ApiEndpointNames["assetGroups"] + "/" + name;
            var response = await Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetPairDTO> CreateTestAssetPair()
        {
            string url = ApiEndpointNames["assetPairs"];
            AssetPairEntity templateEntity = EnumerableUtils.PickRandom(AllAssetPairsFromDB);
            AssetPairDTO createDTO = this.mapper.Map<AssetPairDTO>(templateEntity);

            createDTO.Id = templateEntity.Id + Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Accuracy = Helpers.Random.Next(2, 8);
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AssetPairsToDelete.Add(createDTO.Id);

            return createDTO;
        }

        public async Task<bool> DeleteTestAssetPair(string id)
        {
            string url = ApiEndpointNames["assetPairs"] + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetIssuerDTO> CreateTestAssetIssuer()
        {
            string url = ApiEndpointNames["assetIssuers"];
            AssetIssuersEntity templateEntity = EnumerableUtils.PickRandom(AllAssetIssuersFromDB);
            AssetIssuerDTO createDTO = this.mapper.Map<AssetIssuerDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.IconUrl += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            AssetIssuersToDelete.Add(createDTO.Id);

            return createDTO;
        }

        public async Task<bool> DeleteTestAssetIssuer(string id)
        {
            string url = ApiEndpointNames["assetIssuers"] + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<MarginAssetPairDTO> CreateTestMarginAssetPair()
        {
            string url = ApiEndpointNames["marginAssetPairs"];
            MarginAssetPairsEntity templateEntity = EnumerableUtils.PickRandom(AllMarginAssetPairsFromDB);
            MarginAssetPairDTO createDTO = this.mapper.Map<MarginAssetPairDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Accuracy = Helpers.Random.Next(2, 8);
            createDTO.InvertedAccuracy = Helpers.Random.Next(2, 8);
            createDTO.BaseAssetId += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.QuotingAssetId += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            MarginAssetPairsToDelete.Add(createDTO.Id);

            return createDTO;
        }

        public async Task<bool> DeleteTestMarginAssetPair(string id)
        {
            string url = ApiEndpointNames["marginAssetPairs"] + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<MarginAssetDTO> CreateTestMarginAsset()
        {
            string url = ApiEndpointNames["marginAssets"];
            MarginAssetEntity templateEntity = EnumerableUtils.PickRandom(AllMarginAssetsFromDB);
            MarginAssetDTO createDTO = this.mapper.Map<MarginAssetDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.IdIssuer += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Accuracy = Helpers.Random.Next(2, 8);
            createDTO.DustLimit = Helpers.Random.NextDouble();
            createDTO.Multiplier = Helpers.Random.NextDouble();
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            MarginAssetsToDelete.Add(createDTO.Id);

            return createDTO;
        }

        public async Task<bool> DeleteTestMarginAsset(string id)
        {
            string url = ApiEndpointNames["marginAssets"] + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<MarginIssuerDTO> CreateTestMarginIssuer()
        {
            string url = ApiEndpointNames["marginIssuers"];
            MarginIssuerEntity templateEntity = EnumerableUtils.PickRandom(AllMarginIssuersFromDB);
            MarginIssuerDTO createDTO = this.mapper.Map<MarginIssuerDTO>(templateEntity);

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.IconUrl += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.OK) //HttpStatusCode.Created
            {
                return null;
            }

            MarginIssuersToDelete.Add(createDTO.Id);

            return createDTO;
        }

        public async Task<bool> DeleteTestMarginIssuer(string id)
        {
            string url = ApiEndpointNames["marginIssuers"] + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<WatchListDTO> CreateTestWatchList(string clientId = null)
        {
            string url = ApiEndpointNames["watchList"];
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

            createDTO.Id += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.ReadOnly = readOnlyValue;
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, queryParams, createParam, Method.POST);

            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            WatchListsToDelete.Add(createDTO.Id, clientId);

            return createDTO;
        }

        public async Task<bool> DeleteTestWatchList(KeyValuePair<string, string> WatchListIDs)
        {
            string watchListId = WatchListIDs.Key;
            string clientId = WatchListIDs.Value;
            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            string url = ApiEndpointNames["watchList"];
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
    }
}
