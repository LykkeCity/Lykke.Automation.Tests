using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.MatchingEngine.Connector.Abstractions.Models;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestData.Entities;

namespace AFTests.ApiV2
{
    public partial class ApiV2Tests
    {
        [Ignore("Test will fail due to ApiV2 issues")]
        [Test]
        [Category("Smoke")]
        [Category("TransactionHistory")]
        [Category("TransactionHistoryGet")]
        public async Task GetTransactionHistoryWhenThereIsNone()
        {
            var url = ApiPaths.TRANSACTION_HISTORY_BASE_PATH;

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.NoContent);
        }
        [Ignore("Test will fail due to ApiV2 issues")]
        [Test]
        [Category("Smoke")]
        [Category("TransactionHistory")]
        [Category("TransactionHistoryGet")]
        public async Task GetTransactionHistory()
        {
            //Get an account that has no transaction history
            var testAccount = (AccountEntity)await this.AccountRepository.TryGetAsync(
                this.TestClientId
            );
            Assert.NotNull(testAccount);

            var accountBalance = testAccount.BalancesParsed.FirstOrDefault(b => b.Asset == this.TestAssetId);
            Assert.NotNull(accountBalance);

            var realBallance = accountBalance.Balance - accountBalance.Reserved;

            //Try to make cash out
            var cashOutId = Guid.NewGuid().ToString();
            var cashOutAmmount = Math.Round((realBallance / 10) * -1, this.AssetPrecission);

            var meGoodCashOutResponse = await this.MEConsumer.Client.CashInOutAsync(
                cashOutId, testAccount.Id, accountBalance.Asset, cashOutAmmount);

            Assert.True(meGoodCashOutResponse.Status == MeStatusCodes.Ok);

            var url = ApiPaths.TRANSACTION_HISTORY_BASE_PATH;

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
        }
    }
}