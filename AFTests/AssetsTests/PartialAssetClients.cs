using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using System.Threading.Tasks;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetClients")]
        [Category("AssetClientsGet")]
        public async Task GetClientAssetIDs()
        {
            string url = ApiPaths.CLIENTS_BASE_PATH + "/" + this.TestAccountIdForClientEndpoint + "/asset-ids";
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                ["isIosDevice"] = this.TestGroupForClientEndpoint.IsIosDevice.ToString()
            };

            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<string> parsedResponse = JsonUtils.DeserializeJson<List<string>>(response.ResponseJson);
            parsedResponse.Should().Contain(this.TestAssetForClientEndpoint.Id);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetClients")]
        [Category("AssetClientsGet")]
        public async Task GetClientSwiftDepositOption()
        {
            string url = ApiPaths.CLIENTS_BASE_PATH + "/" + this.TestAccountIdForClientEndpoint + "/swift-deposit-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                ["isIosDevice"] = this.TestGroupForClientEndpoint.IsIosDevice.ToString()
            };

            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == this.TestGroupForClientEndpoint.SwiftDepositEnabled);

        }

        [Test]
        [Category("Smoke")]
        [Category("AssetClients")]
        [Category("AssetClientsGet")]
        public async Task GetClientCashInBankOption()
        {
            string url = ApiPaths.CLIENTS_BASE_PATH + "/" + this.TestAccountIdForClientEndpoint + "/cash-in-via-bank-card-enabled";
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                ["isIosDevice"] = this.TestGroupForClientEndpoint.IsIosDevice.ToString()
            };

            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);
            Assert.True(parsedResponse == this.TestGroupForClientEndpoint.ClientsCanCashInViaBankCards);

        }
    }
}
