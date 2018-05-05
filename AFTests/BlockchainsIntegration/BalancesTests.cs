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
                wallet = blockchainSign.PostWallet().GetResponseObject();
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void GetBalancesTest()
            {
                var take = "500";

                AddCyptoToBalanceFromExternal(wallet.PublicAddress, wallet.PrivateKey);

                blockchainApi.Balances.GetBalances(take, null).Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == wallet.PublicAddress),
                    Is.True.After(5 * 60 * 1000, 1 * 1000), $"Wallet {wallet.PublicAddress} is not present in Get Balances after 10 minutes");

                //disable
                var dResponse = blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);

                Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == wallet.PublicAddress),
                    Is.False.After(1 * 60 * 1000, 1 * 1000), "Wallet still present in Get Balances after Delete");
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
                var response = blockchainApi.Balances.GetBalances(take, null);
                response.Validate.StatusCode(HttpStatusCode.BadRequest, $"Unexpected Status code {response.StatusCode} for take: {take}");
            }
        }

        public class GetBalancesContinuation : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetBalancesContinuationTest()
            {
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
                var response = blockchainApi.Balances.PostBalances(address);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class PostBalancesEmptyAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [Category("BlockchainIntegration")]
            public void PostBalancesEmptyAddressTest(string address)
            {
                var response = blockchainApi.Balances.PostBalances(address);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
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
                var response = blockchainApi.Balances.DeleteBalances(address);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class DWHWTransactionWillProduceIncreasOfBlockNumber : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = blockchainSign.PostWallet().GetResponseObject();
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWHWTransactionWillProduceIncreasOfBlockNumberTest()
            {
                // enable observation

                var pResponse = blockchainApi.Balances.PostBalances(wallet.PublicAddress);
                AddCyptoToBalanceFromExternal(wallet.PublicAddress, wallet.PrivateKey);

                blockchainApi.Balances.GetBalances("500", null).Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Any(a => a.Address == wallet.PublicAddress),
                    Is.True.After(5 * 60 * 1000, 2 * 1000), "Wallet is not present in Get Balances after 5 minutes");

                //create transaction and broadcast it

                long? newBlock = null;

                var startBalance = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == wallet.PublicAddress).Balance;
                var startBlock = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == wallet.PublicAddress).Block;

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUT_WITH_FEE,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = true,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                newBlock = GetTransactionCompleteStatusTime(operationId, wallet.PublicAddress);
                if (newBlock == null)
                {
                    var completeBlock = blockchainApi.Operations.GetOperationId(model.OperationId.ToString()).GetResponseObject();
                     Assert.That(completeBlock.Block, Is.GreaterThan(startBlock), "Block when operation got complete status is not greater than block with start balance");
                    Assert.Pass("Transaction got Complete status and wallet dissapear from GET /balances request");
                }
                else
                {
                    Assert.That(() => blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().State, Is.EqualTo(BroadcastedTransactionState.Completed), $"Request doesnt have Complete status after 10 minutes and still in a {blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().State}");

                    var transactionBlock = blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().Block;

                    TestContext.Progress.WriteLine($"old block: {startBlock} \n new block: {newBlock}");

                    Assert.That(newBlock.Value, Is.GreaterThan(startBlock), $"New balance block is not greater than previous");


                    // new part 

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
                    {
                        var balances = blockchainApi.Balances.GetBalances("500", null).GetResponseObject();

                        if (balances.Items.ToList().Any(a => a.Address == wallet.PublicAddress))
                        {
                            Assert.That(balances.Items.ToList().Find(w => w.Address == wallet.PublicAddress).Block, Is.LessThan(transactionBlock), "Transaction block is not greater than balance block");
                        }
                        else
                        {
                            break;
                        }
                    }
                    sw.Stop();

                    Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Any(a => a.Address == wallet.PublicAddress),
                        Is.False, $"Wallet is present in Get Balances after {BLOCKCHAIN_MINING_TIME} minutes");
                }
            }

            static long? GetTransactionCompleteStatusTime(string operationId, string wallet)
            {
                var sw = new Stopwatch();
                var request = new BlockchainApi(BlockchainApi);
                long? block = -1;
                sw.Start();
                while (sw.Elapsed < TimeSpan.FromMinutes(BLOCKCHAIN_MINING_TIME))
                {
                    var r = request.Operations.GetOperationId(operationId);
                    if (r.GetResponseObject().State != BroadcastedTransactionState.InProgress)
                    {
                        if (r.GetResponseObject().State == BroadcastedTransactionState.Failed)
                            Assert.Fail("Operation got 'Failed status'");
                        block = request.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().FirstOrDefault(a => a.Address == wallet)?.Block;
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
