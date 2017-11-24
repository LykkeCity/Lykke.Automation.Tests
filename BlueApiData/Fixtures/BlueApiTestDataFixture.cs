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
        public MatchingEngineConsumer MEConsumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

        public ApiConsumer InvitationLinkRequestConsumer;
        public List<ApiConsumer> InvitationLinkClaimersConsumers;

        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("BlueApi");

            PrepareDependencyContainer();
            PrepareApiConsumers().Wait();
            PrepareMapper();
        }

        private async Task PrepareApiConsumers()
        {
            var oAuthConsumer = new OAuthConsumer(_configBuilder);

            Consumer = new ApiConsumer(_configBuilder, oAuthConsumer);

            ConfigBuilder MeConfig = new ConfigBuilder("MatchingEngine");
            if (Int32.TryParse(MeConfig.Config["Port"], out int port))
            {
                MEConsumer = new MatchingEngineConsumer(MeConfig.Config["BaseUrl"], port);
            }

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            TestPledgeClientIDs = new Dictionary<string, string>();

        }

        public async Task CreatePledgeClientAndApiConsumer(string purpose)
        {
            OAuthConsumer oAuthConsumer = new OAuthConsumer(_configBuilder);
            ClientRegisterDTO createPledgeUser = await oAuthConsumer.RegisterNewUser();
            ApiConsumer consumer = new ApiConsumer(_configBuilder, oAuthConsumer);
            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.ClientId));

            TestPledgeClientIDs[purpose] = consumer.ClientInfo.ClientId;
            PledgeApiConsumers.Add(purpose, consumer);
        }

        private async Task<List<ApiConsumer>> RegisterNUsers(int n)
        {
            List<ApiConsumer> result = new List<ApiConsumer>();

            for (int i = 0; i < n; i++)
            {
                OAuthConsumer oAuthConsumer = new OAuthConsumer(_configBuilder);
                ClientRegisterDTO createPledgeUser = await oAuthConsumer.RegisterNewUser();
                ApiConsumer consumer = new ApiConsumer(_configBuilder, oAuthConsumer);
                AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(consumer.ClientInfo.ClientId));
                result.Add(new ApiConsumer(_configBuilder, oAuthConsumer));
            }

            return result;
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BlueApiTestModule(_configBuilder));
            _container = builder.Build();

            PledgeRepository = RepositoryUtils.ResolveGenericRepository<PledgeEntity, IPledgeEntity>(this._container);
            PersonalDataRepository = RepositoryUtils.ResolveGenericRepository<PersonalDataEntity, IPersonalData>(this._container);
            ReferralLinkRepository = RepositoryUtils.ResolveGenericRepository<ReferralLinkEntity, IReferralLink>(_container);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            
        }
    }
}
