﻿using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using BlueApiData.Fixtures;
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
    public partial class BlueApiTests : IClassFixture<BlueApiTestDataFixture>
    {
        [Fact(Skip = "test will fail")]
        //[Fact]
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

        [Fact(Skip = "test will fail")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesGet")]
        public async Task GetPledgeById()
        {
            var getSingleUrl = _fixture.ApiEndpointNames["Pledges"] + "/" + _fixture.TestPledge.Id;

            var singleResponse = await _fixture.Consumer.ExecuteRequest(getSingleUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(singleResponse.Status == HttpStatusCode.OK);
            var parsedResponseSingle = JsonUtils.DeserializeJson<PledgeDTO>(singleResponse.ResponseJson);
            _fixture.TestPledge.ShouldBeEquivalentTo(parsedResponseSingle);

        }

        [Fact(Skip = "test will fail")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesPost")]
        public async Task CreatePledge()
        {
            var createdPledge = await _fixture.CreateTestPledge("CreatePledge");
            Assert.NotNull(createdPledge);

            var createdPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.TryGetAsync(createdPledge.Id);
            createdPledgeEntity.ShouldBeEquivalentTo(createdPledge, o => o
            .ExcludingMissingMembers());
        }

        [Fact(Skip = "test will fail")]
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
            //Assert.True(editResponse.Status == HttpStatusCode.OK);
            var parsedEditResponse = JsonUtils.DeserializeJson<PledgeDTO>(editResponse.ResponseJson);

            var editedPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.TryGetAsync(_fixture.TestPledgeUpdate.Id);
            editedPledgeEntity.ShouldBeEquivalentTo(parsedEditResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(p => p.ClientId));
        }

        [Fact(Skip = "test will fail")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesDelete")]
        public async Task DeletePledge()
        {
            var deletePledgeUrl = _fixture.ApiEndpointNames["Pledges"] + "/" + _fixture.TestPledgeDelete.Id;

            var deleteResponse = await _fixture.PledgeApiConsumers["DeletePledge"].ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var deletedPledgeEntity = (PledgeEntity)await _fixture.PledgeRepository.TryGetAsync(_fixture.TestPledgeDelete.Id);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
