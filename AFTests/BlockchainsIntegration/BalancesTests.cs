using BlockchainsIntegration.LiteCoin;
using BlockchainsIntegration.LiteCoin.Api;
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

namespace AFTests.BlockchainsIntegrationTests.LiteCoin
{
    class BalancesTests
    {
        public class GetBalances : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetBalancesTest()
            {
                var take = "1";

                if (blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS))
                {
                    blockchainApi.Balances.DeleteBalances(WALLET_ADDRESS);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                }

                //enable observation
                var pResponse = blockchainApi.Balances.PostBalances(WALLET_ADDRESS);


                Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS), 
                    Is.True.After(10*60 * 1000, 1 * 1000), "Wallet is not present in Get Balances after 10 minutes");

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
            [TestCase("")]
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

        public class CheckBalanceIsZeroBeforeGetBalance : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            [Description("Test will faill if at same time two or more treads will run it.")]
            public void CheckBalanceIsZeroBeforeGetBalanceTest()
            {
                // enable observation

                var pResponse = blockchainApi.Balances.PostBalances(WALLET_SINGLE_USE);
                Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_SINGLE_USE),
                    Is.True.After(10 * 60 * 1000, 1 * 1000), "Wallet is not present in Get Balances after 10 minutes");
                
                //create transaction and broadcast it

                long time1 = 0;
                long time2 = 0;
                
                var startBalance = blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == WALLET_SINGLE_USE).Balance;

                var model = new BuildTransactionRequest()
                {
                    Amount = "100002",
                    AssetId = CurrentAssetId(),
                    FromAddress = WALLET_SINGLE_USE,
                    IncludeFee = true,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString("N");

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { KEY_WALLET_SINGLE_USE }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                Parallel.Invoke(() =>
                {
                    GetBalanceDissapearingTime(startBalance, out time1);             
                }, () =>
                {
                    GetTransactionCompleteStatusTime(operationId, out time2);
                });

                TestContext.Out.WriteLine($"tick when balance changed: {time1} \n tick when we get Complete status: {time2}");

                Assert.Multiple(() => 
                {
                    Assert.That(time1, Is.LessThanOrEqualTo(time2), $"Time in Ticks. Time of balance changing is not less than Status became complete");
                    Assert.That(long.Parse(startBalance) - 100002, Is.EqualTo(long.Parse(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == WALLET_SINGLE_USE).Balance)), "New balance is not as expected");
                });
            }

            static void GetBalanceDissapearingTime(string startBalance, out long time)
            {
                var sw = new Stopwatch();
                time = 0;
                var request = new BlockchainApi(BlockchainSpecificSettings().ApiUrl);
                while (sw.Elapsed < TimeSpan.FromMinutes(10))
                {
                    if (int.Parse(request.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == WALLET_SINGLE_USE).Balance) <
                        int.Parse(startBalance))
                    {
                        time = DateTime.Now.Ticks;
                        sw.Stop();
                        return;
                    }
                }
                sw.Stop();
            }

            static void GetTransactionCompleteStatusTime(string operationId, out long time)
            {
                var sw = new Stopwatch();
                var request = new BlockchainApi(BlockchainSpecificSettings().ApiUrl);
                time = 0;
                while (sw.Elapsed < TimeSpan.FromMinutes(10))
                {
                    if ((request.Operations.GetOperationId(operationId).GetResponseObject().State == BroadcastedTransactionState.Completed))
                    {
                        time = DateTime.Now.Ticks;
                        sw.Stop();
                        return;
                    }
                }
                sw.Stop();
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
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
