using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Utils;

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture
    {
        public async Task<PledgeDTO> CreateTestPledge(string consumerIndex = null)
        {
            ApiConsumer consumer;
            if (consumerIndex == null)
                consumer = this.Consumer;
            else
                consumer = this.PledgeApiConsumers[consumerIndex];

            var url = ApiEndpointNames["Pledges"];
            var newPledge = new CreatePledgeDTO()
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

            var returnDto = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            _pledgesToDelete.Add(returnDto.Id, consumerIndex);

            return returnDto;
        }

        public async Task<bool> DeleteTestPledge(string id, string consumerIndex = null)
        {
            ApiConsumer consumer;
            if (consumerIndex == null)
                consumer = this.Consumer;
            else
                consumer = this.PledgeApiConsumers[consumerIndex];

            var deletePledgeUrl = ApiEndpointNames["Pledges"] + "/" + id;
            var deleteResponse = await consumer.ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            if (deleteResponse.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }
    }
}
