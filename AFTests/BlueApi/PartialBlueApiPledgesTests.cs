using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using FluentAssertions;
using RestSharp;
using Xunit;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.BlueApi;

namespace AFTests.BlueApi
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "BlueApiService")]
    public partial class BlueApiTests
    {
        //[Fact(Skip = "test will fail")]
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesGet")]
        public async Task GetPledge()
        {
            var url = _fixture.ApiEndpointNames["Pledges"];
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);

            var parsedResponse = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            _fixture.TestPledge.ShouldBeEquivalentTo(parsedResponse);
        }

        //[Fact(Skip = "test will fail")]
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesPost")]
        public async Task CreatePledge()
        {
            var createdPledge = await _fixture.CreateTestPledge(_fixture.TestPledgeCreateClientId, "CreatePledge");
            Assert.NotNull(createdPledge);

            var createdPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.GetPledgeAsync(_fixture.TestPledgeCreateClientId);
            createdPledgeEntity.ShouldBeEquivalentTo(createdPledge, o => o.ExcludingMissingMembers());
        }

        //[Fact(Skip = "test will fail")]
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesPut")]
        public async Task UpdatePledge()
        {
            var editPledge = new PledgeDTO()
            {
                Id = _fixture.TestPledgeUpdate.Id,
                ClientId = _fixture.TestPledgeUpdate.ClientId,
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            var editPledgeUrl = _fixture.ApiEndpointNames["Pledges"];
            var editParam = JsonUtils.SerializeObject(editPledge);
            var editResponse = await _fixture.PledgeApiConsumers["UpdatePledge"].ExecuteRequest(editPledgeUrl, Helpers.EmptyDictionary, editParam, Method.PUT);

            Assert.True(editResponse.Status == HttpStatusCode.NoContent);

            var editedPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.GetPledgeAsync(_fixture.TestPledgeUpdateClientId);

            editedPledgeEntity.ShouldBeEquivalentTo(editPledge, o => o.ExcludingMissingMembers().Excluding(p => p.ClientId));
        }

        //[Fact(Skip = "test will fail")]
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesDelete")]
        public async Task DeletePledge()
        {
            var deletePledgeUrl = _fixture.ApiEndpointNames["Pledges"];

            var deleteResponse = await _fixture.PledgeApiConsumers["DeletePledge"].ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var deletedPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.GetPledgeAsync(_fixture.TestPledgeDeleteClientId);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
