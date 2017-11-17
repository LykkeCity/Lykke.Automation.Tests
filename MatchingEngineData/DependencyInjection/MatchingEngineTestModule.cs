using System;
using System.Collections.Generic;
using Autofac;
using System.Text;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Services;
using XUnitTestData.Repositories;
using XUnitTestCommon.Utils;
using AzureStorage.Tables;
using XUnitTestData.Entities.MatchingEngine;
using XUnitTestData.Domains.MatchingEngine;
using XUnitTestData.Domains.Assets;
using XUnitTestData.Entities;
using XUnitTestData.Entitites.ApiV2.Assets;

namespace MatchingEngineData.DependencyInjection
{
    class MatchingEngineTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public MatchingEngineTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new GenericRepository<AccountEntity, IAccount>(
                    new AzureTableStorage<AccountEntity>(
                        _configBuilder.Config["BalancesInfoConnectionString"], "Accounts", null), "ClientBalance"))
                .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new GenericRepository<CashSwapEntity, ICashSwap>(
                    new AzureTableStorage<CashSwapEntity>(
                        _configBuilder.Config["BalancesInfoConnectionString"], "SwapOperationsCash", null)))
                .As<IDictionaryRepository<ICashSwap>>();

            builder.Register(c =>
                new GenericRepository<AssetPairEntity, IAssetPair>(
                    new AzureTableStorage<AssetPairEntity>(_configBuilder.Config["DictionariesConnectionString"], "Dictionaries", null),
                    "AssetPair"
                )
            )
            .As<IDictionaryRepository<IAssetPair>>();

            RepositoryUtils.RegisterDictionaryManager<IAssetPair>(builder);

            builder.Register(c => new GenericRepository<MarketOrderEntity, IMarketOrderEntity>(
                    new AzureTableStorage<MarketOrderEntity>(
                        _configBuilder.Config["LimitOrdersConnectionString"], "MarketOrders", null), "OrderId"))
                .As<IDictionaryRepository<IMarketOrderEntity>>();

            builder.Register(c => new GenericRepository<LimitOrderEntity, ILimitOrderEntity>(
                new AzureTableStorage<LimitOrderEntity>(
                    _configBuilder.Config["LimitOrdersConnectionString"], "LimitOrders", null)))
                .As<IDictionaryRepository<ILimitOrderEntity>>();


        }
    }
}
