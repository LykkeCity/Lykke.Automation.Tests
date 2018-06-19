using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.SettingsReader;
using System;
using XUnitTestCommon;
using XUnitTestCommon.Settings;
using XUnitTestData.Domains;
using XUnitTestData.Domains.AlgoStore;
using XUnitTestData.Entities.AlgoStore;
using XUnitTestData.Repositories;

namespace AlgoStoreData.DependancyInjection
{
    class AlgoStoreTestModule : Module
    {
        private ConfigBuilder _configBuilder;
        private TimeSpan timespan = TimeSpan.FromSeconds(120);

        private readonly IReloadingManager<AppSettings> _settings;

        public AlgoStoreTestModule(ConfigBuilder configBuilder)
        {
            _configBuilder = configBuilder;
        }

        public AlgoStoreTestModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            builder.Register(c => new GenericRepository<AlgoEntity, IAlgo>(
                    AzureTableStorage<AlgoEntity>.Create(reloadingDbManager, "Algos", null, timespan)))
                .As<IDictionaryRepository<IAlgo>>();

            builder.Register(c => new GenericRepository<MetaDataEntity, IMetaData>(
                    AzureTableStorage<MetaDataEntity>.Create(reloadingDbManager, "AlgoMetaDataTable", null, timespan)))
                .As<IDictionaryRepository<IMetaData>>();

            builder.Register(c => new GenericRepository<RuntimeDataEntity, IStatistics>(
                    AzureTableStorage<RuntimeDataEntity>.Create(reloadingDbManager, "AlgoRuntimeDataTable", null, timespan)))
                .As<IDictionaryRepository<IStatistics>>();

            builder.Register(c => new GenericRepository<ClientInstanceEntity, IClientInstance>(
                   AzureTableStorage<ClientInstanceEntity>.Create(reloadingDbManager, "AlgoClientInstanceTable", null, timespan)))
               .As<IDictionaryRepository<IClientInstance>>();

            builder.Register(c => new GenericRepository<AlgoRatingsTableEntity, IAlgoRatingsTable>(
                   AzureTableStorage<AlgoRatingsTableEntity>.Create(reloadingDbManager, "AlgoRatingsTable", null, timespan)))
               .As<IDictionaryRepository<IAlgoRatingsTable>>();

            builder.Register(c => new GenericRepository<AlgoStoreApiLogEntity, IAlgoStoreApiLog>(
                   AzureTableStorage<AlgoStoreApiLogEntity>.Create(reloadingDbManager, "AlgoStoreApiLog", null, timespan)))
               .As<IDictionaryRepository<IAlgoStoreApiLog>>();

            builder.Register(c => new GenericRepository<CSharpAlgoTemplateLogEntity, ICSharpAlgoTemplateLog>(
                  AzureTableStorage<CSharpAlgoTemplateLogEntity>.Create(reloadingDbManager, "CSharpAlgoTemplateLog", null, timespan)))
              .As<IDictionaryRepository<ICSharpAlgoTemplateLog>>();

            builder.Register(c => new GenericRepository<CSharpAlgoTemplateUserLogEntity, ICSharpAlgoTemplateUserLog>(
                 AzureTableStorage<CSharpAlgoTemplateUserLogEntity>.Create(reloadingDbManager, "CSharpAlgoTemplateUserLog", null, timespan)))
             .As<IDictionaryRepository<ICSharpAlgoTemplateUserLog>>();

            builder.Register(c => new GenericRepository<PublicsAlgosTableEntity, IPublicAlgosTable>(
                 AzureTableStorage<PublicsAlgosTableEntity>.Create(reloadingDbManager, "PublicAlgosTable", null, timespan)))
             .As<IDictionaryRepository<IPublicAlgosTable>>();

            builder.Register(c => new GenericRepository<StatisticsEntity, IStatisticss>(
                AzureTableStorage<StatisticsEntity>.Create(reloadingDbManager, "Statistics", null, timespan)))
            .As<IDictionaryRepository<IStatisticss>>();

            builder.Register(c => new GenericRepository<AlgoInstanceStatisticsEntity, IAlgoInstanceStatistics>(
                AzureTableStorage<AlgoInstanceStatisticsEntity>.Create(reloadingDbManager, "AlgoInstanceStatistics", null, timespan)))
            .As<IDictionaryRepository<IAlgoInstanceStatistics>>();
        }
    }
}
