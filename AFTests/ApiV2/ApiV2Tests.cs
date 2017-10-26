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
                if (fixture.AllPledgesFromDB[i].Id != fixture.TestPledgeUpdate.Id &&
                    fixture.AllPledgesFromDB[i].Id != fixture.TestPledgeDelete.Id)
                {
                    fixture.AllPledgesFromDB[i].ShouldBeEquivalentTo(
                        parsedResponseAll.Where(p => p.Id == fixture.AllPledgesFromDB[i].Id).FirstOrDefault(),
                        o => o.ExcludingMissingMembers());
                }
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
        public async void CreatePledge()
        {

            PledgeDTO createdPledge = await fixture.CreateTestPledge();
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
            Random random = new Random();

            PledgeDTO editPledge = new PledgeDTO()
            {
                Id = fixture.TestPledgeUpdate.Id,
                ClientId = fixture.TestClientId,
                CO2Footprint = random.Next(100, 100000),
                ClimatePositiveValue = random.Next(100, 100000)
            };

            string editPledgeUrl = fixture.ApiEndpointNames["Pledges"];
            string editParam = JsonUtils.SerializeObject(editPledge);
            var editResponse = await fixture.Consumer.ExecuteRequest(editPledgeUrl, emptyDict, editParam, Method.PUT);
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

            var deleteResponse = await fixture.Consumer.ExecuteRequest(deletePledgeUrl, emptyDict, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            PledgeEntity deletedPledgeEntity = (PledgeEntity)await fixture.PledgeRepository.TryGetAsync(fixture.TestPledgeDelete.Id);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
