using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest : AssetsTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetClients")]
        [Category("AssetClientsGet")]
        public async void GetClientAssetIDs()
        {
            string url = fixture.ApiEndpointNames["assetClients"] + "/" + fixture.TestAccountIdForClientEndpoint + "/asset-ids";
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString();

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            Assert.True(parsedResponse[0] == fixture.TestAssetForClientEndpoint.Id);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetClients")]
        [Category("AssetClientsGet")]
        public async void GetClientSwiftDepositOption()
        {
            string url = fixture.ApiEndpointNames["assetClients"] + "/" + fixture.TestAccountIdForClientEndpoint + "/swift-deposit-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString();

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == fixture.TestGroupForClientEndpoint.SwiftDepositEnabled);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetClients")]
        [Category("AssetClientsGet")]
        public async void GetClientCashInBankOption()
        {
            string url = fixture.ApiEndpointNames["assetClients"] + "/" + fixture.TestAccountIdForClientEndpoint + "/cash-in-via-bank-card-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams["isIosDevice"] = fixture.TestGroupForClientEndpoint.IsIosDevice.ToString();

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == fixture.TestGroupForClientEndpoint.ClientsCanCashInViaBankCards);

        }
    }
}
