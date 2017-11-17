using System;
using System.Linq;
using System.Net;
using Lykke.MatchingEngine.Connector.Abstractions.Models;
using RestSharp;
using Xunit;
using XUnitTestCommon;
using XUnitTestData.Repositories;

namespace AFTests.ApiV2
{
    public partial class ApiV2Tests
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "TransactionHistory")]
        [Trait("Category", "TransactionHistoryGet")]
        public async void GetTransactionHistoryWhenThereIsNone()
        {
            var url = ApiPaths.TRANSACTION_HISTORY_BASE_PATH;

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.NoContent);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "TransactionHistory")]
        [Trait("Category", "TransactionHistoryGet")]
        public async void GetTransactionHistory()
        {
            //Get an account that has no transaction history
            var testAccount = (AccountEntity)await _matchingEngineFixture.AccountRepository.TryGetAsync(
                _fixture.TestClientId
            );
            Assert.NotNull(testAccount);

            var accountBalance = testAccount.BalancesParsed.FirstOrDefault(b => b.Asset == _matchingEngineFixture.TestAsset1);
            Assert.NotNull(accountBalance);

            var realBallance = accountBalance.Balance - accountBalance.Reserved;

            //Try to make cash out
            var cashOutId = Guid.NewGuid().ToString();

            var cashOutAmmount = Math.Round((realBallance / 10) * -1, _matchingEngineFixture.AssetPrecission);

            var meGoodCashOutResponse = await _matchingEngineFixture.Consumer.Client.CashInOutAsync(
                cashOutId, testAccount.Id, accountBalance.Asset, cashOutAmmount);

            Assert.True(meGoodCashOutResponse.Status == MeStatusCodes.Ok);

            var url = ApiPaths.TRANSACTION_HISTORY_BASE_PATH;

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
        }
    }
}