using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using BlueApiData.Fixtures;
using FluentAssertions;
using RestSharp;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities.BlueApi;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests: BlueApiTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesGet")]
        public async Task GetPledge()
        {
            await this.PrepareDefaultTestPledge();

            var url = ApiPaths.PLEDGES_BASE_PATH;
            var response = await this.PledgeApiConsumers["GetPledge"].ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);

            var parsedResponse = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            this.TestPledge.ShouldBeEquivalentTo(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesPost")]
        public async Task CreatePledge()
        {
            await this.PrepareCreateTestPledge();
            var createdPledge = await this.CreateTestPledge("CreatePledge");
            Assert.NotNull(createdPledge);

            var createdPledgeEntity = (PledgeEntity)await this.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == PledgeApiConsumers["CreatePledge"].ClientInfo.Account.Id);

            createdPledgeEntity.ShouldBeEquivalentTo(createdPledge, o => o.ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesPut")]
        public async Task UpdatePledge()
        {
            await this.PrepareUpdateTestPledge();

            var editPledge = new PledgeDTO()
            {
                Id = this.TestPledgeUpdate.Id,
                ClientId = this.TestPledgeUpdate.ClientId,
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            var url = ApiPaths.PLEDGES_BASE_PATH;
            var body = JsonUtils.SerializeObject(editPledge);
            var response = await this.PledgeApiConsumers["UpdatePledge"].ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.PUT);

            Assert.True(response.Status == HttpStatusCode.NoContent);

            var editedPledgeEntity = (PledgeEntity)await this.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == this.PledgeApiConsumers["UpdatePledge"].ClientInfo.Account.Id);

            editedPledgeEntity.ShouldBeEquivalentTo(editPledge, o => o.ExcludingMissingMembers().Excluding(p => p.ClientId));
        }

        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesDelete")]
        public async Task DeletePledge()
        {
            await this.PrepareDeleteTestPledge();

            var url = ApiPaths.PLEDGES_BASE_PATH;

            var deleteResponse = await this.PledgeApiConsumers["DeletePledge"].ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var deletedPledgeEntity = (PledgeEntity)await this.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == this.PledgeApiConsumers["DeletePledge"].ClientInfo.Account.Id);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
