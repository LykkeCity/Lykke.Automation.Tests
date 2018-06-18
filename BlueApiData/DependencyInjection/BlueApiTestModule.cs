using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.SettingsReader;
using XUnitTestCommon;
using XUnitTestCommon.Settings;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Entities;
using XUnitTestData.Entities.ApiV2;
using XUnitTestData.Entities.BlueApi;
using XUnitTestData.Repositories;

namespace BlueApiData.DependencyInjection
{
    public class BlueApiTestModule : Module
    {
        private readonly AppSettings _appSettings;

        private readonly IReloadingManager<AppSettings> _settings;

        public BlueApiTestModule(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public BlueApiTestModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var reloadingDbManager = _settings.ConnectionString(x => x.AlgoApi.Db.TableStorageConnectionString);

            builder.Register(c => new GenericRepository<PledgeEntity, IPledgeEntity>(
                    AzureTableStorage<PledgeEntity>.Create(reloadingDbManager, "Pledges", null), "Pledge"))
                .As<IDictionaryRepository<IPledgeEntity>>();

            builder.Register(c => new GenericRepository<AccountEntity, IAccount>(
                    AzureTableStorage<AccountEntity>.Create(reloadingDbManager, "Accounts", null), "ClientBalance"))
                .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new GenericRepository<PersonalDataEntity, IPersonalData>(
                    AzureTableStorage<PersonalDataEntity>.Create(reloadingDbManager, "PersonalData", null), "PD"))
               .As<IDictionaryRepository<IPersonalData>>();

            builder.Register(c => new GenericRepository<ReferralLinkEntity, IReferralLink>(
                    AzureTableStorage<ReferralLinkEntity>.Create(reloadingDbManager, "ReferralLinks", null), "ReferallLink"))
                .As<IDictionaryRepository<IReferralLink>>();
        }
    }
}
