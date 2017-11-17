using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using BlueApiData.DependencyInjection;
using BlueApiData.DTOs;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestData.Domains;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Repositories.BlueApi;

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture : IDisposable
    {
        private readonly ConfigBuilder _configBuilder;
        private IContainer _container;
        public IMapper Mapper;

        public string TestClientId;
        public string AccountEmail;
        public string TwitterSearchQuery;
        public DateTime TwitterSearchUntilDate;
        public string TestPledgeCreateClientId;
        public string TestPledgeUpdateClientId;
        public string TestPledgeDeleteClientId;

        private Dictionary<string, string> _pledgesToDelete;

        public PledgesRepository PledgeRepository;
        public PledgeDTO TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;
        public ApiConsumer Consumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

        public BlueApiTestDataFixture()
        {
            _configBuilder = new ConfigBuilder("BlueApi");

            Consumer = new ApiConsumer(_configBuilder);
            Consumer.Authenticate();

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>
            {
                { "CreatePledge", new ApiConsumer(_configBuilder) },
                { "UpdatePledge", new ApiConsumer(_configBuilder) },
                { "DeletePledge", new ApiConsumer(_configBuilder) }
            };

            PledgeApiConsumers["CreatePledge"].Authenticate(ApiConsumerType.Create);
            PledgeApiConsumers["UpdatePledge"].Authenticate(ApiConsumerType.Update);
            PledgeApiConsumers["DeletePledge"].Authenticate(ApiConsumerType.Delete);

            PrepareDependencyContainer();
            PrepareTestData().Wait();
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BlueApiTestModule(_configBuilder));
            _container = builder.Build();

            PledgeRepository = (PledgesRepository)_container.Resolve<IDictionaryRepository<IPledgeEntity>>();
        }

        public void Dispose()
        {
            var deleteTasks = new List<Task<bool>>();

            foreach (KeyValuePair<string, string> pledgeData in _pledgesToDelete) { deleteTasks.Add(DeleteTestPledge(pledgeData.Value)); }

            Task.WhenAll(deleteTasks).Wait();
            GC.SuppressFinalize(this);
        }
    }
}
