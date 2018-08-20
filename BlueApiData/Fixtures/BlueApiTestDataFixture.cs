using Autofac;
using AutoMapper;
using BlueApiData.DependencyInjection;
using BlueApiData.DTOs;
using Lykke.Service.Balances.Client;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs;
using XUnitTestCommon.GlobalActions;
using XUnitTestCommon.Settings;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Entities.ApiV2;
using XUnitTestData.Entities.BlueApi;
using XUnitTestData.Repositories;

namespace BlueApiData.Fixtures
{
    [TestFixture]
    public partial class BlueApiTestDataFixture : BaseTest
    {
        private AppSettings _appSettings;
        private BlueApiSettings _blueApiAppSettings;
        private ClientAccountSettings _clientAccountAppSettings;
        private IContainer _container;
        public IMapper Mapper;

        public string AccountEmail;
        public bool TwitterAggressiveCheck;

        public GenericRepository<PledgeEntity, IPledgeEntity> PledgeRepository;
        public GenericRepository<PersonalDataEntity, IPersonalData> PersonalDataRepository;
        public GenericRepository<ReferralLinkEntity, IReferralLink> ReferralLinkRepository;

        public string TestPledgeCreateClientId;
        public PledgeDTO TestPledge;
        public string TestPledgeUpdateClientId;
        public PledgeDTO TestPledgeUpdate;
        public string TestPledgeDeleteClientId;
        public PledgeDTO TestPledgeDelete;
        
        public ApiConsumer Consumer;
        public BalancesClient BalancesClient;
        
        public ClientRegisterDTO ClientInfoInstance;
        public ApiConsumer ClientInfoConsumer;
        public ApiConsumer ClientAccountConsumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

        public ApiConsumer GlobalConsumer;
        public ApiConsumer InvitationLinkRequestConsumer;
        public List<ApiConsumer> InvitationLinkClaimersConsumers;
        public RequestInvitationLinkResponseDto TestInvitationLink;
        public ClientRegisterResponseDTO TestInvitationLinkUserData;

        public ApiConsumer GiftCoinLinkRequestConsumer;
        public List<ApiConsumer> GiftCoinLinkClaimConsumers;
        public RequestGiftCoinsLinkResponseDto TestGiftCoinLink;

        [OneTimeSetUp]
        public void Initialize()
        {
            _appSettings = new ConfigBuilder().ReloadingManager.CurrentValue;
            _blueApiAppSettings = _appSettings.AutomatedFunctionalTests.BlueApi;
            _clientAccountAppSettings = _appSettings.AutomatedFunctionalTests.ClientAccount;

            PrepareDependencyContainer();
            PrepareApiConsumers().Wait();
            PrepareMapper();
            PrepareGlobalTestData().Wait();
        }

        private async Task PrepareApiConsumers()
        {
            Consumer = new ApiConsumer(_blueApiAppSettings);

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            this.BalancesClient = new BalancesClient(_blueApiAppSettings.BalancesServiceUrl, null);
            ClientAccountConsumer = new ApiConsumer(_clientAccountAppSettings);
            
        }

        public async Task CreatePledgeClientAndApiConsumer(string purpose)
        {
            ApiConsumer consumer = new ApiConsumer(_blueApiAppSettings);
            await consumer.RegisterNewUser();
            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.Account.Id));
            PledgeApiConsumers.Add(purpose, consumer);
        }

        private async Task<List<ApiConsumer>> RegisterNUsers(int n)
        {
            List<ApiConsumer> result = new List<ApiConsumer>();

            for (int i = 0; i < n; i++)
            {
                ApiConsumer consumer = new ApiConsumer(_blueApiAppSettings);
                await consumer.RegisterNewUser();
                AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.Account.Id));
                result.Add(consumer);
            }

            return result;
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BlueApiTestModule(_appSettings));
            _container = builder.Build();
            
            PledgeRepository = RepositoryUtils.ResolveGenericRepository<PledgeEntity, IPledgeEntity>(this._container);
            PersonalDataRepository = RepositoryUtils.ResolveGenericRepository<PersonalDataEntity, IPersonalData>(this._container);
            ReferralLinkRepository = RepositoryUtils.ResolveGenericRepository<ReferralLinkEntity, IReferralLink>(this._container);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            
        }

        public async Task CreateLykkeBluePartnerClientAndApiConsumer()
        {
            var consumer = new ApiConsumer(_blueApiAppSettings);

            await consumer.RegisterNewUser(
                new ClientRegisterDTO
                {
                    Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                    FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                    ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                    Password = Helpers.RandomString(10),
                    Hint = Helpers.RandomString(3),
                    PartnerId = _blueApiAppSettings.LykkeBluePartnerId // "Lykke.blue"
                }
            );

            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.Account.Id));
        }

        public async Task CreateTestPartnerClient()
        {
            await ClientAccountConsumer.RegisterNewUser(
                new ClientRegisterDTO
                {
                    Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                    FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                    ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                    Password = Helpers.RandomString(10),
                    Hint = Helpers.RandomString(3),
                    PartnerId = _blueApiAppSettings.TestPartnerId //  "NewTestPartner"
                }
            );

            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(ClientAccountConsumer.ClientInfo.Account.Id));
        }
    }
}
