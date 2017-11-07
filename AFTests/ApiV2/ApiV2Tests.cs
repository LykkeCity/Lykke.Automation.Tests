using ApiV2Data.DTOs;
using ApiV2Data.Fixtures;
using FluentAssertions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.ApiV2;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        private ApiV2TestDataFixture fixture;

        public ApiV2Tests(ApiV2TestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesGet")]
        public async void GetPledge()
        {
            string url = fixture.ApiEndpointNames["Pledges"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);
            PledgeDTO parsedResponse = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            fixture.TestPledge.ShouldBeEquivalentTo(parsedResponse);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesGet")]
        public async void GetPledgeById()
        {
            string getSingleUrl = fixture.ApiEndpointNames["Pledges"] + "/" + fixture.TestPledge.Id;

            var singleResponse = await fixture.Consumer.ExecuteRequest(getSingleUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(singleResponse.Status == HttpStatusCode.OK);
            PledgeDTO parsedResponseSingle = JsonUtils.DeserializeJson<PledgeDTO>(singleResponse.ResponseJson);
            fixture.TestPledge.ShouldBeEquivalentTo(parsedResponseSingle);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesPost")]
        public async void CreatePledge()
        {
            PledgeDTO createdPledge = await fixture.CreateTestPledge("CreatePledge");
            Assert.NotNull(createdPledge);

            PledgeEntity createdPledgeEntity = (PledgeEntity)await fixture.PledgeRepository.TryGetAsync(createdPledge.Id);
            createdPledgeEntity.ShouldBeEquivalentTo(createdPledge, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesPut")]
        public async void UpdatePledge()
        {
            PledgeDTO editPledge = new PledgeDTO()
            {
                Id = fixture.TestPledgeUpdate.Id,
                ClientId = fixture.TestPledgeUpdate.ClientId,
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            string editPledgeUrl = fixture.ApiEndpointNames["Pledges"];
            string editParam = JsonUtils.SerializeObject(editPledge);
            var editResponse = await fixture.PledgeApiConsumers["UpdatePledge"].ExecuteRequest(editPledgeUrl, Helpers.EmptyDictionary, editParam, Method.PUT);
            //Assert.True(editResponse.Status == HttpStatusCode.OK);
            PledgeDTO parsedEditResponse = JsonUtils.DeserializeJson<PledgeDTO>(editResponse.ResponseJson);

            PledgeEntity editedPledgeEntity = (PledgeEntity)await fixture.PledgeRepository.TryGetAsync(fixture.TestPledgeUpdate.Id);
            editedPledgeEntity.ShouldBeEquivalentTo(parsedEditResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(p => p.ClientId));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesDelete")]
        public async void DeletePledge()
        {
            string deletePledgeUrl = fixture.ApiEndpointNames["Pledges"] + "/" + fixture.TestPledgeDelete.Id;

            var deleteResponse = await fixture.PledgeApiConsumers["DeletePledge"].ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            PledgeEntity deletedPledgeEntity = (PledgeEntity)await fixture.PledgeRepository.TryGetAsync(fixture.TestPledgeDelete.Id);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
