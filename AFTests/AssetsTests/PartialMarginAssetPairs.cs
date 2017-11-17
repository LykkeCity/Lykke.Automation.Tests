﻿using AssetsData.Fixtures;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon;
using AssetsData.DTOs.Assets;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest : AssetsTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsGet")]
        public async Task GetAllMarginAssetPairs()
        {
            string url = fixture.ApiEndpointNames["marginAssetPairs"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<MarginAssetPairDTO> parsedResponse = JsonUtils.DeserializeJson<List<MarginAssetPairDTO>>(response.ResponseJson);

            foreach (MarginAssetPairsEntity entity in fixture.AllMarginAssetPairsFromDB)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == entity.Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsGet")]
        public async Task GetSingleMarginAssetPairs()
        {
            string url = fixture.ApiEndpointNames["marginAssetPairs"] + "/" + fixture.TestMarginAssetPair.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginAssetPairDTO parsedResponse = JsonUtils.DeserializeJson<MarginAssetPairDTO>(response.ResponseJson);

            fixture.TestMarginAssetPair.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsGet")]
        public async Task CheckIfMarginAssetPairExists()
        {
            string url = fixture.ApiEndpointNames["marginAssetPairs"] + "/" + fixture.TestMarginAssetPair.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsPost")]
        public async Task CreateMarginAssetPair()
        {
            MarginAssetPairDTO createdDTO = await fixture.CreateTestMarginAssetPair();
            Assert.NotNull(createdDTO);

            await fixture.MarginAssetPairManager.UpdateCacheAsync();
            MarginAssetPairsEntity entity = await fixture.MarginAssetPairManager.TryGetAsync(createdDTO.Id) as MarginAssetPairsEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(createdDTO, o => o.ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsDelete")]
        public async Task UpdateMarginAssetPair()
        {
            string url = fixture.ApiEndpointNames["marginAssetPairs"];
            MarginAssetPairDTO updateDTO = new MarginAssetPairDTO()
            {
                Id = fixture.TestMarginAssetPairUpdate.Id,
                Accuracy = fixture.TestMarginAssetPairUpdate.Accuracy + Helpers.Random.Next(1,4),
                InvertedAccuracy = fixture.TestMarginAssetPairUpdate.InvertedAccuracy,
                Name = fixture.TestMarginAssetPairUpdate.Name + "_AutoTest",
                BaseAssetId = fixture.TestMarginAssetPairUpdate.BaseAssetId,
                QuotingAssetId = fixture.TestMarginAssetPairUpdate.QuotingAssetId
            };

            string editParam = JsonUtils.SerializeObject(updateDTO);
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.MarginAssetPairManager.UpdateCacheAsync();
            MarginAssetPairsEntity entity = await fixture.MarginAssetPairManager.TryGetAsync(updateDTO.Id) as MarginAssetPairsEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(updateDTO, o => o.ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginAssetPairs")]
        [Category("MarginAssetPairsDelete")]
        public async Task DeleteMarginAssetPair()
        {
            string url = fixture.ApiEndpointNames["marginAssetPairs"] + "/" + fixture.TestMarginAssetPairDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.MarginAssetPairManager.UpdateCacheAsync();
            MarginAssetPairsEntity entity = await fixture.MarginAssetPairManager.TryGetAsync(fixture.TestMarginAssetPairDelete.Id) as MarginAssetPairsEntity;
            Assert.Null(entity);

        }
    }
}
