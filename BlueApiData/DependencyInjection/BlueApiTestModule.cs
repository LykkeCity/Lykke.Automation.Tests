using Autofac;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Entities;
using XUnitTestData.Entities.BlueApi;
using XUnitTestData.Repositories;

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
            builder.Register(c => new GenericRepository<PledgeEntity, IPledgeEntity>(
                    new AzureTableStorage<PledgeEntity>(
                        _configBuilder.Config["MainConnectionString"], "Pledges", null), "Pledge"))
                .As<IDictionaryRepository<IPledgeEntity>>();

            builder.Register(c => new GenericRepository<AccountEntity, IAccount>(
                    new AzureTableStorage<AccountEntity>(
                        _configBuilder.Config["MainConnectionString"], "Accounts", null), "ClientBalance"))
                .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new GenericRepository<ReferralLinkEntity, IReferralLink>(
                    new AzureTableStorage<ReferralLinkEntity>(
                        _configBuilder.Config["MainConnectionString"], "ReferralLinks", null), "ReferralLink"))
                .As<IDictionaryRepository<IReferralLink>>();
        }
    }
}
