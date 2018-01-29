using AlgoStoreData.DependancyInjection;
using AlgoStoreData.DTOs;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
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
        public ApiConsumer Consumer;
        private OAuthConsumer User;
        private IContainer _container;
        public GenericRepository<MetaDataEntity, IMetaData> MetaDataRepository;
        public GenericRepository<RuntimeDataEntity, IRuntimeData> RuntimeDataRepository;
        public GenericRepository<ClientInstanceEntity, IClientInstance> ClientInstanceRepository;
        public List<MetaDataResponseDTO> PreStoredMetadata;
        public AlgoBlobRepository BlobRepository;


        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("AlgoStore");

            User = new OAuthConsumer(_configBuilder);
            Consumer = new ApiConsumer(_configBuilder, User);

            PrepareDependencyContainer();
            PrepareTestData().Wait();
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AlgoStoreTestModule(_configBuilder));
            _container = builder.Build();

            this.MetaDataRepository = RepositoryUtils.ResolveGenericRepository<MetaDataEntity, IMetaData>(this._container);
            this.RuntimeDataRepository = RepositoryUtils.ResolveGenericRepository<RuntimeDataEntity, IRuntimeData>(this._container);
            this.ClientInstanceRepository = RepositoryUtils.ResolveGenericRepository<ClientInstanceEntity, IClientInstance>(this._container);
            this.BlobRepository = new AlgoBlobRepository(_configBuilder.Config["MainConnectionString"]);
    }

        private async Task PrepareTestData()
        {
            PreStoredMetadata = await UploadSomeBaseMetaData(15);
            DataManager.storeMetadata(PreStoredMetadata);
        }

        private async Task ClearTestData()
        {
            await ClearAllCascadeDelete(DataManager.getAllMetaData());
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            ClearTestData().Wait();
            GC.SuppressFinalize(this);
        }
    }
}
