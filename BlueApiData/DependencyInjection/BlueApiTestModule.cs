using Autofac;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.BlueApi;

namespace BlueApiData.DependencyInjection
{
    public class BlueApiTestModule : Module
    {
        private readonly ConfigBuilder _configBuilder;

        public BlueApiTestModule(ConfigBuilder configBuilder)
        {
            _configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new PledgesRepository(
                    new AzureTableStorage<PledgeEntity>(
                        _configBuilder.Config["MainConnectionString"], "Pledges", null)))
                .As<IDictionaryRepository<IPledgeEntity>>();

            RepositoryUtils.RegisterDictionaryManager<IPledgeEntity>(builder);

            builder.Register(c => new AccountRepository(
                    new AzureTableStorage<AccountEntity>(
                        _configBuilder.Config["MainConnectionString"], "Accounts", null)))
                .As<IDictionaryRepository<IAccount>>();

            RepositoryUtils.RegisterDictionaryManager<IAccount>(builder);
        }
    }
}
