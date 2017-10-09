using System;
using System.Collections.Generic;
using System.Text;
using AFTMatchingEngine.Fixtures;
using Xunit;
using Lykke.MatchingEngine.Connector.Abstractions.Models;
using AFTMatchingEngine.DTOs.RabbitMQ;
using XUnitTestData.Repositories;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace AFTMatchingEngine
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "MatchingEngine")]
    public class MatchingEngineServiceTests : IClassFixture<MatchingEngineTestDataFixture>
    {
        private MatchingEngineTestDataFixture fixture;

        public MatchingEngineServiceTests(MatchingEngineTestDataFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        [Trait("Category", "Smoke")]
        public async void CashInOut()
        {
            Assert.NotNull(fixture.Consumer.Client);
            Assert.True(fixture.Consumer.Client.IsConnected);

            AccountEntity testAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount);

            BalanceDTO accountBalance = testAccount.BalancesParsed.Where(b => b.Asset == "LKK").FirstOrDefault();
            Assert.NotNull(accountBalance);

            double realBallance = accountBalance.Balance - accountBalance.Reserved;



            // Low Balance cashout test
            double badCashOutAmmount = (realBallance + 0.1) * -1;

            MeResponseModel meBadCashOutResponse = await fixture.Consumer.Client.CashInOutAsync(
                Guid.NewGuid().ToString(), testAccount.Id, accountBalance.Asset, badCashOutAmmount);


            Assert.True(meBadCashOutResponse.Status == MeStatusCodes.LowBalance);



            // Good cashout test
            string goodCashOutID = Guid.NewGuid().ToString();

            double goodCashOutAmmount = (realBallance / 10) * -1;

            MeResponseModel meGoodCashOutResponse = await fixture.Consumer.Client.CashInOutAsync(
                goodCashOutID, testAccount.Id, accountBalance.Asset, goodCashOutAmmount);

            Assert.True(meGoodCashOutResponse.Status == MeStatusCodes.Ok);

            CashOperation message =  await fixture.WaitForRabbitMQ(goodCashOutID);

            Assert.NotNull(message);

            Assert.Equal(message.clientId, testAccount.Id);
            Assert.Equal(message.asset, accountBalance.Asset);

            double parsedVolume = -999999;
            if (Double.TryParse(message.volume, out parsedVolume))
            {
                Assert.Equal(parsedVolume, goodCashOutAmmount);
            }


            //Cash In test
            double cashInAmmount = goodCashOutAmmount * -1; //cash in the same ammount we cashed out
            MeResponseModel meCashInResponse = await fixture.Consumer.Client.CashInOutAsync(
                goodCashOutID, testAccount.Id, accountBalance.Asset, goodCashOutAmmount);


            message = await fixture.WaitForRabbitMQ(goodCashOutID);

            Assert.NotNull(message);

            Assert.Equal(message.clientId, testAccount.Id);
            Assert.Equal(message.asset, accountBalance.Asset);

            parsedVolume = -999999;
            if (Double.TryParse(message.volume, out parsedVolume))
            {
                Assert.Equal(parsedVolume, cashInAmmount);
            }


            //balances after cashout -> cashin with the same ammount should not differ

            AccountEntity testAccountAfter = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccountAfter);

            BalanceDTO accountBalanceAfter = testAccount.BalancesParsed.Where(b => b.Asset == "LKK").FirstOrDefault();
            Assert.NotNull(accountBalanceAfter);

            Assert.Equal(accountBalance.Balance, accountBalanceAfter.Balance);

        }

        //[Fact]
        //[Trait("Category", "Smoke")]
        //public async void AddAsset()
        //{
        //    double ammount = 9.16;

        //    AccountEntity testAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
        //    MeResponseModel meGoodResponse = await fixture.Consumer.Client.CashInOutAsync(
        //        Guid.NewGuid().ToString(), testAccount1.Id, "LKK", ammount);

        //    Assert.True(meGoodResponse.Status == MeStatusCodes.Ok);
        //}
    }
}
