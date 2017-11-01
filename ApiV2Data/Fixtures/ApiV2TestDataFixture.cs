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

        private List<string> PledgesToDelete;

        public string TestClientId;

        public PledgesRepository PledgeRepository;
        public List<PledgeEntity> AllPledgesFromDB;
        public PledgeEntity TestPledge;
        public PledgeDTO TestPledgeUpdate;
        public PledgeDTO TestPledgeDelete;

        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;


        public ApiV2TestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("ApiV2");
            this.Consumer = new ApiConsumer(this._configBuilder);

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

            PledgesToDelete = new List<string>();
            TestClientId = this._configBuilder.Config["AuthClientId"];

            var pledgesFromDB = Task.Run(async () => { return await this.PledgeRepository.GetAllByClientAsync(this._configBuilder.Config["AuthClientId"]); }).Result;
            this.AllPledgesFromDB = pledgesFromDB.Cast<PledgeEntity>().ToList();
            this.TestPledge = EnumerableUtils.PickRandom(AllPledgesFromDB);

            this.TestPledgeUpdate = await CreateTestPledge();
            this.TestPledgeDelete = await CreateTestPledge(false);


        }

        public void Dispose()
        {
            List<Task<bool>> deleteTasks = new List<Task<bool>>();
            foreach (string pledgeId in PledgesToDelete) { deleteTasks.Add(DeleteTestPledge(pledgeId)); }

            Task.WhenAll(deleteTasks).Wait();
        }

        #region Create / Delete methods

        public async Task<PledgeDTO> CreateTestPledge(bool deleteWithDispose = true)
        {
            string url = ApiEndpointNames["Pledges"];
            CreatePledgeDTO newPledge = new CreatePledgeDTO()
            {
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            string createParam = JsonUtils.SerializeObject(newPledge);
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            PledgeDTO returnDTO = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            if (deleteWithDispose)
            {
                PledgesToDelete.Add(returnDTO.Id);
            }

            return returnDTO;
        }

        public async Task<bool> DeleteTestPledge(string id)
        {
            string deletePledgeUrl = ApiEndpointNames["Pledges"] + "/" + id;
            var deleteResponse = await Consumer.ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
