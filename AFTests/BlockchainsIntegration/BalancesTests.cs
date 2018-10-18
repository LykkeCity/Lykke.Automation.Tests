using BlockchainsIntegration.Api;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.TestsData;

namespace AFTests.BlockchainsIntegrationTests
{
    class BalancesTests
    {
        public class GetBalances : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), $"Unxpected balance for wallet {wallet.PublicAddress}");
            }

            [TearDown]
            public void TearDown()
            {
                TransferCryptoBetweenWallets(wallet, HOT_WALLET);
                blockchainApi.Balances.DeleteBalances(GetWalletCorrectName(wallet?.PublicAddress));
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void GetBalancesTest()
            {
                var take = "500";

                Step($"Make GET /balances&take=500 and validate {wallet.PublicAddress} is present in response", () => 
                {
                    blockchainApi.Balances.GetBalances(take, null).Validate.StatusCode(HttpStatusCode.OK);

                    Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == wallet.PublicAddress),
                        Is.True.After(60).Seconds.PollEvery(1).Seconds, $"Wallet {wallet.PublicAddress} is not present in Get Balances after {BLOCKCHAIN_MINING_TIME} minutes");
                });
            }
        }

        public class GetBalancesInvalidTake : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testTake")]
            [TestCase("325.258")]
            [TestCase("!@&*()")]
            [Category("BlockchainIntegration")]
            public void GetBalancesInvalidTakeTest(string take)
            {
                Step($"Make GET /balances/{take} and validate response status is BadRequest", () => {
                    var response = blockchainApi.Balances.GetBalances(take, null);
                    response.Validate.StatusCode(HttpStatusCode.BadRequest, $"Unexpected Status code {response.StatusCode} for take: {take}");
                }); 
            }
        }

        public class GetBalancesContinuation : BlockchainsIntegrationBaseTest
        {
            //[Test]
            [Category("BlockchainIntegration")]
            public void GetBalancesContinuationTest()
            {
                EnableBLockchainProxy();

                var blockchainApi = new BlockchainApi(proxyApi);

                var take = "1";
                var continuation = TestData.GenerateString();
                var response = blockchainApi.Balances.GetBalances(take, continuation);
                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.BadRequest, HttpStatusCode.OK));
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class PostBalancesInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("testAddress")]
            [TestCase("!@&*()")]
            [TestCase("352.58")]
            [Category("BlockchainIntegration")]
            public void PostBalancesInvalidAddressTest(string address)
            {
                Step($"Make POST /balances/{address}/observation and validate response status is BadRequest(invalid address)", () =>
                {
                    var response = blockchainApi.Balances.PostBalances(address);
                    response.Validate.StatusCode(HttpStatusCode.BadRequest);
                });
            }
        }

        public class PostBalancesEmptyAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [Category("BlockchainIntegration")]
            public void PostBalancesEmptyAddressTest(string address)
            {
                Step($"Make POST /balances//observation and validate response status is NotFound", () => 
                {
                    var response = blockchainApi.Balances.PostBalances(address);
                    response.Validate.StatusCode(HttpStatusCode.NotFound);
                });
            }
        }

        public class DeleteBalancesInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("testAddress")]
            [TestCase("!@&*()")]
            [TestCase("352.58")]
            [Category("BlockchainIntegration")]
            public void DeleteBalancesInvalidAddressTest(string address)
            {
                Step($"Make DELETE /balances/{address}/observation and validate response status code is BadRequest", () => 
                {
                    var response = blockchainApi.Balances.DeleteBalances(address);
                    response.Validate.StatusCode(HttpStatusCode.BadRequest);
                });
            }
        }

        public class DWHWTransactionWillProduceIncreasOfBlockNumber : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
            }

            [TearDown]
            public void TearDown()
            {
                TransferCryptoBetweenWallets(wallet, HOT_WALLET);
                blockchainApi.Balances.DeleteBalances(GetWalletCorrectName(wallet?.PublicAddress));
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWHWTransactionWillProduceIncreasOfBlockNumberTest()
            {
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null, $"Wallet {wallet.PublicAddress} balance is null. Fail test");

                long? newBlock = null;

                var startBalance = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == wallet.PublicAddress).Balance;
                var startBlock = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == wallet.PublicAddress).Block;

                string operationId = "default operation id";

                Step($"Create DW - HW transtaction", () => 
                {
                    var model = new BuildSingleTransactionRequest()
                    {
                        Amount = AMOUNT,
                        AssetId = ASSET_ID,
                        FromAddress = wallet.PublicAddress,
                        IncludeFee = true,
                        OperationId = Guid.NewGuid(),
                        ToAddress = HOT_WALLET,
                        FromAddressContext = wallet.AddressContext
                    };

                    var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                    operationId = model.OperationId.ToString();

                    var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                    var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });
                });

                Step($"Wait for Transaction got Complete status(operationId: {operationId}) and for Balance block will be increased", () => 
                {
                    GetTransactionCompleteStatusTime(operationId, wallet.PublicAddress);
                    WaitForBalanceBlockIncreased(wallet.PublicAddress, startBlock);
                });
  
                newBlock = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == wallet.PublicAddress)?.Block;
                Step("Validate that final balance block is greater than start balance block", () =>
                {
                    if (newBlock == null)
                    {
                        var completeBlock = blockchainApi.Operations.GetOperationId(operationId).GetResponseObject();
                        Assert.That(completeBlock.Block, Is.GreaterThan(startBlock), "Block when operation got complete status is not greater than block with start balance");
                        Assert.Pass("Transaction got Complete status and wallet dissapear from GET /balances request");
                    }
                    else
                    {
                        Assert.That(() => blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().State, Is.EqualTo(BroadcastedTransactionState.Completed), $"Request doesnt have Complete status after {BLOCKCHAIN_MINING_TIME} minutes and still in a {blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().State}");

                        var transactionBlock = blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().Block;

                        TestContext.Progress.WriteLine($"old block: {startBlock} \n new block: {newBlock}");

                        Assert.That(newBlock.Value, Is.GreaterThan(startBlock), $"New balance block is not greater than previous");

                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
                        {
                            var balances = blockchainApi.Balances.GetBalances("500", null).GetResponseObject();

                            if (balances.Items.ToList().Any(a => a.Address == wallet.PublicAddress))
                            {
                                Assert.That(balances.Items.ToList().Find(w => w.Address == wallet.PublicAddress).Block, Is.GreaterThanOrEqualTo(transactionBlock), "Transaction block is not less or equal than latest balance block");
                            }
                            else
                            {
                                break;
                            }
                        }
                        sw.Stop();
                    }
                });

                Step("Validate balance records for wallet in GET /balances response. Validate wallet balance", () => 
                {
                    Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Count(a => a.Address == wallet.PublicAddress), Is.EqualTo(1), $"Unexpected instances of balance for wallet {wallet.PublicAddress}");
                    Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == wallet.PublicAddress).Balance, Is.EqualTo((decimal.Parse(AMOUT_WITH_FEE) - decimal.Parse(AMOUNT)).ToString()), "Balance is not expected");
                });
            }

            long? GetTransactionCompleteStatusTime(string operationId, string wallet)
            {
                var sw = new Stopwatch();
                long? block = -1;
                sw.Start();
                while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
                {
                    var r = blockchainApi.Operations.GetOperationId(operationId);
                    if (r.GetResponseObject().State != BroadcastedTransactionState.InProgress)
                    {
                        if (r.GetResponseObject().State == BroadcastedTransactionState.Failed)
                            Assert.Fail("Operation got 'Failed status'");
                        block = r.GetResponseObject()?.Block;
                        break;
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                sw.Stop();
                return block;
            }
        }

        public class DWHWandEwDwTransactionsFinalBlockNumber : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), $"Unxpected balance for wallet {wallet.PublicAddress}");
            }

            [TearDown]
            public void TearDown()
            {
                TransferCryptoBetweenWallets(wallet, HOT_WALLET);
                blockchainApi.Balances.DeleteBalances(GetWalletCorrectName(wallet?.PublicAddress));
            }

            long? GetTransactionCompleteStatusTime(string operationId, string wallet)
            {
                var sw = new Stopwatch();
                long? block = -1;
                sw.Start();
                while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
                {
                    var r = blockchainApi.Operations.GetOperationId(operationId);
                    if (r.GetResponseObject().State != BroadcastedTransactionState.InProgress)
                    {
                        if (r.GetResponseObject().State == BroadcastedTransactionState.Failed)
                            Assert.Fail("Operation got 'Failed status'");
                        block = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().FirstOrDefault(a => a.Address == wallet)?.Block;
                        break;
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                sw.Stop();
                return block;
            }
        }
    }
}
