using ApiV2Data.DTOs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Utils;

namespace ApiV2Data.Fixtures
{
    public partial class ApiV2TestDataFixture : IDisposable
    {
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

        public async Task<WalletDTO> CreateTestWallet(bool isHFT = false)
        {
            string url = ApiEndpointNames["Wallets"];
            if (isHFT)
                url += "/hft";

            WalletCreateDTO newWallet = new WalletCreateDTO()
            {
                Name = Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest",
                Description = Guid.NewGuid().ToString() + Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest"
            };
            string createParam = JsonUtils.SerializeObject(newWallet);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.OK)
            {
                return null;
            }

            WalletDTO returnModel;

            if (isHFT)
            {
                returnModel = new WalletDTO();
                WalletCreateHFTDTO createdDTO = JsonUtils.DeserializeJson<WalletCreateHFTDTO>(response.ResponseJson);
                returnModel.Id = createdDTO.WalletId;
                returnModel.ApiKey = createdDTO.ApiKey;
                returnModel.Name = newWallet.Name;
                returnModel.Description = newWallet.Description;
            }
            else
            {
                returnModel = JsonUtils.DeserializeJson<WalletDTO>(response.ResponseJson);
            }

            WalletsToDelete.Add(returnModel.Id);

            return returnModel;
        }

        public async Task<bool> DeleteTestWallet(string id)
        {
            string url = ApiEndpointNames["Wallets"] + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }
    }
}
