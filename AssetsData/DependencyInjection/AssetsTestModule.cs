using Autofac;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.ApiV2;
using XUnitTestData.Entities.Assets;

namespace AssetsData.DependencyInjection
{
    class AssetsTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public AssetsTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => 
                new GenericRepository<AssetEntity, IAsset>(
                    new AzureTableStorage<AssetEntity>(_configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null),
                    "Asset"
                )
            )
            .As<IDictionaryRepository<IAsset>>();

            builder.Register(c => 
                new GenericRepository<AssetExtendedInfosEntity, IAssetExtendedInfo>(
                    new AzureTableStorage<AssetExtendedInfosEntity>(_configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null),
                    "aei"
                )
            )
            .As<IDictionaryRepository<IAssetExtendedInfo>>();

            builder.Register(c => 
                new GenericRepository<AssetCategoryEntity, IAssetCategory>(
                    new AzureTableStorage<AssetCategoryEntity>(_configBuilder.Config["DictionariesConnectionString"], "AssetCategories", null),
                    "AssetCategory"
                )
            )
            .As<IDictionaryRepository<IAssetCategory>>();

            builder.Register(c => 
                new AssetAttributesRepository(
                    new AzureTableStorage<AssetAttributesEntity>(_configBuilder.Config["DictionariesConnectionString"], "AssetAttributes", null)
                )
            )
            .As<IDictionaryRepository<IAssetAttributes>>();

            builder.Register(c => 
                new GenericRepository<AssetGroupEntity, IAssetGroup>(
                    new AzureTableStorage<AssetGroupEntity>(_configBuilder.Config["MainConnectionString"], "AssetGroups", null),
                    "AssetGroup"
                )
            )
            .As<IDictionaryRepository<IAssetGroup>>();

            builder.Register(c => 
                new GenericRepository<AssetPairEntity, IAssetPair>(
                    new AzureTableStorage<AssetPairEntity>(_configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null),
                    "AssetPair"
                )
            )
            .As<IDictionaryRepository<IAssetPair>>();

            builder.Register(c => 
                new GenericRepository<AssetSettingsEntity, IAssetSettings>(
                    new AzureTableStorage<AssetSettingsEntity>(_configBuilder.Config["DictionariesConnectionString"], "AssetSettings", null),
                    "Asset"
                )
            )
            .As<IDictionaryRepository<IAssetSettings>>();

            builder.Register(c => 
                new GenericRepository<AssetIssuersEntity, IAssetIssuers>(
                    new AzureTableStorage<AssetIssuersEntity>(_configBuilder.Config["DictionariesConnectionString"], "AssetIssuers", null),
                    "Issuer"
                )
            )
            .As<IDictionaryRepository<IAssetIssuers>>();

            builder.Register(c => 
                new GenericRepository<MarginAssetPairsEntity, IMarginAssetPairs>(
                    new AzureTableStorage<MarginAssetPairsEntity>(_configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null),
                    "MarginAssetPair"
                )
            )
            .As<IDictionaryRepository<IMarginAssetPairs>>();

            builder.Register(c => 
                new GenericRepository<MarginAssetEntity, IMarginAsset>(
                    new AzureTableStorage<MarginAssetEntity>(_configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null),
                    "MarginAsset"
                )
            )
            .As<IDictionaryRepository<IMarginAsset>>();

            builder.Register(c => 
                new GenericRepository<MarginIssuerEntity, IMarginIssuer>(
                    new AzureTableStorage<MarginIssuerEntity>(_configBuilder.Config["DictionariesConnectionString"], "AssetIssuers", null),
                    "MarginIssuer"
                )
            )
            .As<IDictionaryRepository<IMarginIssuer>>();

            builder.Register(c => 
                new WatchListRepository(
                    new AzureTableStorage<WatchListEntity>(_configBuilder.Config["DictionariesConnectionString"], "WatchLists", null)
                )
            )
            .As<IDictionaryRepository<IWatchList>>();

            builder.Register(c => new GenericRepository<Erc20TokenEntity, IErc20Token>(
                    new AzureTableStorage<Erc20TokenEntity>(
                        _configBuilder.Config["MainConnectionString"], "Erc20Tokens", null), "Erc20Token"))
                .As<IDictionaryRepository<IErc20Token>>();

            base.Load(builder);
        }
    }
}
