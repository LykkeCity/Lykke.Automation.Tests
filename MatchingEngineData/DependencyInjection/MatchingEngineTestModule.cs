using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.SettingsReader;
using XUnitTestCommon;
using XUnitTestCommon.Settings;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using XUnitTestData.Domains.MatchingEngine;
using XUnitTestData.Entities;
using XUnitTestData.Entities.Assets;
using XUnitTestData.Entities.MatchingEngine;
using XUnitTestData.Repositories;

namespace MatchingEngineData.DependencyInjection
{
    class MatchingEngineTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public MatchingEngineTestModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
            _log = null;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            builder.Register(c => new GenericRepository<AccountEntity, IAccount>(
                    AzureTableStorage<AccountEntity>.Create(reloadingDbManager, "Accounts", _log), "ClientBalance"))
                    .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new GenericRepository<CashSwapEntity, ICashSwap>(
                    AzureTableStorage<CashSwapEntity>.Create(reloadingDbManager, "SwapOperationsCash", _log)))
                    .As<IDictionaryRepository<ICashSwap>>();

            builder.Register(c =>
                new GenericRepository<AssetPairEntity, IAssetPair>(
                    AzureTableStorage<AssetPairEntity>.Create(reloadingDbManager, "Dictionaries", _log), "AssetPair"))
                    .As<IDictionaryRepository<IAssetPair>>();

            builder.Register(c => new GenericRepository<MarketOrderEntity, IMarketOrderEntity>(
                    AzureTableStorage<MarketOrderEntity>.Create(reloadingDbManager, "MarketOrders", _log), "OrderId"))
                    .As<IDictionaryRepository<IMarketOrderEntity>>();

            builder.Register(c => new GenericRepository<LimitOrderEntity, ILimitOrderEntity>(
                    AzureTableStorage<LimitOrderEntity>.Create(reloadingDbManager, "LimitOrders", _log), "LimitOrders"))
                    .As<IDictionaryRepository<ILimitOrderEntity>>();
        }
    }
}
