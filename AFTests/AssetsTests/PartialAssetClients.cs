using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using Xunit;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetClients")]
        [Trait("Category", "AssetClientsGet")]
        public async void GetClientAssetIDs()
        {
            string url = ApiPaths.CLIENTS_BASE_PATH + "/" + fixture.TestAccountIdForClientEndpoint + "/asset-ids";
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                ["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString()
            };

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            parsedResponse.Should().Contain(fixture.TestAssetForClientEndpoint.Id);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetClients")]
        [Trait("Category", "AssetClientsGet")]
        public async void GetClientSwiftDepositOption()
        {
            string url = ApiPaths.CLIENTS_BASE_PATH + "/" + fixture.TestAccountIdForClientEndpoint + "/swift-deposit-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                ["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString()
            };

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == fixture.TestGroupForClientEndpoint.SwiftDepositEnabled);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "AssetClients")]
        [Trait("Category", "AssetClientsGet")]
        public async void GetClientCashInBankOption()
        {
            string url = ApiPaths.CLIENTS_BASE_PATH + "/" + fixture.TestAccountIdForClientEndpoint + "/cash-in-via-bank-card-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                ["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString()
            };

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == fixture.TestGroupForClientEndpoint.ClientsCanCashInViaBankCards);

        }
    }
}
