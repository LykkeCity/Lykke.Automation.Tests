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
using XUnitTestData.Repositories.Assets;
using XUnitTestCommon;

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
            BalanceDTO accountBalance = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
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

            CashOperation message = (CashOperation)await fixture.WaitForRabbitMQ<CashOperation>(o => o.id == goodCashOutID);

            Assert.NotNull(message);

            Assert.Equal(message.clientId, testAccount.Id);
            Assert.Equal(message.asset, accountBalance.Asset);

            if (Double.TryParse(message.volume, out double parsedVolume))
            {
                Assert.Equal(parsedVolume, goodCashOutAmmount);
            }

            AccountEntity testAccountCheck = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO balanceCheck = testAccountCheck.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(Math.Round(balanceCheck.Balance, 2) == Math.Round(accountBalance.Balance + goodCashOutAmmount, 2)); // TODO get asset accuracy

            //Cash In test
            string cashInID = Guid.NewGuid().ToString();
            double cashInAmmount = goodCashOutAmmount * -1; //cash in the same ammount we cashed out
            MeResponseModel meCashInResponse = await fixture.Consumer.Client.CashInOutAsync(
                cashInID, testAccount.Id, fixture.TestAsset1, cashInAmmount);

            Assert.True(meCashInResponse.Status == MeStatusCodes.Ok);

            message = (CashOperation)await fixture.WaitForRabbitMQ<CashOperation>(o => o.id == cashInID);

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

            BalanceDTO accountBalanceAfter = testAccountAfter.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
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
            BalanceDTO accountBalance1 = testAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance1);

            AccountEntity testAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            Assert.NotNull(testAccount2);
            BalanceDTO accountBalance2 = testAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance2);

            //Attempt invalid transfer
            double badTransferAmount = accountBalance1.Balance + 0.7;
            string badTransferId = Guid.NewGuid().ToString();
            MeResponseModel badTransferResponse = await fixture.Consumer.Client.TransferAsync(badTransferId, testAccount1.Id, testAccount2.Id, fixture.TestAsset1, badTransferAmount);

            Assert.NotNull(badTransferResponse);
            Assert.True(badTransferResponse.Status == MeStatusCodes.LowBalance);



            //Transfer from Test acc 1 to Test acc 2
            double transferAmount = Math.Round(accountBalance1.Balance / 10, 2);
            string transferId = Guid.NewGuid().ToString();
            MeResponseModel transferResponse = await fixture.Consumer.Client.TransferAsync(transferId, testAccount1.Id, testAccount2.Id, fixture.TestAsset1, transferAmount);

            Assert.NotNull(transferResponse);
            Assert.True(transferResponse.Status == MeStatusCodes.Ok);

            CashTransferOperation message = (CashTransferOperation)await fixture.WaitForRabbitMQ<CashTransferOperation>(o => o.id == transferId);
            Assert.NotNull(message);
            Assert.True(message.asset == fixture.TestAsset1);
            Assert.True(message.fromClientId == testAccount1.Id);
            Assert.True(message.toClientId == testAccount2.Id);
            if (Double.TryParse(message.volume, out double parsedMsgAmount))
                Assert.True(parsedMsgAmount == transferAmount);

            CashSwapEntity checkCashSwapOperation = (CashSwapEntity)await fixture.CashSwapRepository.TryGetAsync(transferId);
            Assert.True(checkCashSwapOperation.Amount == transferAmount);
            Assert.True(checkCashSwapOperation.AssetId == fixture.TestAsset1);
            Assert.True(checkCashSwapOperation.FromClientId == testAccount1.Id);
            Assert.True(checkCashSwapOperation.ToClientId == testAccount2.Id);

            AccountEntity checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            AccountEntity checkTestAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            BalanceDTO checkAccountBalance2 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance1.Balance, 2) == Math.Round(accountBalance1.Balance - transferAmount, 2)); // TODO get asset accuracy
            Assert.True(Math.Round(checkAccountBalance2.Balance, 2) == Math.Round(accountBalance2.Balance + transferAmount, 2)); // TODO get asset accuracy



            //Transfer same amount from Test acc 2 to Test acc 1
            string transferBackId = Guid.NewGuid().ToString();
            MeResponseModel transferBackResponse = await fixture.Consumer.Client.TransferAsync(transferBackId, testAccount2.Id, testAccount1.Id, fixture.TestAsset1, transferAmount);

            Assert.NotNull(transferBackResponse);
            Assert.True(transferBackResponse.Status == MeStatusCodes.Ok);

            message = (CashTransferOperation)await fixture.WaitForRabbitMQ<CashTransferOperation>(o => o.id == transferBackId);
            Assert.NotNull(message);
            Assert.True(message.asset == fixture.TestAsset1);
            Assert.True(message.fromClientId == testAccount2.Id);
            Assert.True(message.toClientId == testAccount1.Id);
            if (Double.TryParse(message.volume, out parsedMsgAmount))
                Assert.True(parsedMsgAmount == transferAmount);

            CashSwapEntity checkCashSwapBackOperation = (CashSwapEntity)await fixture.CashSwapRepository.TryGetAsync(transferBackId);
            Assert.True(checkCashSwapBackOperation.Amount == transferAmount);
            Assert.True(checkCashSwapBackOperation.AssetId == fixture.TestAsset1);
            Assert.True(checkCashSwapBackOperation.FromClientId == testAccount2.Id);
            Assert.True(checkCashSwapBackOperation.ToClientId == testAccount1.Id);

            checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            checkAccountBalance1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            checkTestAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            checkAccountBalance2 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            //balances should be back to their initial state after 2 transfers back and forward
            Assert.True(accountBalance1.Balance == checkAccountBalance1.Balance);
            Assert.True(accountBalance2.Balance == checkAccountBalance2.Balance);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        public async void CashSwap()
        {
            Assert.NotNull(fixture.Consumer.Client);
            Assert.True(fixture.Consumer.Client.IsConnected);

            AccountEntity testAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount1);
            BalanceDTO accountBalance1Asset1 = testAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance1Asset1);
            BalanceDTO accountBalance1Asset2 = testAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();
            Assert.NotNull(accountBalance1Asset2);

            AccountEntity testAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            Assert.NotNull(testAccount2);
            BalanceDTO accountBalance2Asset1 = testAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance2Asset1);
            BalanceDTO accountBalance2Asset2 = testAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();
            Assert.NotNull(accountBalance2Asset2);


            //Attempt invalid swap
            double badSwapAmount1 = accountBalance1Asset1.Balance + 0.7;
            double badSwapAmount2 = accountBalance2Asset2.Balance - 0.2; // this one isn't bad, but the transaction should still fail
            string badSwapId = Guid.NewGuid().ToString();

            MeResponseModel badSwapResponse = await fixture.Consumer.Client.SwapAsync(badSwapId,
                testAccount1.Id, fixture.TestAsset1, badSwapAmount1,
                testAccount2.Id, fixture.TestAsset2, badSwapAmount2);

            Assert.True(badSwapResponse.Status == MeStatusCodes.LowBalance);


            //Attempt a valid swap
            double swapAmount1 = Math.Round(accountBalance1Asset1.Balance / 10, 2); //TODO
            double swapAmount2 = Math.Round(accountBalance2Asset2.Balance / 10, 2); //TODO
            string swapId = Guid.NewGuid().ToString();

            MeResponseModel swapReseponse = await fixture.Consumer.Client.SwapAsync(swapId,
                testAccount1.Id, fixture.TestAsset1, swapAmount1,
                testAccount2.Id, fixture.TestAsset2, swapAmount2);

            Assert.True(swapReseponse.Status == MeStatusCodes.Ok);

            CashSwapOperation message = (CashSwapOperation)await fixture.WaitForRabbitMQ<CashSwapOperation>(o => o.id == swapId);

            Assert.True(message.asset1 == fixture.TestAsset1);
            Assert.True(message.asset2 == fixture.TestAsset2);
            Assert.True(message.clientId1 == testAccount1.Id);
            Assert.True(message.clientId2 == testAccount2.Id);
            if (Double.TryParse(message.volume1, out double parsedVolume))
                Assert.True(parsedVolume == swapAmount1);
            if (Double.TryParse(message.volume2, out parsedVolume))
                Assert.True(parsedVolume == swapAmount2);

            CashSwapEntity checkCashSwapOperation = (CashSwapEntity)await fixture.CashSwapRepository.TryGetAsync(swapId);

            Assert.True(checkCashSwapOperation.AssetId1 == fixture.TestAsset1);
            Assert.True(checkCashSwapOperation.AssetId2 == fixture.TestAsset2);
            Assert.True(checkCashSwapOperation.ClientId1 == testAccount1.Id);
            Assert.True(checkCashSwapOperation.ClientId2 == testAccount2.Id);
            Assert.True(checkCashSwapOperation.Amount1 == swapAmount1);
            Assert.True(checkCashSwapOperation.Amount2 == swapAmount2);

            AccountEntity checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance1Asset1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            BalanceDTO checkAccountBalance1Asset2 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            AccountEntity checkTestAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            BalanceDTO checkAccountBalance2Asset1 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            BalanceDTO checkAccountBalance2Asset2 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance1Asset1.Balance, 2) == Math.Round(accountBalance1Asset1.Balance - swapAmount1, 2)); //TODO get asset accuracy
            Assert.True(Math.Round(checkAccountBalance2Asset1.Balance, 2) == Math.Round(accountBalance2Asset1.Balance + swapAmount1, 2)); //TODO get asset accuracy
            Assert.True(Math.Round(checkAccountBalance1Asset2.Balance, 2) == Math.Round(accountBalance1Asset2.Balance + swapAmount2, 2)); //TODO get asset accuracy
            Assert.True(Math.Round(checkAccountBalance2Asset2.Balance, 2) == Math.Round(accountBalance2Asset2.Balance - swapAmount2, 2)); //TODO get asset accuracy



            // Attempt swap back
            string swapBackId = Guid.NewGuid().ToString();

            MeResponseModel swapBackReseponse = await fixture.Consumer.Client.SwapAsync(swapBackId,
                testAccount2.Id, fixture.TestAsset1, swapAmount1,
                testAccount1.Id, fixture.TestAsset2, swapAmount2);

            Assert.True(swapBackReseponse.Status == MeStatusCodes.Ok);

            message = (CashSwapOperation)await fixture.WaitForRabbitMQ<CashSwapOperation>(o => o.id == swapBackId);

            Assert.True(message.asset1 == fixture.TestAsset1);
            Assert.True(message.asset2 == fixture.TestAsset2);
            Assert.True(message.clientId1 == testAccount2.Id);
            Assert.True(message.clientId2 == testAccount1.Id);
            if (Double.TryParse(message.volume1, out parsedVolume))
                Assert.True(parsedVolume == swapAmount1);
            if (Double.TryParse(message.volume2, out parsedVolume))
                Assert.True(parsedVolume == swapAmount2);

            checkCashSwapOperation = (CashSwapEntity)await fixture.CashSwapRepository.TryGetAsync(swapBackId);

            Assert.True(checkCashSwapOperation.AssetId1 == fixture.TestAsset1);
            Assert.True(checkCashSwapOperation.AssetId2 == fixture.TestAsset2);
            Assert.True(checkCashSwapOperation.ClientId1 == testAccount2.Id);
            Assert.True(checkCashSwapOperation.ClientId2 == testAccount1.Id);
            Assert.True(checkCashSwapOperation.Amount1 == swapAmount1);
            Assert.True(checkCashSwapOperation.Amount2 == swapAmount2);

            checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            checkAccountBalance1Asset1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            checkAccountBalance1Asset2 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            checkTestAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
            checkAccountBalance2Asset1 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            checkAccountBalance2Asset2 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            // balances should be back to their initial state
            Assert.True(accountBalance1Asset1.Balance == checkAccountBalance1Asset1.Balance);
            Assert.True(accountBalance1Asset2.Balance == checkAccountBalance1Asset2.Balance);
            Assert.True(accountBalance2Asset1.Balance == checkAccountBalance2Asset1.Balance);
            Assert.True(accountBalance2Asset2.Balance == checkAccountBalance2Asset2.Balance);

        }

        [Fact]
        [Trait("Category", "Smoke")]
        public async void UpdateBalance()
        {
            Assert.NotNull(fixture.Consumer.Client);
            Assert.True(fixture.Consumer.Client.IsConnected);

            AccountEntity testAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount);
            BalanceDTO accountBalance = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance);



            //Execute balance update
            double newBalance = accountBalance.Balance + 1.0;
            string balanceUpdateId = Guid.NewGuid().ToString();
            await fixture.Consumer.Client.UpdateBalanceAsync(balanceUpdateId, fixture.TestAccountId1, fixture.TestAsset1, newBalance);

            BalanceUpdate message = (BalanceUpdate)await fixture.WaitForRabbitMQ<BalanceUpdate>(o => o.id == balanceUpdateId);
            Assert.True(message.type == "BALANCE_UPDATE");

            BalanceUpdate.ClientBalanceUpdate balance = message.balances.Where(m => m.id == fixture.TestAccountId1).FirstOrDefault();
            Assert.True(balance != null);
            Assert.True(balance.asset == fixture.TestAsset1);
            Assert.True(balance.oldBalance == accountBalance.Balance);
            Assert.True(balance.newBalance == newBalance);
            Assert.True(balance.oldReserved == balance.newReserved);

            AccountEntity checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(checkAccountBalance.Balance == newBalance);

            //reverse balance update
            string reverseBalanceUpdateId = Guid.NewGuid().ToString();
            await fixture.Consumer.Client.UpdateBalanceAsync(reverseBalanceUpdateId, fixture.TestAccountId1, fixture.TestAsset1, accountBalance.Balance);

            message = (BalanceUpdate)await fixture.WaitForRabbitMQ<BalanceUpdate>(o => o.id == reverseBalanceUpdateId);
            Assert.True(message.type == "BALANCE_UPDATE");

            balance = message.balances.Where(m => m.id == fixture.TestAccountId1).FirstOrDefault();
            Assert.True(balance != null);
            Assert.True(balance.asset == fixture.TestAsset1);
            Assert.True(balance.oldBalance == newBalance);
            Assert.True(balance.newBalance == accountBalance.Balance);
            Assert.True(balance.oldReserved == balance.newReserved);

            checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(checkAccountBalance.Balance == accountBalance.Balance);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "LimitOrders")]
        public async void SellLimitOrderTest() // and CancelLimitOrder
        {
            AccountEntity testAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount);
            BalanceDTO accountBalance = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance);

            string limitOrderID = Guid.NewGuid().ToString();
            string badLimitOrderID = Guid.NewGuid().ToString();

            Random random = new Random();
            double amount = 0.2;
            double price = random.Next(100, 999);

            //Attempt bad sell
            MeResponseModel badOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
                badLimitOrderID, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Sell, accountBalance.Balance + 10, price);
            Assert.False(badOrderResponse.Status == MeStatusCodes.Ok);

            LimitOrdersResponse badMessage = (LimitOrdersResponse)await fixture.WaitForRabbitMQ<LimitOrdersResponse>(
                o => o.orders.Any(m => m.order.externalId == badLimitOrderID && m.order.status == "NotEnoughFunds"));
            Assert.NotNull(badMessage);

            LimitOrders badSubMessage = badMessage.orders.Where(m => m.order.externalId == badLimitOrderID && m.order.status == "NotEnoughFunds").FirstOrDefault();
            Assert.NotNull(badSubMessage);
            Assert.True(badSubMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(badSubMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(badSubMessage.order.volume == (accountBalance.Balance + 10) * -1);
            Assert.True(badSubMessage.order.price == price);


            //Attempt proper sell
            MeResponseModel LimitOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
                limitOrderID, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Sell, amount, price);
            Assert.True(LimitOrderResponse.Status == MeStatusCodes.Ok);

            AccountEntity checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance.Reserved, 2) == Math.Round(accountBalance.Reserved + amount, 2));


            LimitOrdersResponse message = (LimitOrdersResponse)await fixture.WaitForRabbitMQ<LimitOrdersResponse>(
                o => o.orders.Any(m => m.order.externalId == limitOrderID && m.order.status == "InOrderBook"));
            Assert.NotNull(message);

            LimitOrders subMessage = message.orders.Where(m => m.order.externalId == limitOrderID && m.order.status == "InOrderBook").FirstOrDefault();
            Assert.NotNull(subMessage);
            Assert.True(subMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(subMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(subMessage.order.volume == amount * -1);
            Assert.True(subMessage.order.price == price);



            //Cancel the proper sell
            MeResponseModel LimitOrderCancelResponse = await fixture.Consumer.Client.CancelLimitOrderAsync(limitOrderID);
            Assert.True(LimitOrderCancelResponse.Status == MeStatusCodes.Ok);

            checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(checkAccountBalance.Reserved == accountBalance.Reserved);

            LimitOrdersResponse cancelMessage = (LimitOrdersResponse)await fixture.WaitForRabbitMQ<LimitOrdersResponse>(
                o => o.orders.Any(m => m.order.externalId == limitOrderID && m.order.status == "Cancelled"));
            Assert.NotNull(cancelMessage);

            LimitOrders cancelSubMessage = cancelMessage.orders.Where(m => m.order.externalId == limitOrderID && m.order.status == "Cancelled").FirstOrDefault();
            Assert.NotNull(cancelSubMessage);
            Assert.True(cancelSubMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(cancelSubMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(cancelSubMessage.order.volume == amount * -1);
            Assert.True(cancelSubMessage.order.price == price);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "LimitOrders")]
        public async void BuyLimitOrderTest() // and CancelLimitOrder
        {
            AccountEntity testAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount);
            BalanceDTO accountBalance = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();
            Assert.NotNull(accountBalance);

            string limitOrderID = Guid.NewGuid().ToString();
            string badLimitOrderID = Guid.NewGuid().ToString();

            Random random = new Random();
            double amount = 0.01;
            double price = random.NextDouble() / 100;

            //Attempt bad buy
            MeResponseModel badOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
                badLimitOrderID, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Buy, accountBalance.Balance + 10, 2);
            Assert.False(badOrderResponse.Status == MeStatusCodes.Ok);

            LimitOrdersResponse badMessage = (LimitOrdersResponse)await fixture.WaitForRabbitMQ<LimitOrdersResponse>(
                o => o.orders.Any(m => m.order.externalId == badLimitOrderID && m.order.status == "NotEnoughFunds"));
            Assert.NotNull(badMessage);

            LimitOrders badSubMessage = badMessage.orders.Where(m => m.order.externalId == badLimitOrderID && m.order.status == "NotEnoughFunds").FirstOrDefault();
            Assert.NotNull(badSubMessage);
            Assert.True(badSubMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(badSubMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(badSubMessage.order.volume == accountBalance.Balance + 10);
            //Assert.True(badSubMessage.order.price == price);


            //Attempt proper buy
            MeResponseModel LimitOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
                limitOrderID, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Buy, amount, price);
            Assert.True(LimitOrderResponse.Status == MeStatusCodes.Ok);

            AccountEntity checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance.Reserved, 2) > Math.Round(accountBalance.Reserved, 2));//TODO calculate price


            LimitOrdersResponse message = (LimitOrdersResponse)await fixture.WaitForRabbitMQ<LimitOrdersResponse>(
                o => o.orders.Any(m => m.order.externalId == limitOrderID && m.order.status == "InOrderBook"));
            Assert.NotNull(message);

            LimitOrders subMessage = message.orders.Where(m => m.order.externalId == limitOrderID && m.order.status == "InOrderBook").FirstOrDefault();
            Assert.NotNull(subMessage);
            Assert.True(subMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(subMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(subMessage.order.volume == amount);
            Assert.True(subMessage.order.price == price);


            //Cancel proper buy
            MeResponseModel LimitOrderCancelResponse = await fixture.Consumer.Client.CancelLimitOrderAsync(limitOrderID);
            Assert.True(LimitOrderCancelResponse.Status == MeStatusCodes.Ok);

            checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            Assert.True(checkAccountBalance.Reserved == accountBalance.Reserved);

            LimitOrdersResponse cancelMessage = (LimitOrdersResponse)await fixture.WaitForRabbitMQ<LimitOrdersResponse>(
                o => o.orders.Any(m => m.order.externalId == limitOrderID && m.order.status == "Cancelled"));
            Assert.NotNull(cancelMessage);

            LimitOrders cancelSubMessage = cancelMessage.orders.Where(m => m.order.externalId == limitOrderID && m.order.status == "Cancelled").FirstOrDefault();
            Assert.NotNull(cancelSubMessage);
            Assert.True(cancelSubMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(cancelSubMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(cancelSubMessage.order.volume == amount);
            Assert.True(cancelSubMessage.order.price == price);

        }

        //[Fact]
        //[Trait("Category", "Smoke")]
        //[Trait("Category", "LimitOrders")]
        //public async void HandleMarketOrder()
        //{
        //    string marketOrderId = Guid.NewGuid().ToString();
        //    double volume = 0.5;
        //    string marketOrderResponse = await fixture.Consumer.Client.HandleMarketOrderAsync(marketOrderId, fixture.TestAssetPair.Id, OrderAction.Buy, volume, true);
        //    Assert.NotNull(marketOrderResponse);
        //}

        //[Fact]
        //[Trait("Category", "Smoke")]
        //[Trait("Category", "LimitOrders")]
        //public async void SellBuyLimitOrderTest() //transaction between the 2 test accoutns
        //{
        //    AccountEntity testAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
        //    Assert.NotNull(testAccount1);
        //    BalanceDTO accountBalance1 = testAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();
        //    Assert.NotNull(accountBalance1);

        //    AccountEntity testAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
        //    Assert.NotNull(testAccount2);
        //    BalanceDTO accountBalance2 = testAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
        //    Assert.NotNull(accountBalance2);

        //    string limitOrderIDBuy = Guid.NewGuid().ToString();
        //    string limitOrderIDSell = Guid.NewGuid().ToString();

        //    //Random random = new Random();
        //    double amount = 0.1;
        //    //double buyPrice = 0.01;//random.NextDouble() / 100;
        //    //double buyvalue = Math.Round(amount * buyPrice, 2); //todo;
        //    double sellPrice = 100; //random.Next(100,900);
        //    //double sellvalue = amount * sellPrice;

        //    MeResponseModel SellLimitOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
        //        limitOrderIDSell, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Sell, amount, sellPrice, true);

        //    Assert.True(SellLimitOrderResponse.Status == MeStatusCodes.Ok);

        //    //AccountEntity checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
        //    //BalanceDTO checkAccountBalance1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

        //    //Assert.True(Math.Round(checkAccountBalance1.Reserved, 2) == Math.Round(accountBalance1.Reserved + amount, 2)); // todo

        //    Thread.Sleep(1000);

        //    MeResponseModel BuyLimitOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
        //        limitOrderIDBuy, fixture.TestAccountId2, fixture.TestAssetPair.Id, OrderAction.Buy, amount, sellPrice + 10, true);

        //    Assert.True(BuyLimitOrderResponse.Status == MeStatusCodes.Ok);

        //    //MeResponseModel HandleLimitOrderResponse = await fixture.Consumer.Client.HandleMarketOrderAsync()

        //    //checkTestAccount1 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
        //    //checkAccountBalance1 = checkTestAccount1.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

        //    //AccountEntity checkTestAccount2 = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId2);
        //    //BalanceDTO checkAccountBalance2 = checkTestAccount2.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

        //    //Assert.True(Math.Round(checkAccountBalance1.Reserved, 2) == Math.Round(accountBalance1.Reserved + amount, 2)); // todo
        //    //Assert.True(Math.Round(checkAccountBalance2.Balance, 2) == Math.Round(accountBalance2.Balance - buyprice, 2)); // todo

        //}

        //[Fact]
        //[Trait("Category", "Smoke")]
        //public async void AddAsset()
        //{
        //    await fixture.Consumer.Client.UpdateBalanceAsync(
        //        Guid.NewGuid().ToString(), fixture.TestAccountId1, fixture.TestAsset2, 106.65);

        //    await fixture.Consumer.Client.UpdateBalanceAsync(
        //        Guid.NewGuid().ToString(), fixture.TestAccountId2, fixture.TestAsset2, 103.31);

        //}
    }
}
