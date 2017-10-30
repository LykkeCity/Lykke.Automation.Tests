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

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture : IDisposable
    {
        public async Task<AssetDTO> CreateTestAsset()
        {
            AssetDTO newAssetDTO = Mapper.Map<AssetDTO>(EnumerableUtils.PickRandom(AllAssetsFromDB));

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

            AssetCategoryDTO newCategory = Mapper.Map<AssetCategoryDTO>(EnumerableUtils.PickRandom(AllAssetCategoriesFromDB));
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

            AssetExtendedInfoDTO newExtendedInfo = Mapper.Map<AssetExtendedInfoDTO>(EnumerableUtils.PickRandom(AllAssetExtendedInfosFromDB));
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

            AssetGroupDTO newAssetGroup = Mapper.Map<AssetGroupDTO>(EnumerableUtils.PickRandom(AllAssetGroupsFromDB));
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
            AssetPairDTO createDTO = Mapper.Map<AssetPairDTO>(templateEntity);

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
    }
}
