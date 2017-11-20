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
    public partial class BlueApiTests
    {
        //[Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesGet")]
        public async Task GetPledge()
        {
            var url = ApiPaths.PLEDGES_BASE_PATH;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);

            var parsedResponse = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            _fixture.TestPledge.ShouldBeEquivalentTo(parsedResponse);
        }

        //[Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesGet")]
        public async Task GetPledgeById()
        {
            var getSingleUrl = ApiPaths.PLEDGES_BASE_PATH;

            var singleResponse = await _fixture.Consumer.ExecuteRequest(getSingleUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(singleResponse.Status == HttpStatusCode.OK);
            var parsedResponseSingle = JsonUtils.DeserializeJson<PledgeDTO>(singleResponse.ResponseJson);
            _fixture.TestPledge.ShouldBeEquivalentTo(parsedResponseSingle);

        }

        //[Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesPost")]
        public async Task CreatePledge()
        {
            var createdPledge = await _fixture.CreateTestPledge(_fixture.TestPledgeClientIDs["CreatePledge"], "CreatePledge");
            Assert.NotNull(createdPledge);

            var createdPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == _fixture.TestPledgeClientIDs["CreatePledge"]);

            createdPledgeEntity.ShouldBeEquivalentTo(createdPledge, o => o.ExcludingMissingMembers());
        }

        //[Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesPut")]
        public async Task UpdatePledge()
        {
            var editPledge = new PledgeDTO()
            {
                Id = _fixture.TestPledgeUpdate.Id,
                ClientId = _fixture.TestPledgeUpdate.ClientId,
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            var url = ApiPaths.PLEDGES_BASE_PATH;
            var editParam = JsonUtils.SerializeObject(editPledge);
            var editResponse = await _fixture.PledgeApiConsumers["UpdatePledge"].ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);

            Assert.True(editResponse.Status == HttpStatusCode.NoContent);

            var editedPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == _fixture.TestPledgeClientIDs["UpdatePledge"]);

            editedPledgeEntity.ShouldBeEquivalentTo(editPledge, o => o.ExcludingMissingMembers().Excluding(p => p.ClientId));
        }

        //[Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesDelete")]
        public async Task DeletePledge()
        {
            var url = ApiPaths.PLEDGES_BASE_PATH;

            var deleteResponse = await _fixture.PledgeApiConsumers["DeletePledge"].ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var deletedPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == _fixture.TestPledgeClientIDs["DeletePledge"]);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
