using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
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

        public string TestClientId;

        private Dictionary<string, string> _pledgesToDelete;

        public PledgesRepository PledgeRepository;
        public PledgeDTO TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;

        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

        public BlueApiTestDataFixture()
        {
            _configBuilder = new ConfigBuilder("BlueApi");

            Consumer = new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]));
            Consumer.Authenticate(
                _configBuilder.Config["BaseUrlAuth"], 
                _configBuilder.Config["AuthPath"], 
                _configBuilder.Config["AuthEmail"],
                _configBuilder.Config["AuthPassword"], 
                _configBuilder.Config["AuthClientInfo"], 
                _configBuilder.Config["AuthPartnerId"], 
                Int32.Parse(_configBuilder.Config["AuthTokenTimeout"])
            );

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            PledgeApiConsumers.Add(
                "CreatePledge", 
                new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]))
            );
            PledgeApiConsumers.Add(
                "UpdatePledge", 
                new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]))
            );
            PledgeApiConsumers.Add(
                "DeletePledge", 
                new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]))
            );

            PledgeApiConsumers["CreatePledge"].Authenticate(
                _configBuilder.Config["BaseUrlAuth"], 
                _configBuilder.Config["AuthPath"], 
                _configBuilder.Config["PledgeCreateAuthEmail"],
                _configBuilder.Config["AuthPassword"], 
                _configBuilder.Config["AuthClientInfo"], 
                _configBuilder.Config["AuthPartnerId"], 
                Int32.Parse(_configBuilder.Config["AuthTokenTimeout"])
            );
            PledgeApiConsumers["UpdatePledge"].Authenticate(
                _configBuilder.Config["BaseUrlAuth"], 
                _configBuilder.Config["AuthPath"], 
                _configBuilder.Config["PledgeUpdateAuthEmail"],
                _configBuilder.Config["AuthPassword"], 
                _configBuilder.Config["AuthClientInfo"], 
                _configBuilder.Config["AuthPartnerId"], 
                Int32.Parse(_configBuilder.Config["AuthTokenTimeout"])
            );
            PledgeApiConsumers["DeletePledge"].Authenticate(
                _configBuilder.Config["BaseUrlAuth"], 
                _configBuilder.Config["AuthPath"], 
                _configBuilder.Config["PledgeDeleteAuthEmail"],
                _configBuilder.Config["AuthPassword"], 
                _configBuilder.Config["AuthClientInfo"], 
                _configBuilder.Config["AuthPartnerId"], 
                Int32.Parse(_configBuilder.Config["AuthTokenTimeout"])
            );

            PrepareDependencyContainer();
            PrepareTestData().Wait();
        }

        private async Task PrepareTestData()
        {
            ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["Pledges"] = "/api/pledges";

            _pledgesToDelete = new Dictionary<string, string>();

            TestClientId = _configBuilder.Config["AuthClientId"];

            TestPledge = await CreateTestPledge();
            TestPledgeUpdate = await CreateTestPledge("UpdatePledge");
            TestPledgeDelete = await CreateTestPledge("DeletePledge");
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

            foreach (KeyValuePair<string, string> pledgeData in _pledgesToDelete) { deleteTasks.Add(DeleteTestPledge(pledgeData.Key, pledgeData.Value)); }

            Task.WhenAll(deleteTasks).Wait();
            GC.SuppressFinalize(this);
        }
    }
}
