using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using BlueApiData.DependencyInjection;
using BlueApiData.DTOs;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestData.Domains.Authentication;
using XUnitTestData.Domains.BlueApi;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories;
using XUnitTestData.Entities.BlueApi;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Entities.ApiV2;
using NUnit.Framework;
using XUnitTestCommon.Tests;
using XUnitTestCommon.DTOs;
using XUnitTestCommon.GlobalActions;

namespace BlueApiData.Fixtures
{
    [TestFixture]
    public partial class BlueApiTestDataFixture : BaseTest
    {
        private ConfigBuilder _configBuilder;
        private ConfigBuilder _clientAccountConfigBuilder;
        private IContainer _container;
        public IMapper Mapper;

        public string AccountEmail;
        public string TwitterSearchQuery;
        public DateTime TwitterSearchUntilDate;

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
        public MatchingEngineConsumer MEConsumer;
        public ClientRegisterDTO ClientInfoInstance;
        public ApiConsumer ClientInfoConsumer;
        public ApiConsumer ClientAccountConsumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

        public ApiConsumer InvitationLinkRequestConsumer;
        public List<ApiConsumer> InvitationLinkClaimersConsumers;

        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("BlueApi");
            _clientAccountConfigBuilder = new ConfigBuilder("ClientAccount");

            PrepareDependencyContainer();
            PrepareApiConsumers().Wait();
            PrepareMapper();
        }

        private async Task PrepareApiConsumers()
        {
            Consumer = new ApiConsumer(_configBuilder);

            ConfigBuilder MeConfig = new ConfigBuilder("MatchingEngine");
            if (Int32.TryParse(MeConfig.Config["Port"], out int port))
            {
                MEConsumer = new MatchingEngineConsumer(MeConfig.Config["BaseUrl"], port);
            }

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            ClientAccountConsumer = new ApiConsumer(_clientAccountConfigBuilder);
            
        }

        public async Task CreatePledgeClientAndApiConsumer(string purpose)
        {
            ApiConsumer consumer = new ApiConsumer(_configBuilder);
            await consumer.RegisterNewUser();
            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.Account.Id));
            PledgeApiConsumers.Add(purpose, consumer);
        }

        private async Task<List<ApiConsumer>> RegisterNUsers(int n)
        {
            List<ApiConsumer> result = new List<ApiConsumer>();

            for (int i = 0; i < n; i++)
            {
                ApiConsumer consumer = new ApiConsumer(_configBuilder);
                await consumer.RegisterNewUser();
                AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.Account.Id));
                result.Add(consumer);
            }

            return result;
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BlueApiTestModule(_configBuilder));
            _container = builder.Build();

            PledgeRepository = RepositoryUtils.ResolveGenericRepository<PledgeEntity, IPledgeEntity>(_container);
            PersonalDataRepository = RepositoryUtils.ResolveGenericRepository<PersonalDataEntity, IPersonalData>(_container);
            ReferralLinkRepository = RepositoryUtils.ResolveGenericRepository<ReferralLinkEntity, IReferralLink>(_container);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            
        }

        public async Task CreateLykkeBluePartnerClientAndApiConsumer()
        {
            var consumer = new ApiConsumer(_configBuilder);

            await consumer.RegisterNewUser(
                new ClientRegisterDTO
                {
                    Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                    FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                    ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                    Password = Helpers.RandomString(10),
                    Hint = Helpers.RandomString(3),
                    PartnerId = _configBuilder.Config["LykkeBluePartnerId"] // "Lykke.blue"
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
                    PartnerId = _configBuilder.Config["TestPartnerId"] //  "NewTestPartner"
                }
            );

            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(ClientAccountConsumer.ClientInfo.Account.Id));
        }
    }
}
