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

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture : IDisposable
    {
        private readonly ConfigBuilder _configBuilder;
        private IContainer _container;
        public IMapper Mapper;

        public string TestClientId;
        public string AccountEmail;
        public string TwitterSearchQuery;
        public DateTime TwitterSearchUntilDate;
        public Dictionary<string, string> TestPledgeClientIDs;

        private Dictionary<string, string> _pledgesToDelete;

        public GenericRepository<PledgeEntity, IPledgeEntity> PledgeRepository;
        public GenericRepository<PersonalDataEntity, IPersonalData> PersonalDataRepository;
        public PledgeDTO TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;
        public ApiConsumer Consumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;

        public BlueApiTestDataFixture()
        {
            _configBuilder = new ConfigBuilder("BlueApi");

            PrepareDependencyContainer();
            PrepareApiConsumers().Wait();
            PrepareTestData().Wait();
        }

        private async Task PrepareApiConsumers()
        {
            var oAuthConsumer = new OAuthConsumer(_configBuilder);

            Consumer = new ApiConsumer(_configBuilder, oAuthConsumer);

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            TestPledgeClientIDs = new Dictionary<string, string>();

            List<Task> ceratePledgesTasks = new List<Task>()
            {
                CreatePledgeClientAndApiConsumer("GetPledge"),
                CreatePledgeClientAndApiConsumer("CreatePledge"),
                CreatePledgeClientAndApiConsumer("UpdatePledge"),
                CreatePledgeClientAndApiConsumer("DeletePledge"),
            };

            await Task.WhenAll(ceratePledgesTasks);
        }

        private async Task CreatePledgeClientAndApiConsumer(string purpose)
        {
            OAuthConsumer oAuthConsumer = new OAuthConsumer(_configBuilder);
            User createPledgeUser = await oAuthConsumer.RegisterNewUser();
            TestPledgeClientIDs[purpose] = await GetClientIdByEmail(createPledgeUser.Email);
            PledgeApiConsumers.Add(purpose, new ApiConsumer(_configBuilder, oAuthConsumer));
        }

        private void PrepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new BlueApiTestModule(_configBuilder));
            _container = builder.Build();

            PledgeRepository = RepositoryUtils.ResolveGenericRepository<PledgeEntity, IPledgeEntity>(this._container);
            PersonalDataRepository = RepositoryUtils.ResolveGenericRepository<PersonalDataEntity, IPersonalData>(this._container);
        }

        public void Dispose()
        {
            var deleteTasks = new List<Task<bool>>();

            foreach (KeyValuePair<string, string> pledgeData in _pledgesToDelete) { deleteTasks.Add(DeleteTestPledge(pledgeData.Value)); }

            Task.WhenAll(deleteTasks).Wait();
            GC.SuppressFinalize(this);
        }
    }
}
