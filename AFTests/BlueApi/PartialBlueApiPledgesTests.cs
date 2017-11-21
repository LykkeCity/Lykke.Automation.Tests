﻿using System.Net;
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
            var response = await this.PledgeApiConsumers["GetPledge"].ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);

            var parsedResponse = JsonUtils.DeserializeJson<PledgeDTO>(response.ResponseJson);

            this.TestPledge.ShouldBeEquivalentTo(parsedResponse);
        }

        //[Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("Pledges")]
        [Category("PledgesPost")]
        public async Task CreatePledge()
        {
            var createdPledge = await this.CreateTestPledge(this.TestPledgeClientIDs["CreatePledge"], "CreatePledge");
            Assert.NotNull(createdPledge);

            var createdPledgeEntity = (PledgeEntity)await this.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == this.TestPledgeClientIDs["CreatePledge"]);

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
                Id = this.TestPledgeUpdate.Id,
                ClientId = this.TestPledgeUpdate.ClientId,
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            var url = ApiPaths.PLEDGES_BASE_PATH;
            var editParam = JsonUtils.SerializeObject(editPledge);
            var editResponse = await this.PledgeApiConsumers["UpdatePledge"].ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);

            Assert.True(editResponse.Status == HttpStatusCode.NoContent);

            var editedPledgeEntity = (PledgeEntity)await this.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == this.TestPledgeClientIDs["UpdatePledge"]);

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

            var deleteResponse = await this.PledgeApiConsumers["DeletePledge"].ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var deletedPledgeEntity = (PledgeEntity)await this.PledgeRepository.TryGetAsync(
                p => p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == this.TestPledgeClientIDs["DeletePledge"]);
            Assert.Null(deletedPledgeEntity);
        }
    }
}
