using Autofac;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.AlgoStore;
using XUnitTestData.Repositories;
using XUnitTestData.Entities.AlgoStore;
using XUnitTestData.Entities;
using AzureStorage.Blob;
using XUnitTestData.Repositories.AlgoStore;
using System;

namespace AlgoStoreData.DependancyInjection
{
    class AlgoStoreTestModule : Module
    {
        private ConfigBuilder _configBuilder;
        private TimeSpan timespan = TimeSpan.FromSeconds(120);

        public AlgoStoreTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
                     
            builder.Register(c => new GenericRepository<MetaDataEntity, IMetaData>(
                    new AzureTableStorage<MetaDataEntity>(
                        _configBuilder.Config["MainConnectionString"], "AlgoMetaDataTable", null, timespan), "AlgoMetaDataTable"))
                .As<IDictionaryRepository<IMetaData>>();

            builder.Register(c => new GenericRepository<RuntimeDataEntity, IStatistics>(
                    new AzureTableStorage<RuntimeDataEntity>(
                        _configBuilder.Config["MainConnectionString"], "AlgoRuntimeDataTable", null, timespan), "AlgoRuntimeDataTable"))
                .As<IDictionaryRepository<IStatistics>>();

            builder.Register(c => new GenericRepository<ClientInstanceEntity, IClientInstance>(
                   new AzureTableStorage<ClientInstanceEntity>(
                       _configBuilder.Config["MainConnectionString"], "AlgoClientInstanceTable", null, timespan), "AlgoClientInstanceTable"))
               .As<IDictionaryRepository<IClientInstance>>();






            builder.Register(c => new GenericRepository<AlgoRatingsTableEntity, IAlgoRatingsTable>(
                   new AzureTableStorage<AlgoRatingsTableEntity>(
                       _configBuilder.Config["MainConnectionString"], "AlgoRatingsTable", null, timespan), "AlgoRatingsTable"))
               .As<IDictionaryRepository<IAlgoRatingsTable>>();

            builder.Register(c => new GenericRepository<AlgoStoreApiLogEntity, IAlgoStoreApiLog>(
                   new AzureTableStorage<AlgoStoreApiLogEntity>(
                       _configBuilder.Config["MainConnectionString"], "AlgoStoreApiLog", null, timespan), "AlgoStoreApiLog"))
               .As<IDictionaryRepository<IAlgoStoreApiLog>>();

            builder.Register(c => new GenericRepository<CSharpAlgoTemplateLogEntity, ICSharpAlgoTemplateLog>(
                  new AzureTableStorage<CSharpAlgoTemplateLogEntity>(
                      _configBuilder.Config["MainConnectionString"], "CSharpAlgoTemplateLog", null, timespan), "CSharpAlgoTemplateLog"))
              .As<IDictionaryRepository<ICSharpAlgoTemplateLog>>();

            builder.Register(c => new GenericRepository<CSharpAlgoTemplateUserLogEntity, ICSharpAlgoTemplateUserLog>(
                 new AzureTableStorage<CSharpAlgoTemplateUserLogEntity>(
                     _configBuilder.Config["MainConnectionString"], "CSharpAlgoTemplateUserLog", null, timespan), "CSharpAlgoTemplateUserLog"))
             .As<IDictionaryRepository<ICSharpAlgoTemplateUserLog>>();

            builder.Register(c => new GenericRepository<PublicsAlgosTableEntity, IPublicAlgosTable>(
                 new AzureTableStorage<PublicsAlgosTableEntity>(
                     _configBuilder.Config["MainConnectionString"], "PublicAlgosTable", null, timespan), "PublicAlgosTable"))
             .As<IDictionaryRepository<IPublicAlgosTable>>();

            builder.Register(c => new GenericRepository<StatisticsEntity, IStatisticss>(
                new AzureTableStorage<StatisticsEntity>(
                    _configBuilder.Config["MainConnectionString"], "Statistics", null, timespan), "Statistics"))
            .As<IDictionaryRepository<IStatisticss>>();











        }
    }
}
