using System;
using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture
    {
        public async Task<PledgeDTO> CreateTestPledge(string clientId, string consumerIndex = null)
        {
            var consumer = String.IsNullOrEmpty(consumerIndex) ? Consumer : PledgeApiConsumers[consumerIndex];

            var url = ApiEndpointNames["Pledges"];
            var newPledge = new CreatePledgeDTO
            {
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            var createParam = JsonUtils.SerializeObject(newPledge);
            var response = await consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            var pledge = await PledgeRepository.GetPledgeAsync(clientId);

            var returnDto = new PledgeDTO
            {
                Id = pledge.Id,
                CO2Footprint = pledge.CO2Footprint,
                ClientId = pledge.ClientId,
                ClimatePositiveValue = pledge.ClimatePositiveValue
            };

            _pledgesToDelete.Add(returnDto.Id, consumerIndex);

            return returnDto;
        }

        public async Task<bool> DeleteTestPledge(string consumerIndex = null)
        {
            var consumer = String.IsNullOrEmpty(consumerIndex) ? Consumer : PledgeApiConsumers[consumerIndex];

            var deletePledgeUrl = ApiEndpointNames["Pledges"];
            var deleteResponse = await consumer.ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }
    }
}
