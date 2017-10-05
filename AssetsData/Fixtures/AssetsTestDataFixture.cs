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

namespace FirstXUnitTest.Fixtures
{
    public class AssetsTestDataFixture : IDisposable
    {
        public List<AssetEntity> AllAssetsFromDB;
        public AssetEntity TestAsset;

        public List<AssetDescriptionEntity> AllAssetDescriptionsFromDB;
        public AssetDescriptionEntity TestAssetDescription;

        public List<AssetAttributesEntity> AllAssetAttributesFromDB;
        public AssetAttributesEntity TestAssetAttribute;

        public string TestAttributeKey;

        public IDictionaryManager<IAsset> AssetManager;
        public IDictionaryManager<IAssetDescription> AssetDescriptionManager;
        public IDictionaryManager<IAssetCategory> AssetCategoryManager;
        public IDictionaryManager<IAssetAttributes> AssetAttributesManager;

        public ApiConsumer Consumer;

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

            this.AssetManager = prepareRepositoryManager<IAsset>();
            this.AssetDescriptionManager = prepareRepositoryManager<IAssetDescription>();
            this.AssetCategoryManager = prepareRepositoryManager<IAssetCategory>();
            this.AssetAttributesManager = prepareRepositoryManager<IAssetAttributes>();
        }

        private IDictionaryManager<T> prepareRepositoryManager<T>() where T : IDictionaryItem
        {
            var repository = this.container.Resolve<IDictionaryRepository<T>>();
            var cacheService = this.container.Resolve<IDictionaryCacheService<T>>();
            var dateTimeProvider = new DateTimeProvider();


            var manager = this.container.Resolve<IDictionaryManager<T>>(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IDictionaryRepository<T>) && pi.Name == "repository",
                    (pi, ctx) => repository),

                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IDictionaryCacheService<T>) && pi.Name == "cache",
                    (pi, ctx) => cacheService),

                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IDateTimeProvider) && pi.Name == "dateTimeProvider",
                    (pi, ctx) => dateTimeProvider)
                    );

            return manager;
        }

        private void prepareTestData()
        {
            var assetsFromDB = Task.Run(async () =>
            {
                return await AssetManager.GetAllAsync();
            }).Result;

            this.AllAssetsFromDB = assetsFromDB.Cast<AssetEntity>().ToList();

            this.TestAsset = PickRandom(AllAssetsFromDB);

            var assetsDescsFromDB = Task.Run(async () =>
            {
                return await AssetDescriptionManager.GetAllAsync();
            }).Result;

            this.AllAssetDescriptionsFromDB = assetsDescsFromDB.Cast<AssetDescriptionEntity>().ToList();
            this.TestAssetDescription = PickRandom(AllAssetDescriptionsFromDB);

            var assetsAttrFromDB = Task.Run(async () =>
            {
                return await AssetAttributesManager.GetAllAsync();
            }).Result;

            this.AllAssetAttributesFromDB = assetsAttrFromDB.Cast<AssetAttributesEntity>().ToList();
            this.TestAssetAttribute = PickRandom(AllAssetAttributesFromDB);


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
