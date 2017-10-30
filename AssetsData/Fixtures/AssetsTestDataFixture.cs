using Autofac;
using Autofac.Core;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using AssetsData.DependencyInjection;
using XUnitTestData.Domains.Assets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Repositories.Assets;
using System.Linq;
using XUnitTestData.Services;
using XUnitTestData.Domains;
using XUnitTestCommon.Utils;
using AutoMapper;
using AssetsData.DTOs;
using RestSharp;
using System.Net;
using AssetsData.DTOs.Assets;

namespace AssetsData.Fixtures
{
    public class AssetsTestDataFixture : IDisposable
    {
        #region fields
        public List<AssetEntity> AllAssetsFromDB;
        public AssetEntity TestAsset;
        public AssetDTO TestAssetUpdate;
        public AssetDTO TestAssetDelete;

        public List<AssetExtendedInfosEntity> AllAssetExtendedInfosFromDB;
        public AssetExtendedInfosEntity TestAssetExtendedInfo;
        public AssetExtendedInfoDTO TestAssetExtendedInfoUpdate;
        public AssetExtendedInfoDTO TestAssetExtendedInfoDelete;

        public List<AssetAttributesEntity> AllAssetAttributesFromDB;
        public AssetAttributesEntity TestAssetAttribute;
        public AssetAttributeIdentityDTO TestAssetAttributeUpdate;
        public AssetAttributeIdentityDTO TestAssetAttributeDelete;

        public List<AssetCategoryEntity> AllAssetCategoriesFromDB;
        public AssetCategoryEntity TestAssetCategory;
        public AssetCategoryDTO TestAssetCategoryUpdate;
        public AssetCategoryDTO TestAssetCategoryDelete;

        public List<AssetGroupEntity> AllAssetGroupsFromDB;
        public AssetGroupEntity TestAssetGroup;
        public AssetGroupDTO TestAssetGroupUpdate;
        public AssetGroupDTO TestAssetGroupDelete;

        public AssetGroupDTO TestGroupForGroupRelationAdd;
        public AssetDTO TestAssetForGroupRelationAdd;
        public AssetGroupDTO TestGroupForGroupRelationDelete;
        public AssetDTO TestAssetForGroupRelationDelete;
        public AssetGroupDTO TestGroupForClientRelationAdd;
        public AssetGroupDTO TestGroupForClientRelationDelete;
        public string TestAccountId;
        public AssetGroupDTO TestGroupForClientEndpoint;
        public AssetDTO TestAssetForClientEndpoint;
        public string TestAccountIdForClientEndpoint;

        public List<AssetPairEntity> AllAssetPairsFromDB;
        public AssetPairEntity TestAssetPair;
        public AssetPairDTO TestAssetPairUpdate;
        public AssetPairDTO TestAssetPairDelete;

        public List<AssetSettingsEntity> AllAssetSettingsFromDB;
        public AssetSettingsEntity TestAssetSettings;
        public AssetSettingsDTO TestAssetSettingsUpdate;
        public AssetSettingsDTO TestAssetSettingsDelete;

        public IDictionaryManager<IAsset> AssetManager;
        public IDictionaryManager<IAssetExtendedInfo> AssetExtendedInfosManager;
        public IDictionaryManager<IAssetCategory> AssetCategoryManager;
        public IDictionaryManager<IAssetAttributes> AssetAttributesManager;
        public IDictionaryManager<IAssetGroup> AssetGroupsManager;
        public IDictionaryManager<IAssetPair> AssetPairManager;
        public IDictionaryManager<IAssetSettings> AssetSettingsManager;

        public AssetAttributesRepository AssetAttributesRepository;
        public AssetGroupsRepository AssetGroupsRepository;
        private List<string> AssetsToDelete;
        private List<AssetAttributeIdentityDTO> AssetAtributesToDelete;
        private List<string> AssetCategoriesToDelete;
        private List<string> AssetExtendedInfosToDelete;
        private List<string> AssetGroupsToDelete;
        private List<string> AssetPairsToDelete;

        #endregion

        public ApiConsumer Consumer;
        public Dictionary<string, string> ApiEndpointNames;

        private IContainer container;

        private ConfigBuilder _configBuilder;

        public AssetsTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("Assets");
            this.Consumer = new ApiConsumer(this._configBuilder);

            prepareDependencyContainer();
            prepareTestData().Wait();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AssetsTestModule(_configBuilder));
            this.container = builder.Build();

