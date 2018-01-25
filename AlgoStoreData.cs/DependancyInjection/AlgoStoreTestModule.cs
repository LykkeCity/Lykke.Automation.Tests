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

namespace AlgoStoreData.DependancyInjection
{
    class AlgoStoreTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public AlgoStoreTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            
            builder.Register(c => new GenericRepository<MetaDataEntity, IMetaData>(
                    new AzureTableStorage<MetaDataEntity>(
                        _configBuilder.Config["MainConnectionString"], "AlgoMetaDataTable", null), "AlgoMetaDataTable"))
                .As<IDictionaryRepository<IMetaData>>();

            builder.Register(c => new GenericRepository<RuntimeDataEntity, IRuntimeData>(
                    new AzureTableStorage<RuntimeDataEntity>(
                        _configBuilder.Config["MainConnectionString"], "AlgoRuntimeDataTable", null), "AlgoRuntimeDataTable"))
                .As<IDictionaryRepository<IRuntimeData>>();

            builder.Register(c => new GenericRepository<ClientInstanceEntity, IClientInstance>(
                   new AzureTableStorage<ClientInstanceEntity>(
                       _configBuilder.Config["MainConnectionString"], "AlgoClientInstanceTable", null), "AlgoClientInstanceTable"))
               .As<IDictionaryRepository<IClientInstance>>();

        }
    }
}
