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
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.ApiV2;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        private ApiV2TestDataFixture fixture;
        private Dictionary<string, string> emptyDict = new Dictionary<string, string>();

        public ApiV2Tests(ApiV2TestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesGet")]
        public async void GetAllPledges()
        {
            string url = fixture.ApiEndpointNames["Pledges"];
            var allResponse = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
            Assert.True(allResponse.Status == HttpStatusCode.OK);
            List<PledgeDTO> parsedResponseAll = JsonUtils.DeserializeJson<List<PledgeDTO>>(allResponse.ResponseJson);
            for (int i = 0; i < fixture.AllPledgesFromDB.Count; i++)
            {
                fixture.AllPledgesFromDB[i].ShouldBeEquivalentTo(parsedResponseAll[i], o => o
                .ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesGet")]
        public async void GetSinglePledge()
        {
            string getSingleUrl = fixture.ApiEndpointNames["Pledges"] + "/" + fixture.TestPledge.Id;

            var singleResponse = await fixture.Consumer.ExecuteRequest(getSingleUrl, emptyDict, null, Method.GET);
            Assert.True(singleResponse.Status == HttpStatusCode.OK);
            PledgeDTO parsedResponseSingle = JsonUtils.DeserializeJson<PledgeDTO>(singleResponse.ResponseJson);
            fixture.TestPledge.ShouldBeEquivalentTo(parsedResponseSingle, o => o
            .ExcludingMissingMembers());

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Pledges")]
        [Trait("Category", "PledgesPost")]
        [Trait("Category", "PledgesPut")]
        [Trait("Category", "PledgesDelete")]
        public async void CreateUpdateDeletePledge()
        {
            string url = fixture.ApiEndpointNames["Pledges"];

            //create pledge
            Random random = new Random();
            CreatePledgeDTO newPledge = new CreatePledgeDTO()
            {
                CO2Footprint = random.Next(100, 100000),
                ClimatePositiveValue = random.Next(100, 100000)
            };

            string createParam = JsonUtils.SerializeObject(newPledge);
            var response = await fixture.Consumer.ExecuteRequest(url, emptyDict, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);
            PledgeDTO createdPledge = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);
            Assert.NotNull(createdPledge);

            PledgeEntity createdPledgeEntity = (PledgeEntity)await fixture.PledgeRepository.TryGetAsync(createdPledge.Id);
            createdPledgeEntity.ShouldBeEquivalentTo(createdPledge, o => o
            .ExcludingMissingMembers());

            //Edit pledge
            CreatePledgeDTO editPledge = new CreatePledgeDTO();
            editPledge.CO2Footprint = random.Next(100, 100000);
            editPledge.ClimatePositiveValue = random.Next(100, 100000);
            string editPledgeUrl = url + "/" + createdPledge.Id;
            string editParam = JsonUtils.SerializeObject(editPledge);
            var editResponse = await fixture.Consumer.ExecuteRequest(editPledgeUrl, emptyDict, editParam, Method.PUT);
            Assert.True(editResponse.Status == HttpStatusCode.OK);
            PledgeDTO parsedEditResponse = JsonUtils.DeserializeJson<PledgeDTO>(editResponse.ResponseJson);

            PledgeEntity editedPledgeEntity = (PledgeEntity)await fixture.PledgeRepository.TryGetAsync(createdPledge.Id);
            editedPledgeEntity.ShouldBeEquivalentTo(parsedEditResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(p => p.ClientId));


            //Delete pledge
            var deleteResponse = await fixture.Consumer.ExecuteRequest(editPledgeUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.OK);

            PledgeEntity deletedPledgeEntity = (PledgeEntity)await fixture.PledgeRepository.TryGetAsync(createdPledge.Id);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