            this.AssetManager = RepositoryUtils.PrepareRepositoryManager<IAsset>(this.container);
            this.AssetExtendedInfosManager = RepositoryUtils.PrepareRepositoryManager<IAssetExtendedInfo>(this.container);
            this.AssetCategoryManager = RepositoryUtils.PrepareRepositoryManager<IAssetCategory>(this.container);
            this.AssetAttributesManager = RepositoryUtils.PrepareRepositoryManager<IAssetAttributes>(this.container);
            this.AssetGroupsManager = RepositoryUtils.PrepareRepositoryManager<IAssetGroup>(this.container);
            this.AssetAttributesRepository = (AssetAttributesRepository)this.container.Resolve<IDictionaryRepository<IAssetAttributes>>();
            this.AssetGroupsRepository = (AssetGroupsRepository)this.container.Resolve<IDictionaryRepository<IAssetGroup>>();
            this.AssetPairManager = RepositoryUtils.PrepareRepositoryManager<IAssetPair>(this.container);
            this.AssetSettingsManager = RepositoryUtils.PrepareRepositoryManager<IAssetSettings>(this.container);
        }

        private async Task prepareTestData()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AssetEntity, AssetDTO>();
                cfg.CreateMap<AssetDTO, AssetEntity>();
                cfg.CreateMap<AssetCategoryEntity, AssetCategoryDTO>();
                cfg.CreateMap<AssetCategoryDTO, AssetCategoryEntity>();
                cfg.CreateMap<AssetExtendedInfosEntity, AssetExtendedInfoDTO>();
                cfg.CreateMap<AssetExtendedInfoDTO, AssetExtendedInfosEntity>();
                cfg.CreateMap<AssetGroupDTO, AssetGroupEntity>();
                cfg.CreateMap<AssetGroupEntity, AssetGroupDTO>();
                cfg.CreateMap<AssetPairDTO, AssetPairEntity>();
                cfg.CreateMap<AssetPairEntity, AssetPairDTO>();

            });

            this.AssetsToDelete = new List<string>();
            this.AssetAtributesToDelete = new List<AssetAttributeIdentityDTO>();
            this.AssetCategoriesToDelete = new List<string>();
            this.AssetExtendedInfosToDelete = new List<string>();
            this.AssetGroupsToDelete = new List<string>();
            this.AssetPairsToDelete = new List<string>();

            this.ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["assets"] = "/api/v2/assets";
            ApiEndpointNames["assetAttributes"] = "/api/v2/asset-attributes";
            ApiEndpointNames["assetCategories"] = "/api/v2/asset-categories";
            ApiEndpointNames["assetExtendedInfos"] = "/api/v2/asset-extended-infos";
            ApiEndpointNames["assetGroups"] = "/api/v2/asset-groups";
            ApiEndpointNames["assetPairs"] = "/api/v2/asset-pairs";
            ApiEndpointNames["assetSettings"] = "/api/v2/asset-settings";
            ApiEndpointNames["assetClients"] = "/api/v2/clients";

            var assetsFromDB = AssetManager.GetAllAsync();
            var AssetExtInfoFromDB = AssetExtendedInfosManager.GetAllAsync();
            var assetsAttrFromDB = AssetAttributesManager.GetAllAsync();
            var assetsCatsFromDB = AssetCategoryManager.GetAllAsync();
            var assetsGroupsFromDB = AssetGroupsManager.GetAllAsync();
            var assetPairsFromDB = AssetPairManager.GetAllAsync();
            var assetSettingsFromDB = AssetSettingsManager.GetAllAsync();

            this.AllAssetsFromDB = (await assetsFromDB).Cast<AssetEntity>().ToList();
            this.TestAsset = EnumerableUtils.PickRandom(AllAssetsFromDB);
            this.TestAssetUpdate = await CreateTestAsset();
            this.TestAssetDelete = await CreateTestAsset();

            this.AllAssetExtendedInfosFromDB = (await AssetExtInfoFromDB).Cast<AssetExtendedInfosEntity>().ToList();
            this.TestAssetExtendedInfo = EnumerableUtils.PickRandom(AllAssetExtendedInfosFromDB);
            this.TestAssetExtendedInfoUpdate = await CreateTestAssetExtendedInfo();
            this.TestAssetExtendedInfoDelete = await CreateTestAssetExtendedInfo();

            this.AllAssetAttributesFromDB = (await assetsAttrFromDB).Cast<AssetAttributesEntity>().ToList();
            this.TestAssetAttribute = EnumerableUtils.PickRandom(AllAssetAttributesFromDB);
            this.TestAssetAttributeUpdate = await CreateTestAssetAttribute();
            this.TestAssetAttributeDelete = await CreateTestAssetAttribute();

            this.AllAssetCategoriesFromDB = (await assetsCatsFromDB).Cast<AssetCategoryEntity>().ToList();
            this.TestAssetCategory = EnumerableUtils.PickRandom(AllAssetCategoriesFromDB);
            this.TestAssetCategoryUpdate = await CreateTestAssetCategory();
            this.TestAssetCategoryDelete = await CreateTestAssetCategory();

            this.AllAssetGroupsFromDB = (await assetsGroupsFromDB).Cast<AssetGroupEntity>().ToList();
            this.TestAssetGroup = EnumerableUtils.PickRandom(AllAssetGroupsFromDB);
            this.TestAssetGroupUpdate = await CreateTestAssetGroup();
            this.TestAssetGroupDelete = await CreateTestAssetGroup();

            this.AllAssetPairsFromDB = (await assetPairsFromDB).Cast<AssetPairEntity>().ToList();
            this.TestAssetPair = EnumerableUtils.PickRandom(AllAssetPairsFromDB);
            this.TestAssetPairUpdate = await CreateTestAssetPair();
            this.TestAssetPairDelete = await CreateTestAssetPair();

            this.TestGroupForGroupRelationAdd = await CreateTestAssetGroup();
            this.TestAssetForGroupRelationAdd = await CreateTestAsset();
            this.TestGroupForGroupRelationDelete = await CreateTestAssetGroup();
            this.TestAssetForGroupRelationDelete = await CreateTestAsset();
            this.TestGroupForClientRelationAdd = await CreateTestAssetGroup();
            this.TestGroupForClientRelationDelete = await CreateTestAssetGroup();
            this.TestAccountId = "AFTest_Client1";

            this.TestGroupForClientEndpoint = await CreateTestAssetGroup();
            this.TestAssetForClientEndpoint = await CreateTestAsset();
            this.TestAccountIdForClientEndpoint = "AFTest_Client2";
            await AddClientToGroup(TestAccountIdForClientEndpoint, TestGroupForClientEndpoint.Name);
            await AddAssetToGroup(TestAssetForClientEndpoint.Id, TestGroupForClientEndpoint.Name);

            this.AllAssetSettingsFromDB = (await assetSettingsFromDB).Cast<AssetSettingsEntity>().ToList();
            this.TestAssetSettings = EnumerableUtils.PickRandom(AllAssetSettingsFromDB);
            //TestAssetSettingsUpdate
            //TestAssetSettingsDelete



        }

        public void Dispose()
        {
            List<Task<bool>> deleteTasks = new List<Task<bool>>();

            foreach (string assetId in AssetsToDelete) { deleteTasks.Add(DeleteTestAsset(assetId)); }
            foreach (AssetAttributeIdentityDTO attrDTO in AssetAtributesToDelete) { deleteTasks.Add(DeleteTestAssetAttribute(attrDTO.AssetId, attrDTO.Key)); }
            foreach (string catId in AssetCategoriesToDelete) { deleteTasks.Add(DeleteTestAssetCategory(catId)); }
            foreach (string infoId in AssetExtendedInfosToDelete) { deleteTasks.Add(DeleteTestAssetExtendedInfo(infoId)); }
            foreach (string groupName in AssetGroupsToDelete) { deleteTasks.Add(DeleteTestAssetGroup(groupName)); }
            foreach (string pairId in AssetPairsToDelete) { deleteTasks.Add(DeleteTestAssetPair(pairId)); }

            Task.WhenAll(deleteTasks).Wait();
        }



        #region Create / Delete methods

        public async Task<AssetDTO> CreateTestAsset()
        {
            AssetDTO newAssetDTO = Mapper.Map<AssetDTO>(EnumerableUtils.PickRandom(AllAssetsFromDB));

            newAssetDTO.Id += Helpers.Random.Next(1000,9999).ToString() + "_AutoTest";
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
            string newKey = testAssetAttr.Key + Helpers.Random.Next(1000,9999).ToString() + "_AutoTest";
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
            newCategory.Id += Helpers.Random.Next(1000,9999).ToString() +  "_AutoTest";
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

            if(deleteResponse.Status != HttpStatusCode.NoContent)
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
            if(response.Status != HttpStatusCode.Created)
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

            createDTO.Id = templateEntity.Id + Helpers.Random.Next(1000,9999).ToString() + "_AutoTest";
            createDTO.Name += Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest";
            createDTO.Accuracy = Helpers.Random.Next(2, 8);
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if(response.Status != HttpStatusCode.Created)
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

        #endregion
    }
}
