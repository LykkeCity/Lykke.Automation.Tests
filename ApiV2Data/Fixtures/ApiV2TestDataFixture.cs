using ApiV2Data.DependencyInjection;
using Autofac;
using AutoMapper;
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

        private Dictionary<string, string> PledgesToDelete;

        public PledgesRepository PledgeRepository;
        public PledgeDTO TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;

        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;

        public Dictionary<string, ApiConsumer> PledgeApiConsumers;


        public ApiV2TestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("ApiV2");
            this.Consumer = new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]));
            this.Consumer.Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["AuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));

            PledgeApiConsumers = new Dictionary<string, ApiConsumer>();
            PledgeApiConsumers.Add("CreatePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));
            PledgeApiConsumers.Add("UpdatePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));
            PledgeApiConsumers.Add("DeletePledge", new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"])));

            PledgeApiConsumers["CreatePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeCreateAuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));
            PledgeApiConsumers["UpdatePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeUpdateAuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));
            PledgeApiConsumers["DeletePledge"].Authenticate(_configBuilder.Config["BaseUrlAuth"], _configBuilder.Config["AuthPath"], _configBuilder.Config["PledgeDeleteAuthEmail"],
                _configBuilder.Config["AuthPassword"], _configBuilder.Config["AuthClientInfo"], _configBuilder.Config["AuthPartnerId"], Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]));

            prepareDependencyContainer();
            prepareTestData().Wait();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ApiV2TestModule(_configBuilder));
            this.container = builder.Build();

            this.PledgeRepository = (PledgesRepository)this.container.Resolve<IDictionaryRepository<IPledgeEntity>>();
        }

        private async Task prepareTestData()
        {
            ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["Pledges"] = "/api/pledges";

            PledgesToDelete = new Dictionary<string, string>();

            this.TestPledge = await CreateTestPledge();

            this.TestPledgeUpdate = await CreateTestPledge("UpdatePledge");
            this.TestPledgeDelete = await CreateTestPledge("DeletePledge");

        }

        public void Dispose()
        {
            List<Task<bool>> deleteTasks = new List<Task<bool>>();
            foreach (KeyValuePair<string, string> pledgeData in PledgesToDelete) { deleteTasks.Add(DeleteTestPledge(pledgeData.Key, pledgeData.Value)); }

            Task.WhenAll(deleteTasks).Wait();
        }

        #region Create / Delete methods

        public async Task<PledgeDTO> CreateTestPledge(string consumerIndex = null)
        {
            ApiConsumer consumer;
            if (consumerIndex == null)
                consumer = this.Consumer;
            else
                consumer = this.PledgeApiConsumers[consumerIndex];

            string url = ApiEndpointNames["Pledges"];
            CreatePledgeDTO newPledge = new CreatePledgeDTO()
            {
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            string createParam = JsonUtils.SerializeObject(newPledge);
            var response = await consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            PledgeDTO returnDTO = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            PledgesToDelete.Add(returnDTO.Id, consumerIndex);

            return returnDTO;
        }

        public async Task<bool> DeleteTestPledge(string id, string consumerIndex = null)
        {
            ApiConsumer consumer;
            if (consumerIndex == null)
                consumer = this.Consumer;
            else
                consumer = this.PledgeApiConsumers[consumerIndex];

            string deletePledgeUrl = ApiEndpointNames["Pledges"] + "/" + id;
            var deleteResponse = await consumer.ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
