using Autofac;
using Autofac.Core;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using FirstXUnitTest.DependencyInjection;
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

namespace FirstXUnitTest.Fixtures
{
    public class AssetsTestDataFixture : IDisposable
    {
        public List<AssetEntity> AllAssetsFromDB;
        public AssetEntity TestAsset;

        public List<AssetExtendedInfosEntity> AllAssetExtendedInfosFromDB;
        public AssetExtendedInfosEntity TestAssetExtendedInfo;

        public List<AssetAttributesEntity> AllAssetAttributesFromDB;
        public AssetAttributesEntity TestAssetAttribute;

        public List<AssetCategoryEntity> AllAssetCategoriesFromDB;
        public AssetCategoryEntity TestAssetCategory;

        public string TestAttributeKey;

        public IDictionaryManager<IAsset> AssetManager;
        public IDictionaryManager<IAssetExtendedInfo> AssetExtendedInfosManager;
        public IDictionaryManager<IAssetCategory> AssetCategoryManager;
        public IDictionaryManager<IAssetAttributes> AssetAttributesManager;

        public AssetAttributesRepository AssetAttributesRepository;

        public ApiConsumer Consumer;
        public Dictionary<string, string> ApiEndpointNames;

        private Dictionary<string, string> emptyDict = new Dictionary<string, string>();

        private IContainer container;

        private ConfigBuilder _configBuilder;

        public AssetsTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("Assets");
            this.Consumer = new ApiConsumer(this._configBuilder);

            prepareDependencyContainer();
            prepareTestData();
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
            this.AssetAttributesRepository = (AssetAttributesRepository)this.container.Resolve<IDictionaryRepository<IAssetAttributes>>();
        }

        private void prepareTestData()
        {
            this.ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["assets"] = "/api/v2/assets";
            ApiEndpointNames["assetAttributes"] = "/api/v2/asset-attributes";
            ApiEndpointNames["assetCategories"] = "/api/v2/asset-categories";
            ApiEndpointNames["assetExtendedInfos"] = "/api/v2/asset-extended-infos";

            var assetsFromDB = Task.Run(async () =>
            {
                return await AssetManager.GetAllAsync();
            }).Result;

            this.AllAssetsFromDB = assetsFromDB.Cast<AssetEntity>().ToList();

            this.TestAsset = PickRandom(AllAssetsFromDB);

            var assetsDescsFromDB = Task.Run(async () =>
            {
                return await AssetExtendedInfosManager.GetAllAsync();
            }).Result;

            this.AllAssetExtendedInfosFromDB = assetsDescsFromDB.Cast<AssetExtendedInfosEntity>().ToList();
            this.TestAssetExtendedInfo = PickRandom(AllAssetExtendedInfosFromDB);

            var assetsAttrFromDB = Task.Run(async () =>
            {
                return await AssetAttributesManager.GetAllAsync();
            }).Result;

            this.AllAssetAttributesFromDB = assetsAttrFromDB.Cast<AssetAttributesEntity>().ToList();
            this.TestAssetAttribute = PickRandom(AllAssetAttributesFromDB);

            var assetsCatsFromDB = Task.Run(async () =>
            {
                return await AssetCategoryManager.GetAllAsync();
            }).Result;
            
            this.AllAssetCategoriesFromDB = assetsCatsFromDB.Cast<AssetCategoryEntity>().ToList();
            this.TestAssetCategory = PickRandom(AllAssetCategoriesFromDB);

            this.TestAttributeKey = "metadata";
        }

        private T PickRandom<T>(List<T> model)
        {
            Random rnd = new Random();
            int randomInt = rnd.Next(model.Count);
            return model[randomInt];
        }

        public void Dispose()
        {

        }
    }
}
