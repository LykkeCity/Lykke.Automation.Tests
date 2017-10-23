using ApiV2Data.DependencyInjection;
using Autofac;
using System;
using System.Linq;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Repositories.ApiV2;
using XUnitTestData.Services;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using ApiV2Data.DTOs;
using RestSharp;
using System.Net;

namespace ApiV2Data.Fixtures
{
    public class ApiV2TestDataFixture : IDisposable
    {
        private ConfigBuilder _configBuilder;
        private IContainer container;

        public PledgesRepository PledgeRepository;
        public List<PledgeEntity> AllPledgesFromDB;
        public PledgeEntity TestPledge;

        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;

        public ApiV2TestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("ApiV2");
            this.Consumer = new ApiConsumer(this._configBuilder);

            prepareDependencyContainer();
            prepareTestData();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApiV2TestModule(_configBuilder));
            this.container = builder.Build();

            this.PledgeRepository = (PledgesRepository)this.container.Resolve<IDictionaryRepository<IPledgeEntity>>();
        }

        private void prepareTestData()
        {
            ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["Pledges"] = "/api/pledges";

            var pledgesFromDB = Task.Run(async () => { return await this.PledgeRepository.GetAllByClientAsync(this._configBuilder.Config["AuthClientId"]); }).Result;
            this.AllPledgesFromDB = pledgesFromDB.Cast<PledgeEntity>().ToList();

            //create a test pledge of none exist
            if (AllPledgesFromDB.Count == 0)
            {
                string url = ApiEndpointNames["Pledges"];
                Random random = new Random();
                CreatePledgeDTO newPledge = new CreatePledgeDTO()
                {
                    CO2Footprint = random.Next(100, 100000),
                    ClimatePositiveValue = random.Next(100, 100000)
                };

                string createParam = JsonUtils.SerializeObject(newPledge);
                Response response = Task.Run(async () => { return await Consumer.ExecuteRequest(url, new Dictionary<string, string>(), createParam, Method.POST); }).Result;
                if (response.Status == HttpStatusCode.OK) //.Created
                {
                    throw new Exception("Couldn't create test pledge");
                }
                else
                {
                    pledgesFromDB = Task.Run(async () => { return await this.PledgeRepository.GetAllByClientAsync(this._configBuilder.Config["AuthClientId"]); }).Result;
                    this.AllPledgesFromDB = pledgesFromDB.Cast<PledgeEntity>().ToList();
                }
            }

            this.TestPledge = EnumerableUtils.PickRandom(AllPledgesFromDB);


        }

        public void Dispose()
        {
            
        }
    }
}
