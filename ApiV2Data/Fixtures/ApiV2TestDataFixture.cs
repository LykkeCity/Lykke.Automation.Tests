using ApiV2Data.DependencyInjection;
using Autofac;
using System;
using System.Linq;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Entities.ApiV2;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using ApiV2Data.DTOs;
using XUnitTestData.Repositories;
using AssetsData.DTOs.Assets;
using RestSharp;
using XUnitTestData.Domains.Authentication;
using XUnitTestData.Entities;
using NUnit.Framework;
using XUnitTestCommon.DTOs;
using XUnitTestCommon.GlobalActions;
using XUnitTestCommon.Tests;

namespace ApiV2Data.Fixtures
{
    [TestFixture]
    public partial class ApiV2TestDataFixture: BaseTest
    {
        private ConfigBuilder _configBuilder;
        private IContainer _container;

        public string TestClientId;

        public GenericRepository<WalletEntity, IWallet> WalletRepository;
        public List<WalletEntity> AllWalletsFromDb;
        public WalletDTO TestWallet;
        public WalletDTO TestWalletDelete;
        public AccountEntity TestWalletAccount;
        public string TestAssetId;
        public int AssetPrecission;
        public string TestWalletWithBalanceId;
        public WalletDTO TestWalletOperations;
        public WalletDTO TestWalletRegenerateKey;

        public GenericRepository<AccountEntity, IAccount> AccountRepository;

        public GenericRepository<OperationsEntity, IOperations> OperationsRepository;
        public List<OperationsEntity> AllOperationsFromDB;
        public OperationCreateReturnDTO TestOperation;
        public OperationCreateReturnDTO TestOperationCancel;

        public GenericRepository<OperationDetailsEntity, IOperationDetails> OperationDetailsRepository;
        public GenericRepository<PersonalDataEntity, IPersonalData> PersonalDataRepository;
        public OperationCreateReturnDTO TestOperationCreateDetails;
        public OperationCreateReturnDTO TestOperationRegisterDetails;

        public GenericRepository<TradersEntity, ITrader> TradersRepository;
        public ApiConsumer Consumer;
        public MatchingEngineConsumer MEConsumer;
        public ApiConsumer ClientInfoConsumer;

        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("ApiV2");

            ConfigBuilder MeConfig = new ConfigBuilder("MatchingEngine");
            if (Int32.TryParse(MeConfig.Config["Port"], out int port))
            {
                MEConsumer = new MatchingEngineConsumer(MeConfig.Config["BaseUrl"], port);
            }

            Consumer = new ApiConsumer(_configBuilder);

            PrepareDependencyContainer();
            PrepareTestData().Wait();
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApiV2TestModule(_configBuilder));
            _container = builder.Build();

            this.WalletRepository = RepositoryUtils.ResolveGenericRepository<WalletEntity, IWallet>(this._container);
            this.AccountRepository = RepositoryUtils.ResolveGenericRepository<AccountEntity, IAccount>(this._container);
            this.OperationsRepository = RepositoryUtils.ResolveGenericRepository<OperationsEntity, IOperations>(this._container);
            this.OperationDetailsRepository = RepositoryUtils.ResolveGenericRepository<OperationDetailsEntity, IOperationDetails>(this._container);
            this.PersonalDataRepository = RepositoryUtils.ResolveGenericRepository<PersonalDataEntity, IPersonalData>(this._container);
            this.TradersRepository = RepositoryUtils.ResolveGenericRepository<TradersEntity, ITrader>(this._container);
        }

        private async Task PrepareTestData()
        {
            var TestClient = await Consumer.RegisterNewUser();

            TestClientId = TestClient.Account.Id;
            var walletsFromDB = this.WalletRepository.GetAllAsync(w => w.ClientId == TestClientId && w.State != "deleted");
            var operationsFromDB = this.OperationsRepository.GetAllAsync(o => o.PartitionKey == OperationsEntity.GeneratePartitionKey() && o.ClientId.ToString() == TestClientId);

            this.TestAssetId = Constants.TestAssetId;
            this.AssetPrecission = 2;
            this.AllWalletsFromDb = (await walletsFromDB).Cast<WalletEntity>().ToList();
            this.TestWallet = await CreateTestWallet();

            //fill wallet with funds
            await MEConsumer.Client.UpdateBalanceAsync(Guid.NewGuid().ToString(), TestWallet.Id, Constants.TestAssetId, 50.0);
            this.TestWalletWithBalanceId = TestWallet.Id;


            this.TestWalletDelete = await CreateTestWallet();
            this.TestWalletAccount = await AccountRepository.TryGetAsync(TestWallet.Id) as AccountEntity;
            this.TestWalletOperations = await CreateTestWallet();
            this.TestWalletRegenerateKey = await CreateTestWallet(true);

            this.TestOperation = await CreateTestOperation();
            this.TestOperationCancel = await CreateTestOperation();

            this.TestOperationCreateDetails = await CreateTestOperation();
            this.TestOperationRegisterDetails = await CreateTestOperation();


            this.ClientInfoConsumer = new ApiConsumer(_configBuilder);
            await this.ClientInfoConsumer.RegisterNewUser();
            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(ClientInfoConsumer.ClientInfo.Account.Id));

            // set the id to the default one in case it has been changed by any test
            BaseAssetDTO body = new BaseAssetDTO(this.TestAssetId);
            var response = await Consumer.ExecuteRequest(ApiPaths.ASSETS_BASEASSET_PATH, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            GC.SuppressFinalize(this);
        }
    }
}
