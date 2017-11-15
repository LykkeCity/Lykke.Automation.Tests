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

namespace ApiV2Data.Fixtures
{
    public partial class ApiV2TestDataFixture
    {
        private ConfigBuilder _configBuilder;
        private IContainer _container;

        private List<string> _walletsToDelete;

        public string TestClientId;

        public WalletRepository WalletRepository;
        public List<WalletEntity> AllWalletsFromDb;
        public WalletEntity TestWallet;
        public WalletDTO TestWalletDelete;
        public AccountEntity TestWalletAccount;
        public string TestWalletAssetId;

        public IDictionaryManager<IAccount> AccountManager;

        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;

        public ApiV2TestDataFixture()
        {
            _configBuilder = new ConfigBuilder("ApiV2");
            Consumer = new ApiConsumer(_configBuilder);
            Consumer.Authenticate();

            PrepareDependencyContainer();
            PrepareTestData().Wait();
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApiV2TestModule(_configBuilder));
            _container = builder.Build();

            WalletRepository = (WalletRepository)_container.Resolve<IDictionaryRepository<IWallet>>();
            AccountManager = RepositoryUtils.PrepareRepositoryManager<IAccount>(_container);
        }

        private async Task PrepareTestData()
        {
            ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["Wallets"] = "/api/wallets";
            ApiEndpointNames["Assets"] = "/api/assets";
            ApiEndpointNames["AssetsBaseAsset"] = ApiEndpointNames["Assets"] + "/baseAsset";            
            ApiEndpointNames["TransactionHistory"] = "/api/transactionHistory";

            _walletsToDelete = new List<string>();

            TestClientId = _configBuilder.Config["AuthClientId"];
            var walletsFromDb = WalletRepository.GetAllAsync(TestClientId);

            this.AllWalletsFromDb = (await walletsFromDb).Cast<WalletEntity>().ToList();
            this.TestWallet = AllWalletsFromDb.Where(w => w.Id == "fd0f7373-301e-42c0-83a2-1d7b691676c3").FirstOrDefault(); //TODO hardcoded
            this.TestWalletDelete = await CreateTestWallet();
            this.TestWalletAccount = await AccountManager.TryGetAsync(TestWallet.Id) as AccountEntity;
            this.TestWalletAssetId = "LKK";

            // set the id to the default one in case it has been changed by any test
            BaseAssetDTO body = new BaseAssetDTO(this.TestWalletAssetId);
            await Consumer.ExecuteRequest(ApiEndpointNames["AssetsBaseAsset"], Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
        }

        public void Dispose()
        {
            var deleteTasks = new List<Task<bool>>();

            foreach (string walletId in _walletsToDelete) { deleteTasks.Add(DeleteTestWallet(walletId)); }

            Task.WhenAll(deleteTasks).Wait();
            GC.SuppressFinalize(this);
        }
    }
}
