using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.SettingsReader;
using XUnitTestCommon;
using XUnitTestCommon.Settings;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using XUnitTestData.Entities.Assets;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.ApiV2;

namespace AssetsData.DependencyInjection
{
    class AssetsTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public AssetsTestModule(ConfigBuilder configBuilder)
        {
            _configBuilder = configBuilder;
            _log = null;
        }

        public AssetsTestModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
            _log = null;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            builder.Register(c =>
                new GenericRepository<AssetEntity, IAsset>(
                    AzureTableStorage<AssetEntity>.Create(reloadingDbManager, "Dictionaries", _log), "Asset"))
                    .As<IDictionaryRepository<IAsset>>();

            builder.Register(c =>
                new GenericRepository<AssetExtendedInfosEntity, IAssetExtendedInfo>(
                    AzureTableStorage<AssetExtendedInfosEntity>.Create(reloadingDbManager, "Dictionaries", _log), "aei"))
                    .As<IDictionaryRepository<IAssetExtendedInfo>>();

            builder.Register(c =>
                new GenericRepository<AssetCategoryEntity, IAssetCategory>(
                    AzureTableStorage<AssetCategoryEntity>.Create(reloadingDbManager, "AssetCategories", _log), "AssetCategory"))
                    .As<IDictionaryRepository<IAssetCategory>>();

            builder.Register(c =>
                new AssetAttributesRepository(
                    AzureTableStorage<AssetAttributesEntity>.Create(reloadingDbManager, "AssetAttributes", _log), "AssetAttributes"))
                    .As<IDictionaryRepository<IAssetAttributes>>();

            builder.Register(c =>
                new GenericRepository<AssetGroupEntity, IAssetGroup>(
                    AzureTableStorage<AssetGroupEntity>.Create(reloadingDbManager, "AssetGroups", _log), "AssetGroup"))
                    .As<IDictionaryRepository<IAssetGroup>>();

            builder.Register(c =>
                new GenericRepository<AssetPairEntity, IAssetPair>(
                    AzureTableStorage<AssetPairEntity>.Create(reloadingDbManager, "Dictionaries", _log), "AssetPair"))
                    .As<IDictionaryRepository<IAssetPair>>();

            builder.Register(c =>
                new GenericRepository<AssetSettingsEntity, IAssetSettings>(
                    AzureTableStorage<AssetSettingsEntity>.Create(reloadingDbManager, "AssetSettings", _log), "Asset"))
                    .As<IDictionaryRepository<IAssetSettings>>();

            builder.Register(c =>
                new GenericRepository<AssetIssuersEntity, IAssetIssuers>(
                    AzureTableStorage<AssetIssuersEntity>.Create(reloadingDbManager, "AssetIssuers", _log), "Issuer"))
                    .As<IDictionaryRepository<IAssetIssuers>>();

            builder.Register(c =>
                new GenericRepository<MarginAssetPairsEntity, IMarginAssetPairs>(
                    AzureTableStorage<MarginAssetPairsEntity>.Create(reloadingDbManager, "Dictionaries", _log), "MarginAssetPair"))
                    .As<IDictionaryRepository<IMarginAssetPairs>>();

            builder.Register(c =>
                new GenericRepository<MarginAssetEntity, IMarginAsset>(
                    AzureTableStorage<MarginAssetEntity>.Create(reloadingDbManager, "Dictionaries", _log), "MarginAsset"))
                    .As<IDictionaryRepository<IMarginAsset>>();

            builder.Register(c =>
                new GenericRepository<MarginIssuerEntity, IMarginIssuer>(
                    AzureTableStorage<MarginIssuerEntity>.Create(reloadingDbManager, "AssetIssuers", _log), "MarginIssuer"))
                    .As<IDictionaryRepository<IMarginIssuer>>();

            builder.Register(c =>
                new WatchListRepository(
                    AzureTableStorage<WatchListEntity>.Create(reloadingDbManager, "WatchLists", _log)))
                    .As<IDictionaryRepository<IWatchList>>();

            base.Load(builder);
        }
    }
}
