using ApiV2Data.DependencyInjection;
using Autofac;
using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Repositories.ApiV2;
using XUnitTestData.Services;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using ApiV2Data.DTOs;
using RestSharp;
using System.Net;

namespace ApiV2Data.Fixtures
{
    public partial class ApiV2TestDataFixture : IDisposable
    {
        private ConfigBuilder _configBuilder;
        private IContainer container;

        private Dictionary<string, string> PledgesToDelete;

        public string TestClientId;

        public PledgesRepository PledgeRepository;
        public PledgeDTO TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;

        public WalletRepository WalletRepository;
        public List<WalletEntity> AllWalletsFromDB;
        public WalletEntity TestWallet;

        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;


        public ApiV2TestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("ApiV2");
            this.Consumer = new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]));
            this.Consumer.Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["AuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            PledgeApiConsumers.Add("CreatePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));
            PledgeApiConsumers.Add("UpdatePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));
            PledgeApiConsumers.Add("DeletePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));

            PledgeApiConsumers["CreatePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeCreateAuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));
            PledgeApiConsumers["UpdatePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeUpdateAuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));
            PledgeApiConsumers["DeletePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeDeleteAuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));

            prepareDependencyContainer();
            prepareTestData().Wait();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApiV2TestModule(_configBuilder));
            this.container = builder.Build();

            this.PledgeRepository = (PledgesRepository)this.container.Resolve<IDictionaryRepository<IPledgeEntity>>();
            this.WalletRepository = (WalletRepository)this.container.Resolve<IDictionaryRepository<IWallet>>();
        }

        private async Task prepareTestData()
        {
            ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["Pledges"] = "/api/pledges";
            ApiEndpointNames["Wallets"] = "/api/wallets";

            PledgesToDelete = new Dictionary<string, string>();

            TestClientId = this._configBuilder.Config["AuthClientId"];
            var walletsFromDB = this.WalletRepository.GetAllAsync(TestClientId);

            this.TestPledge = await CreateTestPledge();
            this.TestPledgeUpdate = await CreateTestPledge("UpdatePledge");
            this.TestPledgeDelete = await CreateTestPledge("DeletePledge");

            this.AllWalletsFromDB = (await walletsFromDB).Cast<WalletEntity>().ToList();
            this.TestWallet = EnumerableUtils.PickRandom(AllWalletsFromDB);

        }

        public void Dispose()
        {
            List<Task<bool>> deleteTasks = new List<Task<bool>>();
            foreach (KeyValuePair<string, string> pledgeData in PledgesToDelete) { deleteTasks.Add(DeleteTestPledge(pledgeData.Key, pledgeData.Value)); }

            Task.WhenAll(deleteTasks).Wait();
        }
    }
}
