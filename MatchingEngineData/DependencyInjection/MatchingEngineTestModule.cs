using Autofac;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Repositories;
using XUnitTestCommon.Utils;
using AzureStorage.Tables;
using XUnitTestData.Entities.MatchingEngine;
using XUnitTestData.Domains.MatchingEngine;
using XUnitTestData.Domains.Assets;
using XUnitTestData.Entities;
using XUnitTestData.Entities.Assets;
using Lykke.SettingsReader;
using XUnitTestCommon.Settings;

namespace MatchingEngineData.DependencyInjection
{
    class MatchingEngineTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        private readonly IReloadingManager<AppSettings> _settings;

        public MatchingEngineTestModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            builder.Register(c => new GenericRepository<AccountEntity, IAccount>(
                    AzureTableStorage<AccountEntity>.Create(reloadingDbManager, "Accounts", null), "ClientBalance"))
                    .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new GenericRepository<CashSwapEntity, ICashSwap>(
                    AzureTableStorage<CashSwapEntity>.Create(reloadingDbManager, "SwapOperationsCash", null)))
                    .As<IDictionaryRepository<ICashSwap>>();

            builder.Register(c =>
                new GenericRepository<AssetPairEntity, IAssetPair>(
                    AzureTableStorage<AssetPairEntity>.Create(reloadingDbManager, "Dictionaries", null), "AssetPair"))
                    .As<IDictionaryRepository<IAssetPair>>();

            builder.Register(c => new GenericRepository<MarketOrderEntity, IMarketOrderEntity>(
                    AzureTableStorage<MarketOrderEntity>.Create(reloadingDbManager, "MarketOrders", null), "OrderId"))
                    .As<IDictionaryRepository<IMarketOrderEntity>>();

            builder.Register(c => new GenericRepository<LimitOrderEntity, ILimitOrderEntity>(
                    AzureTableStorage<LimitOrderEntity>.Create(reloadingDbManager, "LimitOrders", null), "LimitOrders"))
                    .As<IDictionaryRepository<ILimitOrderEntity>>();
        }
    }
}
