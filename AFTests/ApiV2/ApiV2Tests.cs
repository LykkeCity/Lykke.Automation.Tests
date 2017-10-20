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
        public async void PledgeEndPoint()
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


            //Get pledges
            var allResponse = await fixture.Consumer.ExecuteRequest(url, emptyDict, null, Method.GET);
            Assert.True(allResponse.Status == HttpStatusCode.OK);
            List<PledgeDTO> parsedResponseAll = JsonUtils.DeserializeJson<List<PledgeDTO>>(allResponse.ResponseJson);
            Assert.True(parsedResponseAll.Any(p =>
            p.Id == createdPledge.Id &&
            p.ClientId == createdPledge.ClientId &&
            p.CO2Footprint == createdPledge.CO2Footprint &&
            p.ClimatePositiveValue == createdPledge.ClimatePositiveValue
            ));

            //Get single pledge
            string getSingleUrl = url + "/" + createdPledge.Id;
            var singleResponse = await fixture.Consumer.ExecuteRequest(getSingleUrl, emptyDict, null, Method.GET);
            Assert.True(singleResponse.Status == HttpStatusCode.OK);
            PledgeDTO parsedResponseSingle = JsonUtils.DeserializeJson<PledgeDTO>(singleResponse.ResponseJson);
            createdPledge.ShouldBeEquivalentTo(parsedResponseSingle, o => o
            .ExcludingMissingMembers());

            //Edit pledge
            CreatePledgeDTO editPledge = new CreatePledgeDTO();
            editPledge.CO2Footprint = random.Next(100, 100000);
            editPledge.ClimatePositiveValue = random.Next(100, 100000);
            string editPledgeUrl = url + "/" + parsedResponseSingle.Id;
            string editParam = JsonUtils.SerializeObject(editPledge);
            var editResponse = await fixture.Consumer.ExecuteRequest(editPledgeUrl, emptyDict, editParam, Method.PUT);
            Assert.True(editResponse.Status == HttpStatusCode.OK);

            PledgeDTO parsedEditResponse = JsonUtils.DeserializeJson<PledgeDTO>(editResponse.ResponseJson);

            //Delete pledge
            var deleteResponse = await fixture.Consumer.ExecuteRequest(editPledgeUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.OK);
        }
    }
}
