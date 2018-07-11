using AlgoStoreData.DependancyInjection;
using AlgoStoreData.DTOs;
using AlgoStoreData.DTOs.InstanceData;
using AlgoStoreData.DTOs.InstanceData.Builders;
using Autofac;
using Lykke.SettingsReader;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.AlgoStore;
using XUnitTestData.Entities.AlgoStore;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.AlgoStore;

namespace AlgoStoreData.Fixtures
{
    [TestFixture]
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        private ConfigBuilder _configBuilder;
        private TimeSpan timespan = TimeSpan.FromSeconds(120);
        public ApiConsumer Consumer;
        private OAuthConsumer User;
        public ServicesSettings BaseUrl;
        private IContainer _container;
        public GenericRepository<MetaDataEntity, IMetaData> MetaDataRepository;
        public GenericRepository<AlgoEntity, IAlgo> AlgoRepository;
        public GenericRepository<RuntimeDataEntity, IStatistics> RuntimeDataRepository;
        public GenericRepository<ClientInstanceEntity, IClientInstance> ClientInstanceRepository;
        public GenericRepository<AlgoRatingsTableEntity, IAlgoRatingsTable> AlgoRatingsRepository;
        public GenericRepository<AlgoStoreApiLogEntity, IAlgoStoreApiLog> AlgoApiLogRepository;
        public GenericRepository<CSharpAlgoTemplateLogEntity, ICSharpAlgoTemplateLog> CSharpAlgoTemplateLogRepository;
        public GenericRepository<CSharpAlgoTemplateUserLogEntity, ICSharpAlgoTemplateUserLog> CSharpAlgoTemplateUserLogRepository;
        public GenericRepository<PublicsAlgosTableEntity, IPublicAlgosTable> PublicAlgosRepository;
        public GenericRepository<StatisticsEntity, IStatisticss> StatisticsRepository;
        public GenericRepository<AlgoInstanceStatisticsEntity, IAlgoInstanceStatistics> AlgoInstanceStaticsticsRepository;
        public GenericRepository<AlgoInstanceTradesEntity, IAlgoInstanceTrades> AlgoInstanceTradesRepository;
        public List<BuilInitialDataObjectDTO> PreStoredMetadata;
        public AlgoBlobRepository BlobRepository;
        protected InstanceDataDTO postInstanceData;
        protected AlgoDataDTO algoData;

        protected List<AlgoDataDTO> algosList;
        protected List<InstanceDataDTO> instancesList;

        [OneTimeSetUp]
        public void Initialize()
        {
            algosList = new List<AlgoDataDTO>();
            instancesList = new List<InstanceDataDTO>();

            _configBuilder = new ConfigBuilder();

            User = new OAuthConsumer(_configBuilder.ReloadingManager.CurrentValue.AutomatedFunctionalTests.AlgoStore);
            Consumer = new ApiConsumer(_configBuilder.ReloadingManager.CurrentValue.AutomatedFunctionalTests.AlgoStore, User);
            BaseUrl = _configBuilder.ReloadingManager.CurrentValue.AutomatedFunctionalTests.Services;

            PrepareDependencyContainer();
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AlgoStoreTestModule(_configBuilder.ReloadingManager));
            _container = builder.Build();

            var reloadingDbManager = _configBuilder.ReloadingManager.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            MetaDataRepository = RepositoryUtils.ResolveGenericRepository<MetaDataEntity, IMetaData>(_container);
            AlgoRepository = RepositoryUtils.ResolveGenericRepository<AlgoEntity, IAlgo>(_container);
            RuntimeDataRepository = RepositoryUtils.ResolveGenericRepository<RuntimeDataEntity, IStatistics>(_container);
            ClientInstanceRepository = RepositoryUtils.ResolveGenericRepository<ClientInstanceEntity, IClientInstance>(_container);
            AlgoRatingsRepository = RepositoryUtils.ResolveGenericRepository<AlgoRatingsTableEntity, IAlgoRatingsTable>(_container);
            AlgoApiLogRepository = RepositoryUtils.ResolveGenericRepository<AlgoStoreApiLogEntity, IAlgoStoreApiLog>(_container);
            CSharpAlgoTemplateLogRepository = RepositoryUtils.ResolveGenericRepository<CSharpAlgoTemplateLogEntity, ICSharpAlgoTemplateLog>(_container);
            CSharpAlgoTemplateUserLogRepository = RepositoryUtils.ResolveGenericRepository<CSharpAlgoTemplateUserLogEntity, ICSharpAlgoTemplateUserLog>(_container);
            PublicAlgosRepository = RepositoryUtils.ResolveGenericRepository<PublicsAlgosTableEntity, IPublicAlgosTable>(_container);
            StatisticsRepository = RepositoryUtils.ResolveGenericRepository<StatisticsEntity, IStatisticss>(_container);
            BlobRepository = new AlgoBlobRepository(reloadingDbManager, timespan);
            AlgoInstanceStaticsticsRepository = RepositoryUtils.ResolveGenericRepository<AlgoInstanceStatisticsEntity, IAlgoInstanceStatistics>(_container);
            AlgoInstanceTradesRepository = RepositoryUtils.ResolveGenericRepository<AlgoInstanceTradesEntity, IAlgoInstanceTrades>(_container);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            GC.SuppressFinalize(this);
            ClearTestData().Wait();
        }
    }
}
