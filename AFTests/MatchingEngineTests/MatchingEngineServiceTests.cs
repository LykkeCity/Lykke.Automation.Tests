using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
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

            double goodCashOutAmmount = Math.Round((realBallance / 10) * -1, fixture.AssetPrecission);

            MeResponseModel meGoodCashOutResponse = await fixture.Consumer.Client.CashInOutAsync(
                goodCashOutID, testAccount.Id, accountBalance.Asset, goodCashOutAmmount);

            Assert.True(meGoodCashOutResponse.Status == MeStatusCodes.Ok);

            CashOperation message = (CashOperation)await fixture.WaitForRabbitMQ<CashOperation>(o => o.id == goodCashOutID);

            Assert.NotNull(message);

            Assert.Equal(message.clientId, testAccount.Id);
            Assert.Equal(message.asset, accountBalance.Asset);

            if (Double.TryParse(message.volume, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedVolume))
            {
                Assert.Equal(parsedVolume, goodCashOutAmmount);
            }

            AccountEntity testAccountCheck = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO balanceCheck = testAccountCheck.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(Math.Round(balanceCheck.Balance, fixture.AssetPrecission) == Math.Round(accountBalance.Balance + goodCashOutAmmount, fixture.AssetPrecission)); // TODO get asset accuracy

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

            if (Double.TryParse(message.volume, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedVolume))
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
            double transferAmount = Math.Round(accountBalance1.Balance / 10, fixture.AssetPrecission);
            string transferId = Guid.NewGuid().ToString();
            MeResponseModel transferResponse = await fixture.Consumer.Client.TransferAsync(transferId, testAccount1.Id, testAccount2.Id, fixture.TestAsset1, transferAmount);

            Assert.NotNull(transferResponse);
            Assert.True(transferResponse.Status == MeStatusCodes.Ok);

            CashTransferOperation message = (CashTransferOperation)await fixture.WaitForRabbitMQ<CashTransferOperation>(o => o.id == transferId);
            Assert.NotNull(message);
            Assert.True(message.asset == fixture.TestAsset1);
            Assert.True(message.fromClientId == testAccount1.Id);
            Assert.True(message.toClientId == testAccount2.Id);
            if (Double.TryParse(message.volume, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedMsgAmount))
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

            Assert.True(Math.Round(checkAccountBalance1.Balance, fixture.AssetPrecission) == Math.Round(accountBalance1.Balance - transferAmount, fixture.AssetPrecission)); // TODO get asset accuracy
            Assert.True(Math.Round(checkAccountBalance2.Balance, fixture.AssetPrecission) == Math.Round(accountBalance2.Balance + transferAmount, fixture.AssetPrecission)); // TODO get asset accuracy



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
            if (Double.TryParse(message.volume, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedMsgAmount))
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
            double swapAmount1 = Math.Round(accountBalance1Asset1.Balance / 10, fixture.AssetPrecission);
            double swapAmount2 = Math.Round(accountBalance2Asset2.Balance / 10, fixture.AssetPrecission);
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
            if (Double.TryParse(message.volume1, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedVolume))
                Assert.True(parsedVolume == swapAmount1);
            if (Double.TryParse(message.volume2, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedVolume))
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

            Assert.True(Math.Round(checkAccountBalance1Asset1.Balance, fixture.AssetPrecission) == Math.Round(accountBalance1Asset1.Balance - swapAmount1, fixture.AssetPrecission));
            Assert.True(Math.Round(checkAccountBalance2Asset1.Balance, fixture.AssetPrecission) == Math.Round(accountBalance2Asset1.Balance + swapAmount1, fixture.AssetPrecission));
            Assert.True(Math.Round(checkAccountBalance1Asset2.Balance, fixture.AssetPrecission) == Math.Round(accountBalance1Asset2.Balance + swapAmount2, fixture.AssetPrecission));
            Assert.True(Math.Round(checkAccountBalance2Asset2.Balance, fixture.AssetPrecission) == Math.Round(accountBalance2Asset2.Balance - swapAmount2, fixture.AssetPrecission));



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
            if (Double.TryParse(message.volume1, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedVolume))
                Assert.True(parsedVolume == swapAmount1);
            if (Double.TryParse(message.volume2, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedVolume))
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
        public async void LimitOrderSell() // and CancelLimitOrder
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
            double matchedPrice = random.NextDouble();

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

            //Attempt sell stored in order book
            MeResponseModel LimitOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
                limitOrderID, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Sell, amount, price);
            Assert.True(LimitOrderResponse.Status == MeStatusCodes.Ok);

            AccountEntity checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance.Reserved, fixture.AssetPrecission) == Math.Round(accountBalance.Reserved + amount, fixture.AssetPrecission));


            LimitOrdersResponse message = (LimitOrdersResponse)await fixture.WaitForRabbitMQ<LimitOrdersResponse>(
                o => o.orders.Any(m => m.order.externalId == limitOrderID && m.order.status == "InOrderBook"));
            Assert.NotNull(message);

            LimitOrders subMessage = message.orders.Where(m => m.order.externalId == limitOrderID && m.order.status == "InOrderBook").FirstOrDefault();
            Assert.NotNull(subMessage);
            Assert.True(subMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(subMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(subMessage.order.volume == amount * -1);
            Assert.True(subMessage.order.price == price);


            //LimitOrderEntity limitOrderFromDB = (LimitOrderEntity)await fixture.LimitOrdersRepository.TryGetAsync("Active_" + fixture.TestAccountId1, subMessage.order.id);
            //Assert.NotNull(limitOrderFromDB);
            //Assert.True(limitOrderFromDB.AssetPairId == fixture.TestAssetPair.Id);
            //Assert.True(limitOrderFromDB.Price == subMessage.order.price);
            //Assert.True(limitOrderFromDB.Status == subMessage.order.status);
            //Assert.True(limitOrderFromDB.Volume == amount);
            //Assert.True(limitOrderFromDB.RemainingVolume == subMessage.order.remainingVolume);
            //Assert.True(limitOrderFromDB.Straight == );
            //Assert.True(limitOrderFromDB.MatchingId == subMessage.order.);

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
        public async void LimitOrderBuy() // and CancelLimitOrder
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


            //Attempt buy stored in order book
            MeResponseModel LimitOrderResponse = await fixture.Consumer.Client.PlaceLimitOrderAsync(
                limitOrderID, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Buy, amount, price);
            Assert.True(LimitOrderResponse.Status == MeStatusCodes.Ok);

            AccountEntity checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance.Reserved, fixture.AssetPrecission) == Math.Round(accountBalance.Reserved + amount, fixture.AssetPrecission));

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

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "LimitOrders")]
        public async void HandleMarketOrderBuy()
        {
            AccountEntity testAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount);
            BalanceDTO accountBalance1 = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance1);
            BalanceDTO accountBalance2 = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();
            Assert.NotNull(accountBalance2);

            string badMarketOrderId = Guid.NewGuid().ToString();
            string marketOrderId = Guid.NewGuid().ToString();
            double volume = 0.2;
            bool isStraight = true;
            double reservedVolume = 0.0;

            //Attempt bad buy
            string badMarketOrderResponse = await fixture.Consumer.Client.HandleMarketOrderAsync(
                badMarketOrderId, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Buy, accountBalance2.Balance * 5, isStraight, reservedVolume);

            Assert.NotNull(badMarketOrderResponse);

            MarketOrderWithTrades badMessage = (MarketOrderWithTrades)await fixture.WaitForRabbitMQ<MarketOrderWithTrades>(
                o => o.order.externalId == badMarketOrderId);

            Assert.NotNull(badMessage);
            Assert.True(badMessage.order.id == badMarketOrderResponse);
            Assert.True(badMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(badMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(badMessage.order.straight == isStraight);
            Assert.True(badMessage.order.volume == accountBalance2.Balance * 5);
            Assert.True(badMessage.order.reservedLimitVolume == reservedVolume);

            Assert.True(badMessage.order.status == "NotEnoughFunds");


            //Attempt proper buy
            string marketOrderResponse = await fixture.Consumer.Client.HandleMarketOrderAsync(
                marketOrderId, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Buy, volume, isStraight, reservedVolume);
                                                                       
            Assert.NotNull(marketOrderResponse);

            MarketOrderWithTrades message = (MarketOrderWithTrades)await fixture.WaitForRabbitMQ<MarketOrderWithTrades>(
                o => o.order.externalId == marketOrderId);
        
            Assert.NotNull(message);
            Assert.True(message.order.id == marketOrderResponse);
            Assert.True(message.order.clientId == fixture.TestAccountId1);
            Assert.True(message.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(message.order.straight == isStraight);
            Assert.True(message.order.volume == volume);
            Assert.True(message.order.reservedLimitVolume == reservedVolume);

            Assert.True(message.order.status == "Matched");

            double sumOfLimitVolumes = 0.0;
            double sumOfMarketVolumes = 0.0;
            double currentPrice = 0.0;

            foreach (var trade in message.trades)
            {
                Assert.True(trade.limitAsset == fixture.TestAsset1);
                Assert.True(trade.marketAsset == fixture.TestAsset2);
                Assert.True(trade.marketClientId == fixture.TestAccountId1);

                if (double.TryParse(trade.limitVolume, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedLimitVolume) &&
                    double.TryParse(trade.marketVolume, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedMarketVolume) &&
                    double.TryParse(trade.price, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedPrice))
                {
                    sumOfLimitVolumes += parsedLimitVolume;
                    sumOfMarketVolumes += parsedMarketVolume;
                    currentPrice = parsedPrice;
                    Assert.True(Math.Round(parsedMarketVolume, fixture.AssetPrecission) == Math.Round(parsedLimitVolume * parsedPrice, fixture.AssetPrecission));
                }
            }

            Assert.True(sumOfLimitVolumes == volume);

            //Wait for trades job to add entry to DB
            //Thread.Sleep(2000);

            //check MarketOrders table
            MarketOrderEntity marketOrderDBRecord = (MarketOrderEntity)await fixture.MarketOrdersRepository.TryGetAsync(marketOrderResponse);

            //Assert.NotNull(marketOrderDBRecord);
            if (marketOrderDBRecord != null)
            {
                Assert.True(marketOrderDBRecord.AssetPairId == fixture.TestAssetPair.Id);
                Assert.True(marketOrderDBRecord.ClientId == fixture.TestAccountId1);
                Assert.True(marketOrderDBRecord.Price == currentPrice);
                Assert.True(marketOrderDBRecord.Status == "Matched");
                Assert.True(marketOrderDBRecord.Straight == true);
                Assert.True(marketOrderDBRecord.Volume == volume);
            }

            //check account balance change
            AccountEntity checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance1 = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            BalanceDTO checkAccountBalance2 = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance1.Balance - accountBalance1.Balance, fixture.AssetPrecission) == Math.Round(sumOfLimitVolumes, fixture.AssetPrecission));
            Assert.True(Math.Round(checkAccountBalance2.Balance - accountBalance2.Balance, fixture.AssetPrecission) == Math.Round(sumOfMarketVolumes * -1, fixture.AssetPrecission));

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "LimitOrders")]
        public async void HandleMarketOrderSell()
        {
            AccountEntity testAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            Assert.NotNull(testAccount);
            BalanceDTO accountBalance1 = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            Assert.NotNull(accountBalance1);
            BalanceDTO accountBalance2 = testAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();
            Assert.NotNull(accountBalance2);

            string badMarketOrderId = Guid.NewGuid().ToString();
            string marketOrderId = Guid.NewGuid().ToString();
            double volume = 0.1;
            bool isStraight = true;
            double reservedVolume = 0.0;

            //Attempt bad buy
            string badMarketOrderResponse = await fixture.Consumer.Client.HandleMarketOrderAsync(
                badMarketOrderId, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Sell, accountBalance1.Balance + 10, isStraight, reservedVolume);

            Assert.NotNull(badMarketOrderResponse);

            MarketOrderWithTrades badMessage = (MarketOrderWithTrades)await fixture.WaitForRabbitMQ<MarketOrderWithTrades>(
                o => o.order.externalId == badMarketOrderId);

            Assert.NotNull(badMessage);
            Assert.True(badMessage.order.id == badMarketOrderResponse);
            Assert.True(badMessage.order.clientId == fixture.TestAccountId1);
            Assert.True(badMessage.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(badMessage.order.straight == isStraight);
            Assert.True(badMessage.order.volume == (accountBalance1.Balance + 10) * -1);
            Assert.True(badMessage.order.reservedLimitVolume == reservedVolume);

            Assert.True(badMessage.order.status == "NotEnoughFunds");

            //Attempt proper sell
            string marketOrderResponse = await fixture.Consumer.Client.HandleMarketOrderAsync(
                marketOrderId, fixture.TestAccountId1, fixture.TestAssetPair.Id, OrderAction.Sell, volume, isStraight, reservedVolume);

            Assert.NotNull(marketOrderResponse);

            MarketOrderWithTrades message = (MarketOrderWithTrades)await fixture.WaitForRabbitMQ<MarketOrderWithTrades>(
                o => o.order.externalId == marketOrderId);

            Assert.NotNull(message);
            Assert.True(message.order.id == marketOrderResponse);
            Assert.True(message.order.clientId == fixture.TestAccountId1);
            Assert.True(message.order.assetPairId == fixture.TestAssetPair.Id);
            Assert.True(message.order.straight == isStraight);
            Assert.True(message.order.volume == volume * -1);
            Assert.True(message.order.reservedLimitVolume == reservedVolume);

            Assert.True(message.order.status == "Matched");

            double sumOfLimitVolumes = 0.0;
            double sumOfMarketVolumes = 0.0;
            double currentPrice = 0.0;

            foreach (var trade in message.trades)
            {
                Assert.True(trade.limitAsset == fixture.TestAsset2);
                Assert.True(trade.marketAsset == fixture.TestAsset1);
                Assert.True(trade.marketClientId == fixture.TestAccountId1);

                if (double.TryParse(trade.limitVolume, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedLimitVolume) &&
                    double.TryParse(trade.marketVolume, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedMarketVolume) &&
                    double.TryParse(trade.price, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedPrice))
                {
                    sumOfLimitVolumes += parsedLimitVolume;
                    sumOfMarketVolumes += parsedMarketVolume;
                    currentPrice = parsedPrice;
                    Assert.True(Math.Round(parsedLimitVolume, fixture.AssetPrecission) == Math.Round(parsedMarketVolume * parsedPrice, fixture.AssetPrecission));
                }
            }

            Assert.True(sumOfMarketVolumes == volume);

            //Wait for trades job to add entry to DB
            //Thread.Sleep(2000);

            //check MarketOrders table
            MarketOrderEntity marketOrderDBRecord = (MarketOrderEntity)await fixture.MarketOrdersRepository.TryGetAsync(marketOrderResponse);

            //Assert.NotNull(marketOrderDBRecord);
            if (marketOrderDBRecord != null)
            {
                Assert.True(marketOrderDBRecord.AssetPairId == fixture.TestAssetPair.Id);
                Assert.True(marketOrderDBRecord.ClientId == fixture.TestAccountId1);
                Assert.True(marketOrderDBRecord.Price == currentPrice);
                Assert.True(marketOrderDBRecord.Status == "Matched");
                Assert.True(marketOrderDBRecord.Straight == true);
                Assert.True(marketOrderDBRecord.Volume == volume * -1);
            }

            //check accoutn balance change
            AccountEntity checkTestAccount = (AccountEntity)await fixture.AccountRepository.TryGetAsync(fixture.TestAccountId1);
            BalanceDTO checkAccountBalance1 = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset1).FirstOrDefault();
            BalanceDTO checkAccountBalance2 = checkTestAccount.BalancesParsed.Where(b => b.Asset == fixture.TestAsset2).FirstOrDefault();

            Assert.True(Math.Round(checkAccountBalance1.Balance - accountBalance1.Balance, fixture.AssetPrecission) == Math.Round(sumOfMarketVolumes * -1, fixture.AssetPrecission));
            Assert.True(Math.Round(checkAccountBalance2.Balance - accountBalance2.Balance, fixture.AssetPrecission) == Math.Round(sumOfLimitVolumes, fixture.AssetPrecission));

        }

        //[Fact]
        //public async void AddAsset()
        //{
        //    await fixture.Consumer.Client.UpdateBalanceAsync(
        //        Guid.NewGuid().ToString(), fixture.TestAccountId1, fixture.TestAsset2, 106.65);

        //    await fixture.Consumer.Client.UpdateBalanceAsync(
        //        Guid.NewGuid().ToString(), fixture.TestAccountId2, fixture.TestAsset2, 103.31);

        //}
    }
}
