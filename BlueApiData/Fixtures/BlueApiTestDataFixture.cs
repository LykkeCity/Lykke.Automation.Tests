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
    public partial class BlueApiTestDataFixture: BaseTest
    {
        private ConfigBuilder _configBuilder;
        private ConfigBuilder _clientAccountConfigBuilder;
        private IContainer _container;
        public IMapper Mapper;

        public string TestClientId;
        public string AccountEmail;
        public string TwitterSearchQuery;
        public DateTime TwitterSearchUntilDate;
        public Dictionary<string, string> TestPledgeClientIDs;

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
        public ClientRegisterDTO ClientInfoInstance;
        public ApiConsumer ClientInfoConsumer;
        public ApiConsumer ClientAccountConsumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

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
            var oAuthConsumer = new OAuthConsumer(_configBuilder);

            Consumer = new ApiConsumer(_configBuilder, oAuthConsumer);

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            TestPledgeClientIDs = new Dictionary<string, string>();

            var createPledgesTasks = new List<Task>()
            {
                CreatePledgeClientAndApiConsumer("GetPledge"),
                CreatePledgeClientAndApiConsumer("CreatePledge"),
                CreatePledgeClientAndApiConsumer("UpdatePledge"),
                CreatePledgeClientAndApiConsumer("DeletePledge"),
            };

            await Task.WhenAll(createPledgesTasks);
        }

        private async Task CreatePledgeClientAndApiConsumer(string purpose)
        {
<<<<<<< HEAD
            var oAuthConsumer = new OAuthConsumer(_configBuilder);
            var createPledgeUser = await oAuthConsumer.RegisterNewUser();
            var consumer = new ApiConsumer(_configBuilder, oAuthConsumer);

            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.ClientId));
=======
            ApiConsumer consumer = new ApiConsumer(_configBuilder);
            await consumer.RegisterNewUser();
            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.Account.Id));
>>>>>>> 06617bf6829d2307c9050e174fa1ab237e39489d

            TestPledgeClientIDs[purpose] = consumer.ClientInfo.Account.Id;
            PledgeApiConsumers.Add(purpose, consumer);
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
            var clientInfoAuth = new OAuthConsumer(_configBuilder);

            ClientInfoInstance = await clientInfoAuth.RegisterNewUser(
                new ClientRegisterDTO
                {
                    Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                    FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                    ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                    Password = Helpers.RandomString(10),
                    Hint = Helpers.RandomString(3),
                    PartnerId = "Lykke.blue"
                }
            );

            ClientInfoConsumer = new ApiConsumer(_configBuilder, clientInfoAuth);

            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(clientInfoAuth.ClientInfo.ClientId));
            //AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(ClientInfoConsumer.ClientInfo.ClientId));
        }

        public void CreateClientAccountApiConsumer()
        {
            var clientInfoAuth = new OAuthConsumer(_clientAccountConfigBuilder);
            ClientAccountConsumer = new ApiConsumer(_clientAccountConfigBuilder, clientInfoAuth);
        }

        public async Task RegisterNewUserForTestPartner()
        {
            var clientInfoAuth = new OAuthConsumer(_clientAccountConfigBuilder);

            ClientInfoInstance = await clientInfoAuth.RegisterNewUser(
                new ClientRegisterDTO
                {
                    Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                    FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                    ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                    Password = Helpers.RandomString(10),
                    Hint = Helpers.RandomString(3),
                    PartnerId = "NewTestPartner"
                }
            );

            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(clientInfoAuth.ClientInfo.ClientId));
        }
    }
}
