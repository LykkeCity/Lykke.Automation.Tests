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
            [Test]
            [Category("BlockchainIntegration")]
            public void GetBalancesTest()
            {
                var take = "500";

                var newWallet = blockchainSign.PostWallet().GetResponseObject();

                AddCyptoToBalanceFromExternal(newWallet.PublicAddress);

                blockchainApi.Balances.GetBalances(take, null).Validate.StatusCode(HttpStatusCode.OK);

                if (blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS))
                {
                    blockchainApi.Balances.DeleteBalances(WALLET_ADDRESS);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                }

                //enable observation
                var pResponse = blockchainApi.Balances.PostBalances(WALLET_ADDRESS);

                Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS), 
                    Is.True.After(5*60 * 1000, 1 * 1000), "Wallet is not present in Get Balances after 10 minutes");

                //disable
                var dResponse = blockchainApi.Balances.DeleteBalances(WALLET_ADDRESS);

                Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS),
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

        public class CheckBlockNumberIncreasedAfterTransaction : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void CheckBlockNumberIncreasedAfterTransactionTest()
            {
                // enable observation

                var newWallet = blockchainSign.PostWallet().GetResponseObject();

                var pResponse = blockchainApi.Balances.PostBalances(newWallet.PublicAddress);
                AddCyptoToBalanceFromExternal(newWallet.PublicAddress);

                blockchainApi.Balances.GetBalances("500", null).Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Any(a => a.Address == newWallet.PublicAddress),
                    Is.True.After(5 * 60 * 1000, 1 * 1000), "Wallet is not present in Get Balances after 5 minutes");
                
                //create transaction and broadcast it

                long? newBlock = 0;
                
                var startBalance = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == newWallet.PublicAddress).Balance;
                var startBlock = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == newWallet.PublicAddress).Block;

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = newWallet.PublicAddress,
                    IncludeFee = true,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { newWallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                newBlock = GetTransactionCompleteStatusTime(operationId, newWallet.PublicAddress);

                Assert.That(()=> blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().State, Is.EqualTo(BroadcastedTransactionState.Completed), $"Request doesnt have Complete status after 10 minutes and still in a {blockchainApi.Operations.GetOperationId(operationId).GetResponseObject().State}");

                TestContext.Out.WriteLine($"old block: {startBlock} \n new block: {newBlock}");

                Assert.That(newBlock.Value, Is.GreaterThan(startBlock), $"New block is not greater than start block");
            }

            static long? GetTransactionCompleteStatusTime(string operationId, string wallet)
            {
                var sw = new Stopwatch();
                var request = new BlockchainApi(BlockchainApi);
                long? block = -1;
                sw.Start();
                while (sw.Elapsed < TimeSpan.FromMinutes(10))
                {
                    var r = request.Operations.GetOperationId(operationId);
                    if(r.GetResponseObject().State != BroadcastedTransactionState.InProgress)
                    {
                        if (r.GetResponseObject().State == BroadcastedTransactionState.Failed)
                            Assert.Fail("Operation got 'Failed status'");
                        block = request.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().FirstOrDefault(a => a.Address == wallet)?.Block;
                        break;
                    }                  
                }
                sw.Stop();
                return block;
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
    }
}
