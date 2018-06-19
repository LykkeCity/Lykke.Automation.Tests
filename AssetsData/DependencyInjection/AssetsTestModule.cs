using Autofac;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.ApiV2;
using XUnitTestData.Entities.Assets;
using Lykke.SettingsReader;
using XUnitTestCommon.Settings;

namespace AssetsData.DependencyInjection
{
    class AssetsTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        private readonly IReloadingManager<AppSettings> _settings;

        public AssetsTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        public AssetsTestModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            builder.Register(c =>
                new GenericRepository<AssetEntity, IAsset>(
                    AzureTableStorage<AssetEntity>.Create(reloadingDbManager, "Dictionaries", null), "Asset"))
                    .As<IDictionaryRepository<IAsset>>();

            builder.Register(c =>
                new GenericRepository<AssetExtendedInfosEntity, IAssetExtendedInfo>(
                    AzureTableStorage<AssetExtendedInfosEntity>.Create(reloadingDbManager, "Dictionaries", null), "aei"))
                    .As<IDictionaryRepository<IAssetExtendedInfo>>();

            builder.Register(c =>
                new GenericRepository<AssetCategoryEntity, IAssetCategory>(
                    AzureTableStorage<AssetCategoryEntity>.Create(reloadingDbManager, "AssetCategories", null), "AssetCategory"))
                    .As<IDictionaryRepository<IAssetCategory>>();

            builder.Register(c =>
                new AssetAttributesRepository(
                    AzureTableStorage<AssetAttributesEntity>.Create(reloadingDbManager, "AssetAttributes", null), "AssetAttributes"))
                    .As<IDictionaryRepository<IAssetAttributes>>();

            builder.Register(c =>
                new GenericRepository<AssetGroupEntity, IAssetGroup>(
                    AzureTableStorage<AssetGroupEntity>.Create(reloadingDbManager, "AssetGroups", null), "AssetGroup"))
                    .As<IDictionaryRepository<IAssetGroup>>();

            builder.Register(c =>
                new GenericRepository<AssetPairEntity, IAssetPair>(
                    AzureTableStorage<AssetPairEntity>.Create(reloadingDbManager, "Dictionaries", null), "AssetPair"))
                    .As<IDictionaryRepository<IAssetPair>>();

            builder.Register(c =>
                new GenericRepository<AssetSettingsEntity, IAssetSettings>(
                    AzureTableStorage<AssetSettingsEntity>.Create(reloadingDbManager, "AssetSettings", null), "Asset"))
                    .As<IDictionaryRepository<IAssetSettings>>();

            builder.Register(c =>
                new GenericRepository<AssetIssuersEntity, IAssetIssuers>(
                    AzureTableStorage<AssetIssuersEntity>.Create(reloadingDbManager, "AssetIssuers", null), "Issuer"))
                    .As<IDictionaryRepository<IAssetIssuers>>();

            builder.Register(c =>
                new GenericRepository<MarginAssetPairsEntity, IMarginAssetPairs>(
                    AzureTableStorage<MarginAssetPairsEntity>.Create(reloadingDbManager, "Dictionaries", null), "MarginAssetPair"))
                    .As<IDictionaryRepository<IMarginAssetPairs>>();

            builder.Register(c =>
                new GenericRepository<MarginAssetEntity, IMarginAsset>(
                    AzureTableStorage<MarginAssetEntity>.Create(reloadingDbManager, "Dictionaries", null), "MarginAsset"))
                    .As<IDictionaryRepository<IMarginAsset>>();

            builder.Register(c =>
                new GenericRepository<MarginIssuerEntity, IMarginIssuer>(
                    AzureTableStorage<MarginIssuerEntity>.Create(reloadingDbManager, "AssetIssuers", null), "MarginIssuer"))
                    .As<IDictionaryRepository<IMarginIssuer>>();

            builder.Register(c =>
                new WatchListRepository(
                    AzureTableStorage<WatchListEntity>.Create(reloadingDbManager, "WatchLists", null)))
                    .As<IDictionaryRepository<IWatchList>>();

            base.Load(builder);
        }
    }
}
