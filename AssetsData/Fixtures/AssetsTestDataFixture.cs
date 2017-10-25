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

        public string TestAttributeKey;

        public IDictionaryManager<IAsset> AssetManager;
        public IDictionaryManager<IAssetExtendedInfo> AssetExtendedInfosManager;
        public IDictionaryManager<IAssetCategory> AssetCategoryManager;
        public IDictionaryManager<IAssetAttributes> AssetAttributesManager;
        public IDictionaryManager<IAssetGroup> AssetGroupsManager;

        public AssetAttributesRepository AssetAttributesRepository;
        public AssetGroupsRepository AssetGroupsRepository;

        public ApiConsumer Consumer;
        public Dictionary<string, string> ApiEndpointNames;

        private List<string> AssetsToDelete;
        private List<AssetAttributeIdentityDTO> AssetAtributesToDelete;
        private List<string> AssetCategoriesToDelete;
        private List<string> AssetExtendedInfosToDelete;

        private Dictionary<string, string> emptyDict = new Dictionary<string, string>();

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
        }

        private async Task prepareTestData()
        {
            this.AssetsToDelete = new List<string>();
            this.AssetAtributesToDelete = new List<AssetAttributeIdentityDTO>();
            this.AssetCategoriesToDelete = new List<string>();
            this.AssetExtendedInfosToDelete = new List<string>();

            this.ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["assets"] = "/api/v2/assets";
            ApiEndpointNames["assetAttributes"] = "/api/v2/asset-attributes";
            ApiEndpointNames["assetCategories"] = "/api/v2/asset-categories";
            ApiEndpointNames["assetExtendedInfos"] = "/api/v2/asset-extended-infos";
            ApiEndpointNames["assetGroups"] = "/api/v2/asset-groups";

            var assetsFromDB = AssetManager.GetAllAsync();
            var AssetExtInfoFromDB = AssetExtendedInfosManager.GetAllAsync();
            var assetsAttrFromDB = AssetAttributesManager.GetAllAsync();
            var assetsCatsFromDB = AssetCategoryManager.GetAllAsync();
            var assetsGroupsFromDB = AssetGroupsManager.GetAllAsync();

            this.AllAssetsFromDB = (await assetsFromDB).Cast<AssetEntity>().ToList();
            this.TestAsset = EnumerableUtils.PickRandom(AllAssetsFromDB);
            this.TestAssetUpdate = await CreateTestAsset();
            this.TestAssetDelete = await CreateTestAsset(false);

            this.AllAssetExtendedInfosFromDB = (await AssetExtInfoFromDB).Cast<AssetExtendedInfosEntity>().ToList();
            this.TestAssetExtendedInfo = EnumerableUtils.PickRandom(AllAssetExtendedInfosFromDB);
            this.TestAssetExtendedInfoUpdate = await CreateTestAssetExtendedInfo();
            this.TestAssetExtendedInfoDelete = await CreateTestAssetExtendedInfo(false);

            this.AllAssetAttributesFromDB = (await assetsAttrFromDB).Cast<AssetAttributesEntity>().ToList();
            this.TestAssetAttribute = EnumerableUtils.PickRandom(AllAssetAttributesFromDB);
            this.TestAssetAttributeUpdate = await CreateTestAssetAttribute();
            this.TestAssetAttributeDelete = await CreateTestAssetAttribute(false);

            this.AllAssetCategoriesFromDB = (await assetsCatsFromDB).Cast<AssetCategoryEntity>().ToList();
            this.TestAssetCategory = EnumerableUtils.PickRandom(AllAssetCategoriesFromDB);
            this.TestAssetCategoryUpdate = await CreateTestAssetCategory();
            this.TestAssetCategoryDelete = await CreateTestAssetCategory(false);



            this.AllAssetGroupsFromDB = (await assetsGroupsFromDB).Cast<AssetGroupEntity>().ToList();
            this.TestAssetGroup = EnumerableUtils.PickRandom(AllAssetGroupsFromDB);

        }

        public void Dispose()
        {
            List<Task<bool>> deleteTasks = new List<Task<bool>>();

            foreach (string assetId in AssetsToDelete) { deleteTasks.Add(DeleteTestAsset(assetId)); }
            foreach (AssetAttributeIdentityDTO attrDTO in AssetAtributesToDelete) { deleteTasks.Add(DeleteTestAssetAttribute(attrDTO.AssetId, attrDTO.Key)); }
            foreach (string catId in AssetCategoriesToDelete) { deleteTasks.Add(DeleteTestAssetCategory(catId)); }
            foreach (string infoId in AssetExtendedInfosToDelete) { deleteTasks.Add(DeleteTestAssetExtendedInfo(infoId)); }

            Task.WhenAll(deleteTasks).Wait();
        }



        #region Create / Delete methods

        public async Task<AssetDTO> CreateTestAsset(bool deleteWithDispose = true)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AssetEntity, AssetDTO>();
                cfg.CreateMap<AssetDTO, AssetEntity>();
            });

            AssetDTO newAssetDTO = Mapper.Map<AssetDTO>(EnumerableUtils.PickRandom(AllAssetsFromDB));

            newAssetDTO.Id += "_AutoTest";
            newAssetDTO.Name += "_AutoTest";
            newAssetDTO.BlockChainAssetId += "_AutoTest";
            newAssetDTO.BlockChainId += "_AutoTest";

            string createUrl = ApiEndpointNames["assets"];
            string createParam = JsonUtils.SerializeObject(newAssetDTO);

            var response = await Consumer.ExecuteRequest(createUrl, emptyDict, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            if (deleteWithDispose)
            {
                AssetsToDelete.Add(newAssetDTO.Id);
            }

            //add (fake) entities so tests pass when checking against the list
            AssetEntity entity = Mapper.Map<AssetEntity>(newAssetDTO);
            entity.RowKey = newAssetDTO.Id;
            AllAssetsFromDB.Add(entity);

            return newAssetDTO;
        }

        public async Task<bool> DeleteTestAsset(string id)
        {
            string deleteUrl = ApiEndpointNames["assets"] + "/" + id;
            string deleteParam = JsonUtils.SerializeObject(new { id = id });

            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, emptyDict, deleteParam, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetAttributeIdentityDTO> CreateTestAssetAttribute(bool deleteWithDispose = true)
        {
            AssetAttributesEntity testAssetAttr = EnumerableUtils.PickRandom(AllAssetAttributesFromDB);

            string createUrl = ApiEndpointNames["assetAttributes"] + "/" + testAssetAttr.AssetId;
            string newKey = testAssetAttr.Key + "_AutoTest";
            string newValue = "autotest";

            AssetAttributeDTO createParameter = new AssetAttributeDTO() { Key = newKey, Value = newValue };

            var response = await Consumer.ExecuteRequest(createUrl, emptyDict, JsonUtils.SerializeObject(createParameter), Method.POST);
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

            if (deleteWithDispose)
            {
                this.AssetAtributesToDelete.Add(returnModel);
            }

            //add (fake) entities so tests pass when checking against the list
            AssetAttributesEntity attrEntity = new AssetAttributesEntity()
            {
                AssetId = testAssetAttr.AssetId,
                Key = newKey,
                Value = newValue
            };
            AllAssetAttributesFromDB.Add(attrEntity);

            return returnModel;
        }

        public async Task<bool> DeleteTestAssetAttribute(string assetId, string key)
        {
            string deleteUrl = ApiEndpointNames["assetAttributes"] + "/" + assetId + "/" + key;

            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }

        public async Task<AssetCategoryDTO> CreateTestAssetCategory(bool deleteWithDispose = true)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AssetCategoryEntity, AssetCategoryDTO>();
                cfg.CreateMap<AssetCategoryDTO, AssetCategoryEntity>();
            });

            string url = ApiEndpointNames["assetCategories"];

            AssetCategoryDTO newCategory = Mapper.Map<AssetCategoryDTO>(EnumerableUtils.PickRandom(AllAssetCategoriesFromDB));
            newCategory.Id += "_AutoTest";
            newCategory.Name += "_AutoTest";
            string createParam = JsonUtils.SerializeObject(newCategory);

            var response = await Consumer.ExecuteRequest(url, emptyDict, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            if (deleteWithDispose)
            {
                this.AssetCategoriesToDelete.Add(newCategory.Id);
            }

            //add (fake) entities so tests pass when checking against the list
            AssetCategoryEntity entity = Mapper.Map<AssetCategoryEntity>(newCategory);
            entity.RowKey = newCategory.Id;
            AllAssetCategoriesFromDB.Add(entity);

            return newCategory;
        }

        public async Task<bool> DeleteTestAssetCategory(string id)
        {
            string deleteUrl = ApiEndpointNames["assetCategories"] + "/" + id;
            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);

            if(deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        public async Task<AssetExtendedInfoDTO> CreateTestAssetExtendedInfo(bool deleteWithDispose = true)
        {
            string url = ApiEndpointNames["assetExtendedInfos"];

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AssetExtendedInfosEntity, AssetExtendedInfoDTO>();
                cfg.CreateMap<AssetExtendedInfoDTO, AssetExtendedInfosEntity>();
            });

            AssetExtendedInfoDTO newExtendedInfo = Mapper.Map<AssetExtendedInfoDTO>(EnumerableUtils.PickRandom(AllAssetExtendedInfosFromDB));
            newExtendedInfo.Id += "_AutoTest";
            newExtendedInfo.FullName += "_AutoTest";
            string createParam = JsonUtils.SerializeObject(newExtendedInfo);

            var response = await Consumer.ExecuteRequest(url, emptyDict, createParam, Method.POST);
            if(response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            if (deleteWithDispose)
            {
                this.AssetExtendedInfosToDelete.Add(newExtendedInfo.Id);
            }

            AssetExtendedInfosEntity entity = Mapper.Map<AssetExtendedInfosEntity>(newExtendedInfo);
            entity.RowKey = newExtendedInfo.Id;
            AllAssetExtendedInfosFromDB.Add(entity);
            //add (fake) entities so tests pass when checking against the list

            return newExtendedInfo;
        }

        public async Task<bool> DeleteTestAssetExtendedInfo(string id)
        {
            string deleteUrl = ApiEndpointNames["assetExtendedInfos"] + "/" + id;
            var deleteResponse = await Consumer.ExecuteRequest(deleteUrl, emptyDict, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
