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

        }
    }
}
