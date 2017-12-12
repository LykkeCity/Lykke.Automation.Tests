using BalancesData.Fixtures;
using NUnit.Framework;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using RestSharp;
using BalancesData.DTOs;
using XUnitTestCommon.Utils;
using BalancesData;

namespace AFTests.Balances
{
    public class BalancesTests : BalancesTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("IsAlive")]
        [Category("IsAliveGet")]
        public async Task IsAlive()
        {
            string url = ApiPaths.BALANCES_IS_ALIVE;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);
        }

        [Test]
        [Category("Smoke")]
        [Category("WalletsClientBalances")]
        [Category("WalletsClientBalancesGet")]
        public async Task GetWalletBalance()
        {
            string url = $"{ApiPaths.BALANCES_WALLET_BALANCES}/{this.TestClient.Account.Id}";

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);

            List<BalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<BalanceDTO>>(response.ResponseJson);
            Assert.True(parsedResponse.Count == 1);
            Assert.True(parsedResponse[0].AssetId == Constants.BALANCES_ASSET_ID);
            Assert.True(parsedResponse[0].Balance == Constants.BALANCES_ASSET_AMOUNT);
            Assert.True(parsedResponse[0].Reserved == 0);
        }

        [Test]
        [Category("Smoke")]
        [Category("WalletsClientBalances")]
        [Category("WalletsClientBalancesGet")]
        public async Task GetWalletBalanceForAsset()
        {
            string url = $"{ApiPaths.BALANCES_WALLET_BALANCES}/{this.TestClient.Account.Id}/{Constants.BALANCES_ASSET_ID}";

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);

            BalanceDTO parsedResponse = JsonUtils.DeserializeJson<BalanceDTO>(response.ResponseJson);
            Assert.True(parsedResponse.AssetId == Constants.BALANCES_ASSET_ID);
            Assert.True(parsedResponse.Balance == Constants.BALANCES_ASSET_AMOUNT);
            Assert.True(parsedResponse.Reserved == 0);
        }
    }
}
