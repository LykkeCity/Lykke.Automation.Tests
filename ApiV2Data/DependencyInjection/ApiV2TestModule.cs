using Autofac;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Repositories;
using XUnitTestData.Entities.ApiV2;
using XUnitTestData.Entities;
using Lykke.SettingsReader;
using XUnitTestCommon.Settings;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;

namespace ApiV2Data.DependencyInjection
{
    public class ApiV2TestModule : Module
    {
        private ApiV2Settings _apiV2Settings;

        private readonly IReloadingManager<AppSettings> _settings;

        public ApiV2TestModule(ApiV2Settings apiV2Settings)
        {
            _apiV2Settings = apiV2Settings;
        }

        public ApiV2TestModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            builder.Register(c => new GenericRepository<WalletEntity, IWallet>(
                    AzureTableStorage<WalletEntity>.Create(reloadingDbManager, "Wallets", null), "Wallet"))
                    .As<IDictionaryRepository<IWallet>>();

            builder.Register(c => new GenericRepository<AccountEntity, IAccount>(
                    AzureTableStorage<AccountEntity>.Create(reloadingDbManager, "Accounts", null), "ClientBalance"))
                    .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new GenericRepository<OperationsEntity, IOperations>(
                    AzureTableStorage<OperationsEntity>.Create(reloadingDbManager, "Operations", null), "Operations"))
                    .As<IDictionaryRepository<IOperations>>();

            builder.Register(c => new GenericRepository<OperationDetailsEntity, IOperationDetails>(
                    AzureTableStorage<OperationDetailsEntity>.Create(reloadingDbManager, "OperationDetailsInformation", null)))
                    .As<IDictionaryRepository<IOperationDetails>>();

            builder.Register(c => new GenericRepository<PersonalDataEntity, IPersonalData>(
                    AzureTableStorage<PersonalDataEntity>.Create(reloadingDbManager, "PersonalData", null), "PD"))
                    .As<IDictionaryRepository<IPersonalData>>();

            builder.Register(c => new GenericRepository<TradersEntity, ITrader>(
                    AzureTableStorage<TradersEntity>.Create(reloadingDbManager, "Traders", null)))
                    .As<IDictionaryRepository<ITrader>>();
        }
    }
}
