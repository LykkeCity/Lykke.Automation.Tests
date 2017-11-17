using ApiV2Data.DependencyInjection;
using Autofac;
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
using XUnitTestData.Repositories;
using AssetsData.DTOs.Assets;
using RestSharp;
using XUnitTestData.Domains.Authentication;

namespace ApiV2Data.Fixtures
{
    public partial class ApiV2TestDataFixture
    {
        private ConfigBuilder _configBuilder;
        private IContainer _container;

        private List<string> WalletsToDelete;
        private List<string> OperationsToCancel;
        private List<string> _walletsToDelete;

        public string TestClientId;

        public WalletRepository WalletRepository;
        public List<WalletEntity> AllWalletsFromDb;
        public WalletEntity TestWallet;
        public WalletDTO TestWalletDelete;
        public AccountEntity TestWalletAccount;
        public string TestAssetId;
        public string TestWalletWithBalance;
        public WalletDTO TestWalletOperations;
        public WalletDTO TestWalletRegenerateKey;

        public IDictionaryManager<IAccount> AccountManager;

        public OperationsRepository OperationsRepository;
        public List<OperationsEntity> AllOperationsFromDB;
        public OperationCreateReturnDTO TestOperation;
        public OperationCreateReturnDTO TestOperationCancel;

        public OperationDetailsRepository OperationDetailsRepository;
        public PersonalDataRepository PersonalDataRepository;
        public OperationCreateReturnDTO TestOperationCreateDetails;
        public OperationCreateReturnDTO TestOperationRegisterDetails;

        public TradersRepository TradersRepository;
        public ApiConsumer Consumer;

        public ApiV2TestDataFixture()
        {
            _configBuilder = new ConfigBuilder("ApiV2");

            var oAuthConsumer = new OAuthConsumer
            {
                AuthTokenTimeout = Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]),
                AuthPath = _configBuilder.Config["AuthPath"],
                BaseAuthUrl = _configBuilder.Config["BaseUrlAuth"],
                AuthUser = new User
                {
                    ClientInfo = _configBuilder.Config["AuthClientInfo"],
                    Email = _configBuilder.Config["AuthEmail"],
                    PartnerId = _configBuilder.Config["AuthPartnerId"],
                    Password = _configBuilder.Config["AuthPassword"]
                }
            };

            Consumer = new ApiConsumer(_configBuilder, oAuthConsumer);

            PrepareDependencyContainer();
            PrepareTestData().Wait();
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApiV2TestModule(_configBuilder));
            _container = builder.Build();

            this.WalletRepository = (WalletRepository)this._container.Resolve<IDictionaryRepository<IWallet>>();
            this.AccountManager = RepositoryUtils.PrepareRepositoryManager<IAccount>(this._container);
            this.OperationsRepository = (OperationsRepository)this._container.Resolve<IDictionaryRepository<IOperations>>();
            this.OperationDetailsRepository = (OperationDetailsRepository)this._container.Resolve<IDictionaryRepository<IOperationDetails>>();
            this.PersonalDataRepository = (PersonalDataRepository)this._container.Resolve<IDictionaryRepository<IPersonalData>>();
            this.TradersRepository = (TradersRepository)this._container.Resolve<IDictionaryRepository<ITrader>>();
        }

        private async Task PrepareTestData()
        {        

            WalletsToDelete = new List<string>();
            OperationsToCancel = new List<string>();            

            _walletsToDelete = new List<string>();
            TestClientId = this._configBuilder.Config["AuthClientId"];
            var walletsFromDB = this.WalletRepository.GetAllAsync(TestClientId);
            var operationsFromDB = this.OperationsRepository.GetAllAsync(TestClientId);

            TestClientId = _configBuilder.Config["AuthClientId"];
            var walletsFromDb = WalletRepository.GetAllAsync(TestClientId);

            this.AllWalletsFromDb = (await walletsFromDb).Cast<WalletEntity>().ToList();
            this.TestWallet = AllWalletsFromDb.Where(w => w.Id == Constants.TestWalletId).FirstOrDefault(); //TODO hardcoded
            this.TestAssetId = Constants.TestAssetId;
            this.TestWalletWithBalance = Constants.TestWalletId;
            this.AllWalletsFromDb = (await walletsFromDB).Cast<WalletEntity>().ToList();
            this.TestWallet = AllWalletsFromDb.Where(w => w.Id == TestWalletWithBalance).FirstOrDefault(); //TODO hardcoded
            this.TestWalletDelete = await CreateTestWallet();
            this.TestWalletAccount = await AccountManager.TryGetAsync(TestWallet.Id) as AccountEntity;
            this.TestWalletOperations = await CreateTestWallet();
            this.TestWalletRegenerateKey = await CreateTestWallet(true);

            this.TestOperation = await CreateTestOperation();
            this.TestOperationCancel = await CreateTestOperation();

            this.TestOperationCreateDetails = await CreateTestOperation();
            this.TestOperationRegisterDetails = await CreateTestOperation();

            // set the id to the default one in case it has been changed by any test
            BaseAssetDTO body = new BaseAssetDTO(this.TestAssetId);
            await Consumer.ExecuteRequest(ApiPaths.ASSETS_BASEASSET_PATH, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
        }

        public void Dispose()
        {
            var deleteTasks = new List<Task<bool>>();

            foreach (string walletId in WalletsToDelete) { deleteTasks.Add(DeleteTestWallet(walletId)); }

            foreach (string operationId in OperationsToCancel) { deleteTasks.Add(CancelTestOperation(operationId)); }

            foreach (string walletId in _walletsToDelete) { deleteTasks.Add(DeleteTestWallet(walletId)); }

            Task.WhenAll(deleteTasks).Wait();
            GC.SuppressFinalize(this);
        }
    }
}
