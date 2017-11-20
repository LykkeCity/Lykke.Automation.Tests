using Autofac;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Repositories;
using XUnitTestData.Entities.ApiV2;
using XUnitTestData.Entities;

namespace ApiV2Data.DependencyInjection
{
    public class ApiV2TestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public ApiV2TestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new GenericRepository<WalletEntity, IWallet>(
                    new AzureTableStorage<WalletEntity>(
                        _configBuilder.Config["MainConnectionString"], "Wallets", null), "Wallet"))
                .As<IDictionaryRepository<IWallet>>();

            builder.Register(c => new GenericRepository<AccountEntity, IAccount>(
                    new AzureTableStorage<AccountEntity>(
                        _configBuilder.Config["MainConnectionString"], "Accounts", null), "ClientBalance"))
                .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new GenericRepository<OperationsEntity, IOperations>(
                    new AzureTableStorage<OperationsEntity>(
                        _configBuilder.Config["MainConnectionString"], "Operations", null), "Operations"))
                .As<IDictionaryRepository<IOperations>>();

            builder.Register(c => new GenericRepository<OperationDetailsEntity, IOperationDetails>(
                    new AzureTableStorage<OperationDetailsEntity>(
                        _configBuilder.Config["MainConnectionString"], "OperationDetailsInformation", null)))
                .As<IDictionaryRepository<IOperationDetails>>();

            builder.Register(c => new GenericRepository<PersonalDataEntity, IPersonalData>(
                    new AzureTableStorage<PersonalDataEntity>(
                        _configBuilder.Config["MainConnectionString"], "PersonalData", null), "PD"))
                .As<IDictionaryRepository<IPersonalData>>();

            builder.Register(c => new GenericRepository<TradersEntity, ITrader>(
                    new AzureTableStorage<TradersEntity>(
                        _configBuilder.Config["MainConnectionString"], "Traders", null)))
                .As<IDictionaryRepository<ITrader>>();
        }
    }
}
