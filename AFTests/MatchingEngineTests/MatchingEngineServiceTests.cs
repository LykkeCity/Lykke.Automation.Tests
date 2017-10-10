using System;
using System.Collections.Generic;
using System.Text;
using AFTMatchingEngine.Fixtures;
using Xunit;
using Lykke.MatchingEngine.Connector.Abstractions.Models;
using XUnitTestData.Repositories;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using MatchingEngineData.DTOs.RabbitMQ;
using XUnitTestData.Repositories.MatchingEngine;

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
            BalanceDTO accountBalance = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();
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

            CashOperation message = (CashOperation)await fixture.WaitForRabbitMQ<CashOperation>(goodCashOutID);

            Assert.NotNull(message);

            Assert.Equal(message.clientId, testAccount.Id);
            Assert.Equal(message.asset, accountBalance.Asset);

            if (Double.TryParse(message.volume, out double parsedVolume))
            {
                Assert.Equal(parsedVolume, goodCashOutAmmount);
            }

            AccountEntity testAccountCheck = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO balanceCheck = testAccountCheck.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();

            Assert.True(Math.Round(balanceCheck.Balance, 2) == Math.Round(accountBalance.Balance + goodCashOutAmmount, 2)); // TODO get asset accuracy

            //Cash In test
            string cashInID = Guid.NewGuid().ToString();
            double cashInAmmount = goodCashOutAmmount * -1; //cash in the same ammount we cashed out
            MeResponseModel meCashInResponse = await fixture.Consumer.Client.CashInOutAsync(
                cashInID, testAccount.Id, fixture.TestAsset, cashInAmmount);

            Assert.True(meCashInResponse.Status == MeStatusCodes.Ok);

            message = (CashOperation)await fixture.WaitForRabbitMQ<CashOperation>(cashInID);

            Assert.NotNull(message);

            Assert.Equal(message.clientId, testAccount.Id);
            Assert.Equal(message.asset, accountBalance.Asset);

            if (Double.TryParse(message.volume, out parsedVolume))
            {
                Assert.Equal(parsedVolume, cashInAmmount);
            }


            //balances after cashout -> cashin with the same ammount should not differ

            AccountEntity testAccountAfter = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccountAfter);

            BalanceDTO accountBalanceAfter = testAccountAfter.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();
            Assert.NotNull(accountBalanceAfter);

            Assert.Equal(accountBalance.Balance, accountBalanceAfter.Balance);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        public async void CashTransfer()
        {
            Assert.NotNull(fixture.Consumer.Client);
            Assert.True(fixture.Consumer.Client.IsConnected);

            AccountEntity testAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount1);
            BalanceDTO accountBalance1 = testAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();
            Assert.NotNull(accountBalance1);

            AccountEntity testAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            Assert.NotNull(testAccount2);
            BalanceDTO accountBalance2 = testAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();
            Assert.NotNull(accountBalance2);

            //Attempt invalid transfer
            double badTransferAmount = accountBalance1.Balance + 0.7;
            string badTransferId = Guid.NewGuid().ToString();
            MeResponseModel badTransferResponse = await fixture.Consumer.Client.TransferAsync(badTransferId, testAccount1.Id, testAccount2.Id, fixture.TestAsset, badTransferAmount);

            Assert.NotNull(badTransferResponse);
            Assert.True(badTransferResponse.Status == MeStatusCodes.LowBalance);



            //Transfer from Test acc 1 to Test acc 2
            double transferAmount = accountBalance1.Balance / 10;
            string transferId = Guid.NewGuid().ToString();
            MeResponseModel transferResponse = await fixture.Consumer.Client.TransferAsync(transferId, testAccount1.Id, testAccount2.Id, fixture.TestAsset, transferAmount);

            Assert.NotNull(transferResponse);
            Assert.True(transferResponse.Status == MeStatusCodes.Ok);

            CashTransferOperation message = (CashTransferOperation)await fixture.WaitForRabbitMQ<CashTransferOperation>(transferId);
            Assert.NotNull(message);
            Assert.True(message.asset == fixture.TestAsset);
            Assert.True(message.fromClientId == testAccount1.Id);
            Assert.True(message.toClientId == testAccount2.Id);


            if (Double.TryParse(message.volume, out double parsedMsgAmount))
            {
                Assert.True(parsedMsgAmount == transferAmount);
            }

            CashSwapEntity checkCashSwapOperation = (CashSwapEntity)await fixture.CashSwapRepository.TryGetAsync(transferId);
            Assert.True(checkCashSwapOperation.Amount == transferAmount);
            Assert.True(checkCashSwapOperation.AssetId == fixture.TestAsset);
            Assert.True(checkCashSwapOperation.FromClientId == testAccount1.Id);
            Assert.True(checkCashSwapOperation.ToClientId == testAccount2.Id);

            AccountEntity checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();
            AccountEntity checkTestAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            BalanceDTO checkAccountBalance2 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance1.Balance, 2) == Math.Round(accountBalance1.Balance - transferAmount, 2)); // TODO get asset accuracy
            Assert.True(Math.Round(checkAccountBalance2.Balance, 2) == Math.Round(accountBalance2.Balance + transferAmount, 2)); // TODO get asset accuracy



            //Transfer same amount from Test acc 2 to Test acc 1
            string transferBackId = Guid.NewGuid().ToString();
            MeResponseModel transferBackResponse = await fixture.Consumer.Client.TransferAsync(transferBackId, testAccount2.Id, testAccount1.Id, fixture.TestAsset, transferAmount);

            Assert.NotNull(transferBackResponse);
            Assert.True(transferBackResponse.Status == MeStatusCodes.Ok);

            message = (CashTransferOperation)await fixture.WaitForRabbitMQ<CashTransferOperation>(transferBackId);
            Assert.NotNull(message);
            Assert.True(message.asset == fixture.TestAsset);
            Assert.True(message.fromClientId == testAccount2.Id);
            Assert.True(message.toClientId == testAccount1.Id);


            if (Double.TryParse(message.volume, out parsedMsgAmount))
            {
                Assert.True(parsedMsgAmount == transferAmount);
            }

            CashSwapEntity checkCashSwapBackOperation = (CashSwapEntity)await fixture.CashSwapRepository.TryGetAsync(transferBackId);
            Assert.True(checkCashSwapBackOperation.Amount == transferAmount);
            Assert.True(checkCashSwapBackOperation.AssetId == fixture.TestAsset);
            Assert.True(checkCashSwapBackOperation.FromClientId == testAccount2.Id);
            Assert.True(checkCashSwapBackOperation.ToClientId == testAccount1.Id);

            checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            checkAccountBalance1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();
            checkTestAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            checkAccountBalance2 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset).FirstOrDefault();

            //balances should be back to their initial state after 2 transfers back and forward
            Assert.True(accountBalance1.Balance == checkAccountBalance1.Balance);
            Assert.True(accountBalance2.Balance == checkAccountBalance2.Balance);

        }

        //[Fact]
        //[Trait("Category", "Smoke")]
        //public async void AddAsset()
        //{
        //    double ammount = 6.08;

        //    AccountEntity testAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
        //    MeResponseModel meGoodResponse = await fixture.Consumer.Client.CashInOutAsync(
        //        Guid.NewGuid().ToString(), testAccount1.Id, "LKK", ammount);

        //    Assert.True(meGoodResponse.Status == MeStatusCodes.Ok);
        //}
    }
}
