using ApiV2Data.Fixtures;
using RestSharp;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTestCommon;
using ApiV2Data.DTOs;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.ApiV2;
using FluentAssertions;
using System.Linq;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetAllWallets()
        {
            string url = fixture.ApiEndpointNames["Wallets"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletDTO>>(response.ResponseJson);

            foreach (WalletEntity entity in fixture.AllWalletsFromDB)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(w => w.Id == entity.Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers()
                .Excluding(w => w.Description)); //TODO
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetSingleWallets()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/" + fixture.TestWallet.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WalletDTO parsedResponse = JsonUtils.DeserializeJson<WalletDTO>(response.ResponseJson);

            fixture.TestWallet.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(w => w.Description)); //TODO
        }
    }
}
