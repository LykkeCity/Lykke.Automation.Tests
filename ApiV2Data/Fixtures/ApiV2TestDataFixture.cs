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
using XUnitTestData.Repositories;

namespace ApiV2Data.Fixtures
{
    public partial class ApiV2TestDataFixture : IDisposable
    {
        private ConfigBuilder _configBuilder;
        private IContainer container;

        private Dictionary<string, string> PledgesToDelete;
        private List<string> WalletsToDelete;
        private List<string> OperationsToCancel;

        public string TestClientId;

        public PledgesRepository PledgeRepository;
        public PledgeDTO TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;

        public WalletRepository WalletRepository;
        public List<WalletEntity> AllWalletsFromDB;
        public WalletEntity TestWallet;
        public WalletDTO TestWalletDelete;
        public AccountEntity TestWalletAccount;
        public string TestAssetId;
        public string TestWalletWithBalance;
        public WalletDTO TestWalletOperations;
        public IDictionaryManager<IAccount> AccountManager;

        public OperationsRepository OperationsRepository;
        public List<OperationsEntity> AllOperationsFromDB;
        public OperationCreateReturnDTO TestOperation;
        public OperationCreateReturnDTO TestOperationCancel;
        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;


        public ApiV2TestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("ApiV2");
            this.Consumer = new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]));
            this.Consumer.Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["AuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));

            //PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            //PledgeApiConsumers.Add("CreatePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));
            //PledgeApiConsumers.Add("UpdatePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));
            //PledgeApiConsumers.Add("DeletePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));

            //PledgeApiConsumers["CreatePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeCreateAuthEmail"],
            //    _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));
            //PledgeApiConsumers["UpdatePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeUpdateAuthEmail"],
            //    _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));
            //PledgeApiConsumers["DeletePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeDeleteAuthEmail"],
            //    _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));

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
            this.AccountManager = RepositoryUtils.PrepareRepositoryManager<IAccount>(this.container);
            this.OperationsRepository = (OperationsRepository)this.container.Resolve<IDictionaryRepository<IOperations>>();
        }

        private async Task prepareTestData()
        {
            ApiEndpointNames = new Dictionary<string, string>();
            //ApiEndpointNames["Pledges"] = "/api/pledges";
            ApiEndpointNames["Wallets"] = "/api/wallets";
            ApiEndpointNames["Operations"] = "/api/operations";

            //PledgesToDelete = new Dictionary<string, string>();
            WalletsToDelete = new List<string>();
            OperationsToCancel = new List<string>();

            TestClientId = this._configBuilder.Config["AuthClientId"];
            var walletsFromDB = this.WalletRepository.GetAllAsync(TestClientId);
            var operationsFromDB = this.OperationsRepository.GetAllAsync(TestClientId);

            //this.TestPledge = await CreateTestPledge();
            //this.TestPledgeUpdate = await CreateTestPledge("UpdatePledge");
            //this.TestPledgeDelete = await CreateTestPledge("DeletePledge");

            this.TestAssetId = "LKK";
            this.TestWalletWithBalance = "fd0f7373-301e-42c0-83a2-1d7b691676c3";
            this.AllWalletsFromDB = (await walletsFromDB).Cast<WalletEntity>().ToList();
            this.TestWallet = AllWalletsFromDB.Where(w => w.Id == TestWalletWithBalance).FirstOrDefault(); //TODO hardcoded
            this.TestWalletDelete = await CreateTestWallet();
            this.TestWalletAccount = await AccountManager.TryGetAsync(TestWallet.Id) as AccountEntity;
            this.TestWalletOperations = await CreateTestWallet();

            this.TestOperation = await CreateTestOperation();
            this.TestOperationCancel = await CreateTestOperation();

            //this.AllOperationsFromDB = (await operationsFromDB).Cast<OperationsEntity>().ToList();

        }

        public void Dispose()
        {
            List<Task<bool>> deleteTasks = new List<Task<bool>>();
            //foreach (KeyValuePair<string, string> pledgeData in PledgesToDelete) { deleteTasks.Add(DeleteTestPledge(pledgeData.Key, pledgeData.Value)); }
            foreach (string walletId in WalletsToDelete) { deleteTasks.Add(DeleteTestWallet(walletId)); }
            foreach (string operationId in OperationsToCancel) { deleteTasks.Add(CancelTestOperation(operationId)); }

            Task.WhenAll(deleteTasks).Wait();
        }
    }
}
