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

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture
    {
        private readonly ConfigBuilder _configBuilder;
        private IContainer _container;
        public IMapper Mapper;

        public string TestClientId;
        public string AccountEmail;
        public string TwitterSearchQuery;
        public DateTime TwitterSearchUntilDate;
        public string TestPledgeCreateClientId;
        public string TestPledgeUpdateClientId;
        public string TestPledgeDeleteClientId;

        public GenericRepository<PledgeEntity, IPledgeEntity> PledgeRepository;
        public GenericRepository<ReferralLinkEntity, IReferralLink> ReferralLinkRepository;
        public PledgeDTO TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;
        public ApiConsumer Consumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

        public BlueApiTestDataFixture()
        {
            _configBuilder = new ConfigBuilder("BlueApi");

            PrepareApiConsumers();
            PrepareDependencyContainer();
            PrepareMapper();
        }

        private void PrepareApiConsumers()
        {
            var oAuthConsumer = new OAuthConsumer
            {
                AuthTokenTimeout = Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]),
                AuthPath = _configBuilder.Config["AuthPath"],
                BaseAuthUrl = _configBuilder.Config["BaseUrlAuth"],
                AuthUser = new User
                {
                    ClientInfo = _configBuilder.Config["AuthClientInfo"],
                    Email = _configBuilder.Config["AuthEmail"],
                    PartnerId = _configBuilder.Config["AuthPartnerId"],
                    Password = _configBuilder.Config["AuthPassword"]
                }
            };

            Consumer = new ApiConsumer(_configBuilder, oAuthConsumer);

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            oAuthConsumer = new OAuthConsumer
            {
                AuthTokenTimeout = Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]),
                AuthPath = _configBuilder.Config["AuthPath"],
                BaseAuthUrl = _configBuilder.Config["BaseUrlAuth"],
                AuthUser = new User
                {
                    ClientInfo = _configBuilder.Config["AuthClientInfo"],
                    Email = _configBuilder.Config["PledgeCreateAuthEmail"],
                    PartnerId = _configBuilder.Config["AuthPartnerId"],
                    Password = _configBuilder.Config["AuthPassword"]
                }
            };
            PledgeApiConsumers.Add("CreatePledge", new ApiConsumer(_configBuilder, oAuthConsumer));

            oAuthConsumer = new OAuthConsumer
            {
                AuthTokenTimeout = Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]),
                AuthPath = _configBuilder.Config["AuthPath"],
                BaseAuthUrl = _configBuilder.Config["BaseUrlAuth"],
                AuthUser = new User
                {
                    ClientInfo = _configBuilder.Config["AuthClientInfo"],
                    Email = _configBuilder.Config["PledgeUpdateAuthEmail"],
                    PartnerId = _configBuilder.Config["AuthPartnerId"],
                    Password = _configBuilder.Config["AuthPassword"]
                }
            };
            PledgeApiConsumers.Add("UpdatePledge", new ApiConsumer(_configBuilder, oAuthConsumer));

            oAuthConsumer = new OAuthConsumer
            {
                AuthTokenTimeout = Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]),
                AuthPath = _configBuilder.Config["AuthPath"],
                BaseAuthUrl = _configBuilder.Config["BaseUrlAuth"],
                AuthUser = new User
                {
                    ClientInfo = _configBuilder.Config["AuthClientInfo"],
                    Email = _configBuilder.Config["PledgeDeleteAuthEmail"],
                    PartnerId = _configBuilder.Config["AuthPartnerId"],
                    Password = _configBuilder.Config["AuthPassword"]
                }
            };
            PledgeApiConsumers.Add("DeletePledge", new ApiConsumer(_configBuilder, oAuthConsumer));
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BlueApiTestModule(_configBuilder));
            _container = builder.Build();

            PledgeRepository = RepositoryUtils.ResolveGenericRepository<PledgeEntity, IPledgeEntity>(_container);
            ReferralLinkRepository = RepositoryUtils.ResolveGenericRepository<ReferralLinkEntity, IReferralLink>(_container);
        }
    }
}
