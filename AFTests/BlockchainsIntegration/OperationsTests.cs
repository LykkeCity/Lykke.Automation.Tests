using BlockchainsIntegration.Models;
using Lykke.Client.AutorestClient.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using XUnitTestCommon.RestRequests.Interfaces;

namespace AFTests.BlockchainsIntegrationTests
{
    class OperationsTests
    {
        public class GetOperationId : BlockchainsIntegrationBaseTest
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
            public void GetOperationIdTest()
            {
                var usedOperationId = Guid.NewGuid();

                Step("Perform Build, Sign, Broadcast DW - HW transaction", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;
                });

                Step("Make GET /transactions/broadcast/single/{operationId} and validate response", () => 
                {
                    var getResponse = blockchainApi.Operations.GetOperationId(usedOperationId.ToString());

                    Assert.That(() => blockchainApi.Operations.GetOperationId(usedOperationId.ToString()).StatusCode, Is.EqualTo(HttpStatusCode.OK).After((int)BLOCKCHAIN_MINING_TIME * 60 * 1000, 1 * 1000));

                    Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(usedOperationId));
                });
            }
        }

        public class GetOperationIdInvalidId : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testId")]
            [TestCase("111222333")]
            [TestCase("!@%^&*(")]
            [Category("BlockchainIntegration")]
            public void GetOperationIdInvalidIdTest(string operationId)
            {
                Step($"Make GET /transactions/broadcast/single/{operationId} and validate status code is one of [BadRequest, NotFound, MethodNotAllowed]", () => { });

                var response = blockchainApi.Operations.GetOperationId(operationId);

                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed));
            }
        }

        public class GetOperationIdGuidId : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetOperationIdGuidIdTest()
            {
                var operationId = Guid.NewGuid().ToString();
                Step("Make DELETE /transactions/broadcast/{operationId} to delete from observation ", () => 
                {
                    blockchainApi.Operations.DeleteOperationId(operationId);
                });

                Step("Make GET /transactions/broadcast/single/{operationId} and Validate response status is NoContent", () => 
                {
                    var response = blockchainApi.Operations.GetOperationId(operationId);

                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), "Response for request GET /transactions/broadcast/single/{operationId} with deleted operationId does not equal NoContent(204)");
                });
            }
        }

        public class GetOperationIdGuidIdManyInputs : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetOperationIdGuidIdManyInputsTest()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs are not supported by blockchain");

                var operationId = Guid.NewGuid().ToString();

                Step("Make DELETE /transactions/broadcast/{operationId} to delete from observation", () => { blockchainApi.Operations.DeleteOperationId(operationId); });

                Step("Make GET /transactions/broadcast/many-inputs/{operationId} and Validate response status code is NoContent", () => 
                {
                    var response = blockchainApi.Operations.GetTransactionsManyInputs(operationId);

                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), "Response for request GET /transactions/broadcast/many-inputs/ with deleted operationId does not equal NoContent(204)");
                });
            }
        }

        public class GetOperationIdGuidIdManyOutputs : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetOperationIdGuidIdManyOutputsTest()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many outputs are not supported by blockchain");

                var operationId = Guid.NewGuid().ToString();
                Step("Make DELETE /transactions/broadcast/{operationId} to delete from observation", () => { blockchainApi.Operations.DeleteOperationId(operationId); });

                Step("Make GET /transactions/broadcast/many-outputs/{operationId} and validate response status code is NoContent", () => 
                {
                    var response = blockchainApi.Operations.GetTransactionsManyOutputs(operationId);

                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), "Response for request GET /transactions/broadcast/many-outputs/ with deleted operationId does not equal NoContent(204)");
                });
            }
        }

        public class PostTransactionsInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsInvalidAddressTest()
            {
                Step("Make POST /transactions/single with invalid FromAddress, ToAddress and validate response status is BadRequest", () => 
                {
                    var model = new BuildSingleTransactionRequest()
                    {
                        Amount = "10",
                        AssetId = ASSET_ID,
                        FromAddress = "testAddress",
                        IncludeFee = false,
                        OperationId = Guid.NewGuid(),
                        ToAddress = "testAddress1"
                    };

                    var response = blockchainApi.Operations.PostTransactions(model);
                    response.Validate.StatusCode(HttpStatusCode.BadRequest);
                    Assert.That(response.GetResponseObject().TransactionContext, Is.Null);
                });
            }
        }

        public class PostTransactionsEmptyObject : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsEmptyObjectTest()
            {
                Step("Make POST /transactions/single with empty body and validate status code is BadRequest", () => 
                {
                    var model = new BuildSingleTransactionRequest()
                    {
                    };

                    var response = blockchainApi.Operations.PostTransactions(model);
                    response.Validate.StatusCode(HttpStatusCode.BadRequest);
                });
            }
        }

        public class PostTransactionsBroadcast : BlockchainsIntegrationBaseTest
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
            public void PostTransactionsBroadcastTest()
            {
                Step("Perform BUILD, SIGN, BROADCAST methods with valid parameters and validate broadcast response is OK", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                });
            }
        }

        public class PostTransactionsBroadcastInvalidTransaction : BlockchainsIntegrationBaseTest
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
            public void PostTransactionsBroadcastInvalidTransactionTest()
            {
                var usedOperationId = Guid.NewGuid();
                Step("Make BUILD, SIGN operations with valid parameters", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;
                });

                string sTransaction = Guid.NewGuid().ToString();

                Step("Make Broadcast operation with valid operationId, but invalid SignedTransaction and validate status code is BadRequest, content contains 'errorMessage'", () => 
                {
                    var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest()
                    { OperationId = usedOperationId, SignedTransaction = sTransaction });

                    response.Validate.StatusCode(HttpStatusCode.BadRequest);
                    Assert.That(response.Content, Does.Contain("errorMessage").IgnoreCase);
                });
            }
        }

        public class DeleteOperationId : BlockchainsIntegrationBaseTest
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
            public void DeleteOperationIdTest()
            {
                var operationID = Guid.NewGuid();

                Step("Make BUILD, SIGN, BROADCAST with valid parameters", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    operationID = operationId;
                });

                Step("Make DELETE /transactions/broadcast/{operationId} with valid operationId and validate response status code is OK", () => 
                {
                    var responseDelete = blockchainApi.Operations.DeleteOperationId(operationID.ToString());
                    responseDelete.Validate.StatusCode(HttpStatusCode.OK);
                });  
            }
        }

        public class DeleteOperationIdInvalidOId : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void DeleteOperationIdInvalidOIdTest(string operationId)
            {
                Step($"Make DELETE /transactions/broadcast/{operationId} and validate stponse status code is one of [BadRequest, NoContent, NotFound, MethodNotAllowed]", () => 
                {
                    var response = blockchainApi.Operations.DeleteOperationId(operationId);
                    Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.BadRequest, HttpStatusCode.NoContent, HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed));
                });
            }
        }

        //outputs
        public class GetTransactionsManyOutputsInvalidOperationId : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyOutputsInvalidOperationIdTest(string operationId)
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many outputs are not supported by blockchain");

                Step($"Make GET /transactions/broadcast/many-outputs/{operationId}. Validate status code is one of [NoContent, NotImplemented, BadRequest, MethodNotAllowed, NotFound]", () => 
                {
                    var response = blockchainApi.Operations.GetTransactionsManyOutputs(operationId);
                    Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.NoContent, HttpStatusCode.NotImplemented, HttpStatusCode.BadRequest, HttpStatusCode.MethodNotAllowed, HttpStatusCode.NotFound));
                });
            }
        }

        public class GetTransactionsManyOutputs: BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many outputs are not supported by blockchain");

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
            public void GetTransactionsManyOutputsTest()
            {
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null, $"Wallet {wallet.PublicAddress} balance is null. Fail test");

                var transactionContext = "";

                var operationID = Guid.NewGuid();

                Step("Make BuildTransactionWithManyOutputsRequest with valid parameters", () => 
                {
                    var request = new BuildTransactionWithManyOutputsRequest()
                    {
                        AssetId = ASSET_ID,
                        OperationId = operationID,
                        FromAddress = wallet.PublicAddress,
                        FromAddressContext = wallet.AddressContext,
                        Outputs = new List<TransactionOutputContract>() { new TransactionOutputContract() { Amount = AMOUNT, ToAddress = HOT_WALLET } }
                    };

                    var response = blockchainApi.Operations.PostTransactionsManyOutputs(request);
                    response.Validate.StatusCode(HttpStatusCode.OK);

                    transactionContext = response.GetResponseObject().TransactionContext;
                });

                Step("Sign and Broadcast Many-Outputs transaction", () => 
                {
                    var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = transactionContext, PrivateKeys = new List<string>() { wallet.PrivateKey } });

                    var broadcastRequset = new BroadcastTransactionRequest() { OperationId = operationID, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
                    var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);
                });

                Step($"Make GET /transactions/broadcast/many-outputs/{operationID.ToString()} and validate status code is OK", () => 
                {
                    var getResponse = blockchainApi.Operations.GetTransactionsManyOutputs(operationID.ToString());
                    getResponse.Validate.StatusCode(HttpStatusCode.OK);
                });
            }
        }

        //inputs
        public class GetTransactionsManyInputsInvalidOperationId : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyInputsInvalidOperationIdTest(string operationId)
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs not supported by blockchain");

                Step($"Make GET /transactions/broadcast/many-inputs/{operationId} and validate status code is one of [NoContent, NotImplemented, BadRequest, NotFound, MethodNotAllowed]", () => 
                {
                    var response = blockchainApi.Operations.GetTransactionsManyInputs(operationId);
                    Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.NoContent, HttpStatusCode.NotImplemented, HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed));
                });
            }
        }

        public class GetTransactionsManyInputs : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs are not supported by blockchain");

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
            public void GetTransactionsManyInputsTest()
            {
                var operationId = Guid.NewGuid();
                var transactionContext = "";

                Step("Make POST /transactions/many-inputs with valid parameters", () => 
                {
                    var request = new BuildTransactionWithManyInputsRequest()
                    {
                        AssetId = ASSET_ID,
                        OperationId = operationId,
                        ToAddress = HOT_WALLET,
                        Inputs = new List<TransactionInputContract>() { new TransactionInputContract()
                    {
                        Amount = AMOUNT, FromAddress = wallet.PublicAddress,
                        FromAddressContext = wallet.AddressContext
                    } }
                    };

                    var response = blockchainApi.Operations.PostTransactionsManyInputs(request);
                    response.Validate.StatusCode(HttpStatusCode.OK);

                    transactionContext = response.GetResponseObject().TransactionContext;
                });

                Step("Make Sign and Broadcast requests", () => 
                {
                    var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = transactionContext, PrivateKeys = new List<string>() { wallet.PrivateKey } });

                    var broadcastRequset = new BroadcastTransactionRequest() { OperationId = operationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
                    var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);
                });

                Step("Make GET /transactions/broadcast/many-inputs/{operationId} and validate response status code is OK", () =>
                {
                    var getResponse = blockchainApi.Operations.GetTransactionsManyInputs(operationId.ToString());
                    getResponse.Validate.StatusCode(HttpStatusCode.OK);
                });
            }
        }

        //post many inputs

        public class PostTransactionsManyInputsInvalidOperationId : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs not supported by blockchain");

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

            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void PostTransactionsManyInputsInvalidOperationIdTest(string operationId)
            {
                Step($"Make POST /transactions/many-inputs with invalid operationId '{operationId}' and validate response status code is one from [BadRequest, NotImplemented]", () => 
                {
                    var request = new BuildTransactionWithManyInputsRequest()
                    {
                        AssetId = ASSET_ID,
                        OperationId = Guid.NewGuid(),
                        ToAddress = HOT_WALLET,
                        Inputs = new List<TransactionInputContract>() { new TransactionInputContract()
                    {
                        Amount = AMOUNT,
                        FromAddress = wallet.PublicAddress,
                        FromAddressContext = wallet.AddressContext
                    } }
                    };

                    var json = JsonConvert.SerializeObject(request);
                    json = json.Replace(request.OperationId.ToString(), operationId);

                    var response = blockchainApi.Operations.PostTransactionsManyInputs(json);
                    Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.BadRequest, HttpStatusCode.NotImplemented));
                });  
            }
        }



        public class SameOperationIdFoDifferentTransactions : BlockchainsIntegrationBaseTest
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
            public void SameOperationIdFoDifferentTransactionsTest()
            {
                var operationId = Guid.NewGuid();

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = true,
                    OperationId = operationId,
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                Step("Make POST /transactions/single request", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                    responseTransaction.Validate.StatusCode(HttpStatusCode.OK);
                    Assert.That(responseTransaction.GetResponseObject().TransactionContext, Is.Not.Null.Or.Empty, "Transaction Context is null");
                });

                Step("Repeat POST /transactions/single request. Validate, that transaction context is not null or empty", () =>
                {
                    var secondResponseTransaction = blockchainApi.Operations.PostTransactions(model);
                    Assert.That(secondResponseTransaction.GetResponseObject().TransactionContext, Is.Not.Null.Or.Empty, "Transaction Context is null");
                }); 
            }
        }

        public class DWHWTransfer : BlockchainsIntegrationBaseTest
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
            [Description("Transfer all balance from DW to EW")]
            public void DWHWTransferTest()
            {
                Assert.That(HOT_WALLET, Is.Not.Null.Or.Empty, "Hot wallet address and key are empty!");

                var take = "500";

                var currentBalance = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                Assert.That(currentBalance, Is.Not.Null, $"{wallet.PublicAddress} does not present in GetBalances or its balance is null");

                var operationId = Guid.NewGuid();

                var extensionReponse = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired;
                var includeFee = true;
                if (extensionReponse.HasValue)
                    includeFee = !extensionReponse.Value;

                IResponse<BroadcastedSingleTransactionResponse> getResponse = null;
                Step("Make DW - HW BUILD, SIGN, BROADCAST transactions", () => 
                {
                    var model = new BuildSingleTransactionRequest()
                    {
                        Amount = currentBalance,
                        AssetId = ASSET_ID,
                        FromAddress = wallet.PublicAddress,
                        IncludeFee = includeFee,
                        OperationId = operationId,
                        ToAddress = HOT_WALLET,
                        FromAddressContext = wallet.AddressContext
                    };

                    var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();

                    var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                    var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = operationId, SignedTransaction = signResponse.SignedTransaction });

                    getResponse = blockchainApi.Operations.GetOperationId(operationId.ToString());
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(model.OperationId));
                });

                Step("Wait for operation got Complete status", () =>
                { WaitForOperationGotCompleteStatus(operationId.ToString()); });

                Step("Validate Balance for DW, should be 0 or not present in GET /balances response", () => 
                {
                    if (getResponse.GetResponseObject().State == BroadcastedTransactionState.Completed)
                    {
                        Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.EqualTo("0").Or.Null.After((int)BLOCKCHAIN_MINING_TIME * 60 * 1000, 2 * 1000), $"Unexpected balance for wallet {wallet.PublicAddress}");
                    }
                    else
                    {
                        //validate balance is 0 or null
                        Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.EqualTo("0").Or.Null.After((int)BLOCKCHAIN_MINING_TIME * 60 * 1000, 2 * 1000), $"Unexpected balance for wallet {wallet.PublicAddress}");
                    }
                });

                Step($"Validate that Amount is > 0 in '/transactions/broadcast/single/{operationId}' response", () => 
                {
                    getResponse = blockchainApi.Operations.GetOperationId(operationId.ToString());
                    var amountInDecimal = decimal.Parse(getResponse.GetResponseObject().Amount);
                    Assert.That(amountInDecimal, Is.GreaterThan(0), $"current amount '{amountInDecimal}' is not greater than 0");
                });

                if (includeFee)
                    Step($"Validate that Fee is > 0 in '/transactions/broadcast/single/{operationId}' response", () =>
                {
                    getResponse = blockchainApi.Operations.GetOperationId(operationId.ToString());
                    var feeInDecimal = decimal.Parse(getResponse.GetResponseObject().Fee);
                    Assert.That(feeInDecimal, Is.GreaterThan(0), $"current fee '{feeInDecimal}' is not greater than 0");
                });
                else
                    Step($"Validate that Fee == 0 in '/transactions/broadcast/single/{operationId}' response", () =>
                    {
                        getResponse = blockchainApi.Operations.GetOperationId(operationId.ToString());
                        var feeInDecimal = decimal.Parse(getResponse.GetResponseObject().Fee);
                        Assert.That(feeInDecimal, Is.EqualTo(0), $"current fee '{feeInDecimal}' is not expected");
                    });
            }
        }

        //Test 1 DW-Hw, broadcast double, all balance
        public class DWHWTransferDoubleBroadcast : BlockchainsIntegrationBaseTest
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
            public void DWHWTransferDoubleBroadcastTest()
            {
                string take = "500";

                var currentBalance = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                var extensionReponse = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired;
                var includeFee = true;
                if (extensionReponse.HasValue)
                    includeFee = !extensionReponse.Value;

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = currentBalance,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = includeFee,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };
                var signedTransaction = "";
                Step("Perform BUILD, SIGN, BROADCAST DW - HW operations with valid parameters", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                    string operationId = model.OperationId.ToString();

                    var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                    var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    signedTransaction = signResponse.SignedTransaction;
                });

                Step("Repeat BROADCAST operation and validate status code is Conflict", () => 
                {
                    var response1 = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signedTransaction });

                    response1.Validate.StatusCode(HttpStatusCode.Conflict);
                });
            }
        }

        public class DWHWPartialTransferDoubleBroadcast : BlockchainsIntegrationBaseTest
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
            public void DWHWPartialTransferDoubleBroadcastTest()
            {
                string take = "500";

                var currentBalance = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                var startBlock = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Block;
                
                var partialBalance = Math.Round(Decimal.Parse(currentBalance) * 0.4m);

                var usedOperationId = Guid.NewGuid();

                Step($"Make Build, Sign, Broadcast DW - HW operations. Wallet start balance is '{currentBalance}'. start block is '{startBlock}", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET, partialBalance.ToString(), true);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET, partialBalance.ToString(), true);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;
                });

                Step("Wait for operation got Complete status and for Balance block will be increased", () => 
                {
                    WaitForOperationGotCompleteStatus(usedOperationId.ToString());
                    
                    var txBlock = blockchainApi.Operations.GetOperationId(usedOperationId.ToString()).GetResponseObject().Block;
                    
                    WaitForBalanceBlockIncreased(wallet.PublicAddress, txBlock - 1);
                });

                Step("Validate DW balance after transaction", () => 
                {
                    var balanceAfterTransaction = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                    Assert.That(balanceAfterTransaction, Is.EqualTo((decimal.Parse(currentBalance) - partialBalance).ToString()), "Unexpected Balance after partial transaction");
                });

                Step("Repeat BUILD, SIGN, BROADCAST DW - HW operations", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET, partialBalance.ToString(), true);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET, partialBalance.ToString(), true);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;
                });

                Step("Wait for operation got Complete status and wait for Balance block will be increased", () => 
                {
                    WaitForOperationGotCompleteStatus(usedOperationId.ToString());
                    
                    var txBlock = blockchainApi.Operations.GetOperationId(usedOperationId.ToString()).GetResponseObject().Block;
                    
                    WaitForBalanceBlockIncreased(wallet.PublicAddress, txBlock - 1);
                });

                Step("Validate balance after second DW - HW transaction", () => 
                {
                   var balanceAfterTransaction = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                    Assert.That(balanceAfterTransaction, Is.EqualTo(Math.Round(decimal.Parse(currentBalance) - partialBalance * 2m).ToString()), "Unexpected Balance after partial transaction");
                }); 
            }
        }
       
        public class DoublePostTransactionSingleReturnConflict : BlockchainsIntegrationBaseTest
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
            public void DoublePostTransactionSingleReturnConflictTest()
            {
                var usedOperationId = Guid.NewGuid();     

                Step("Make BUILD, SIGN, BROADCAST DW-HW operations", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;
                });
                
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = HOT_WALLET,
                    IncludeFee = false,
                    OperationId = usedOperationId,
                    ToAddress = EXTERNAL_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT
                };

                Step("Perform BUILD HW-EW transaction with the same operaionId and validate status code is Conflict", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                    Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
                });
            }
        }

        public class DoublePostTransactionSingleRecieveReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired.HasValue &&
    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not require recieve transaction");

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
            public void DoublePostTransactionSingleRecieveConflictTest()
            {
                var usedOperationId = Guid.NewGuid();

                Step("Make BUILD, SIGN, BROADCAST DW - HW operations", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;

                });

                var recieve = new BuildSingleReceiveTransactionRequest
                {
                    operationId = usedOperationId,
                    sendTransactionHash = ""
                };

                Step("Perform POST /transactions/single/receive with used operationId. Validate response status code is Conflict", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTranstactionSingleRecieve(recieve);
                    Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
                });  
            }
        }

        public class DoublePostTransactionManyInputsReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported.HasValue &&
    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support many-inputs transaction");

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
            public void DoublePostTransactionManyInputsReturnConflictTest()
            {
                var usedOperationId = Guid.NewGuid();

                Step("Perform BUILD, SIGN, BROADCAST Many-Inputs  DW - HW operations", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;
                });

                var model = new BuildTransactionWithManyInputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = usedOperationId,
                    ToAddress = EXTERNAL_WALLET,
                    Inputs = new List<TransactionInputContract> { new TransactionInputContract { Amount = AMOUNT, FromAddress = HOT_WALLET, FromAddressContext = HOT_WALLET_CONTEXT } }
                };
                Step("Perform Build Many-Inputs transaction with the same OperatioId. Validate status code is Conflict", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactionsManyInputs(model);
                    Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
                });   
            }
        }

        public class DoublePostTransactionManyOutputsReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported.HasValue &&
    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support many-outputs transaction");

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
            public void DoublePostTransactionManyOutputsReturnConflictTest()
            {
                Guid usedOperationId;

                Step("Perform BUILD, SIGN, BROADCAST DW-HW Many-Outputs operations", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    usedOperationId = operationId;
                });

                var model = new BuildTransactionWithManyOutputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = usedOperationId,
                    FromAddress = HOT_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT,
                    Outputs = new List<TransactionOutputContract> { new TransactionOutputContract { Amount = AMOUNT, ToAddress = HOT_WALLET } }
                };
                Step("Perform POST Many-Outputs with used operationId and validate status code is Conflict", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactionsManyOutputs(model);
                    Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
                }); 
            }
        }

        public class DoublePutTransactionReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTransactionsRebuildingSupported.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTransactionsRebuildingSupported.HasValue &&
!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTransactionsRebuildingSupported.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support PUT /transaction");

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
            public void DoublePutTransactionReturnConflictTest()
            {
                Guid createdOperationId;

                Step("Make BUILD, SIGN, BROADCAST DW-HW operations", () => 
                {
                    var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);

                    while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);
                    }
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    createdOperationId = operationId;
                });

                var model = new RebuildTransactionRequest()
                {
                    OperationId = createdOperationId,
                    FeeFactor = 1
                };

                Step("Make Rebuild(PUT /transactions) with used OperationId. Validate status code is one of [Conflict, NotImplemented]", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PutTransactions(model);
                    Assert.That(responseTransaction.StatusCode, Is.AnyOf(HttpStatusCode.Conflict, HttpStatusCode.NotImplemented));
                });       
            }
        }

        // security

        public class DWHWWithoutHWKey : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue &&
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support address context");

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
            public void DWHWWithoutHWKeyTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT
                };

                var key = "fake_deposit_key";

                var transactionContext = "";
                Step("Perform Build DW-HW operation", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                    transactionContext = responseTransaction.GetResponseObject().TransactionContext;
                });

                Step("Perform Sign operation with invalid key. Validate response status code is not OK", () => 
                {
                    var signResponse = blockchainSign.PostSign(new SignRequest { PrivateKeys = new List<string> { "fake_deposit_key" }, TransactionContext = transactionContext });

                    Assert.That(signResponse.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK), $"DW HW transaction has been successfully signed with DW pkey {key} for blockchains with address extension");
                });
            }
        }

        public class DWHWWithHWKey : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue &&
                   !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support address context");

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
            public void DWHWWithHWKeyTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT
                };

                Step("Sign Build DW-HW transaction with HW key for blockchain with address extension. Validate status code is OK", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                    var signResponse = blockchainSign.PostSign(new SignRequest { PrivateKeys = new List<string> { HOT_WALLET_KEY }, TransactionContext = responseTransaction.GetResponseObject().TransactionContext });

                    Assert.That(signResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "DW HW transaction has NOT been successfully signed with DW pkey for blockchains with address extension");
                }); 
            }
        }

        public class DWDWWithoutHWKey : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;
            WalletCreationResponse wallet1;

            [SetUp]
            public void SetUp()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue &&
    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support address context");

                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), $"Unxpected balance for wallet {wallet.PublicAddress}");
                wallet1 = Wallets().Dequeue();
            }

            [TearDown]
            public void TearDown()
            {
                TransferCryptoBetweenWallets(wallet, HOT_WALLET);
                blockchainApi.Balances.DeleteBalances(GetWalletCorrectName(wallet?.PublicAddress));
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWDWWithoutHWKeyTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = wallet1.PublicAddress,
                    FromAddressContext = HOT_WALLET_CONTEXT
                };

                Step("Perform DW - DW transaction with DW key. Blockchain support address extension. Validate response status code is not OK", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactions(model);

                    var key = blockchainSign.PostWallet().GetResponseObject().PrivateKey;
                    var signResponse = blockchainSign.PostSign(new SignRequest { PrivateKeys = new List<string> { key }, TransactionContext = responseTransaction.GetResponseObject().TransactionContext });

                    Assert.That(signResponse.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK), "DW DW transaction has been successfully signed with DW pkey for blockchains with address extension");
                });
            }
        }

        public class BadRequestAfterSignExpiration : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void BadRequestAfterSignExpirationTest()
            {
                if (SIGN_EXPIRATION_SECONDS == 0)
                    Assert.Ignore("Blockchain Does not support Sign Expiration");

                var operationId = Guid.NewGuid();

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = HOT_WALLET,
                    IncludeFee = false,
                    OperationId = operationId,
                    ToAddress = EXTERNAL_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT
                };

                var signedTransaction = "";
                Step("Make BUILD, SIGN HW - EW operations.", () => 
                {
                    var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();

                    var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { HOT_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();
                    signedTransaction = signResponse.SignedTransaction;
                });

                Step($"Wait '{SIGN_EXPIRATION_SECONDS}' seconds while Sign will be expired", () => 
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(SIGN_EXPIRATION_SECONDS));
                });

                Step("Perform BROADCAST operation after sign has been expired. Validate Status code is BadRequest. Content contains 'buildingShouldBeRepeated'", () => 
                {
                    var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signedTransaction });

                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                    Assert.That(response.Content, Does.Contain("buildingShouldBeRepeated"));
                });

                model.OperationId = Guid.NewGuid();

                Step("Perform BUILD(with new guid), SIGN, BROADCAST HW-EW operations and validate status code is OK", () =>
                {

                    var result = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);

                    while (result.response.StatusCode == HttpStatusCode.BadRequest && result.response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                        REBUILD_ATTEMPT_COUNT--;
                        result = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);
                    }
                    result.response.Validate.StatusCode(HttpStatusCode.OK);
                });            
            }
        }

        public class InvalidXmlSymbol : BlockchainsIntegrationBaseTest
        {
            [TestCase("\\")]
            [TestCase("/")]
            [TestCase("#")]
            [TestCase("\\")]
            [TestCase("?")]
            [TestCase("\t")]
            [TestCase("\r")]
            [TestCase("\n")]
            [TestCase("\0")]
            [TestCase("\a")]
            [TestCase("\b")]
            [Category("BlockchainIntegration")]
            public void InvalidXmlSymbolTest(string symbol)
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue ||
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value)
                    Assert.Ignore($"Blockchain {BlockChainName} does not support public address extension");

                WalletCreationResponse wallet = null;
                var separator = blockchainApi.Constants.GetConstants().GetResponseObject().publicAddressExtension.separator;
                var wrongWalletAddressWithInvalidSymbol = $"{HOT_WALLET}{separator}{symbol}";

                Step("Create wallet", () => 
                {
                    var walletResponse = blockchainSign.PostWallet();
                    Assert.That(walletResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    wallet = walletResponse.GetResponseObject();
                });

                Step($"Create transaction from EW to wrong wallet address {wrongWalletAddressWithInvalidSymbol}", () => 
                {
                    blockchainApi.Balances.PostBalances(wallet.PublicAddress);
                    var model = new BuildSingleTransactionRequest()
                    {
                        Amount = AMOUT_WITH_FEE,
                        AssetId = ASSET_ID,
                        FromAddress = EXTERNAL_WALLET,
                        IncludeFee = false,
                        OperationId = Guid.NewGuid(),
                        ToAddress = wrongWalletAddressWithInvalidSymbol,
                        FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT
                    };
                       
                    var singleTransactionResponse = blockchainApi.Operations.PostTransactions(model);
                    var responseTransaction = singleTransactionResponse.GetResponseObject();

                    string operationId = model.OperationId.ToString();

                    var signResponse = blockchainSign.PostSign(new SignRequest()
                    {
                        PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY },
                        TransactionContext = responseTransaction.TransactionContext }
                    ).GetResponseObject();

                    var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });
                });

                var transferSupported = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTestingTransfersSupported;
                if (transferSupported != null && transferSupported.Value)
                {
                    Step("Make EW - Dw transfer using /testing/transfers", () =>
                    {
                        TestingTransferRequest request = new TestingTransferRequest() { amount = AMOUT_WITH_FEE, assetId = ASSET_ID, fromAddress = EXTERNAL_WALLET, fromPrivateKey = EXTERNAL_WALLET_KEY, toAddress = wallet.PublicAddress };
                        var response = blockchainApi.Testing.PostTestingTransfer(request);
                    });
                }
                else
                {
                    var usedOperationId = Guid.NewGuid();

                    Step("Make POST /transactions/broadcast and validate response status", () =>
                    {
                        var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(EXTERNAL_WALLET, EXTERNAL_WALLET_ADDRESS_CONTEXT, EXTERNAL_WALLET_KEY, wallet.PublicAddress, AMOUT_WITH_FEE);

                        while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                        {
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                            REBUILD_ATTEMPT_COUNT--;
                            (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(EXTERNAL_WALLET, EXTERNAL_WALLET_ADDRESS_CONTEXT, EXTERNAL_WALLET_KEY, wallet.PublicAddress, AMOUT_WITH_FEE);
                        }
                        response.Validate.StatusCode(HttpStatusCode.OK);
                        usedOperationId = operationId;
                    });

                    Step("Make GET /transactions/broadcast/single/{operationId} and validate response", () =>
                    {
                        var getResponse = blockchainApi.Operations.GetOperationId(usedOperationId.ToString());

                        Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(usedOperationId));

                        WaitForOperationGotCompleteStatus(usedOperationId.ToString());
                    });

                    Step("Validate balance records for wallet in GET /balances response. Validate wallet balance", () =>
                    {
                        Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Count(a => a.Address == wallet.PublicAddress), Is.EqualTo(1).After(2).Minutes.PollEvery(2).Seconds, $"Unexpected instances of balance for wallet {wallet.PublicAddress}");
                        Assert.That(() => blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.ToList().Find(a => a.Address == wallet.PublicAddress).Balance, Is.EqualTo(AMOUT_WITH_FEE), "Balance is not expected");
                    });
                }
            }
        }

        [NonParallelizable]
        public class HWTransactionsContainer
        {
            public class HWEWTransferDoubleBroadcast : BlockchainsIntegrationBaseTest
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
                public void HWEWTransferDoubleBroadcastTest()
                {
                    var usedOperationId = Guid.NewGuid();
                    var usedSignedTransaction = "";

                    Step("Perform BUILD, SIGN, BROADCAST HW - EW transaction", () =>
                    {
                        var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);

                        while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                        {
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                            REBUILD_ATTEMPT_COUNT--;
                            (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);
                        }
                        response.Validate.StatusCode(HttpStatusCode.OK);

                        response.Validate.StatusCode(HttpStatusCode.OK);
                        usedSignedTransaction = signedTransaction;
                        usedOperationId = operationId;
                    });

                    Step("Repeat Broadcast transaction with same parameters and validate status code is Conflict", () =>
                    {
                        var response1 = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = usedOperationId, SignedTransaction = usedSignedTransaction });

                        response1.Validate.StatusCode(HttpStatusCode.Conflict);
                    });
                }
            }

            public class HwEwTransfer : BlockchainsIntegrationBaseTest
            {
                [Test]
                [Category("BlockchainIntegration")]
                public void HwEwTransferTest()
                {
                    Assert.That(EXTERNAL_WALLET, Is.Not.Null.Or.Empty, "External wallet address and key are empty!");

                    Step("Make BUILD, SIGN, BROADCAST HW-EW operation", () =>
                    {
                        var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);

                        while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                        {
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                            REBUILD_ATTEMPT_COUNT--;
                            (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(HOT_WALLET, HOT_WALLET_CONTEXT, HOT_WALLET_KEY, EXTERNAL_WALLET);
                        }
                        response.Validate.StatusCode(HttpStatusCode.OK);

                        var getResponse = blockchainApi.Operations.GetOperationId(operationId.ToString());
                        Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(operationId));
                    });
                }
            }
        }

        [NonParallelizable]
        public class EWTransactionContainer
        {
            public class EWDWTransfer : BlockchainsIntegrationBaseTest
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
                public void EWDWTransferTest()
                {
                    Assert.That(EXTERNAL_WALLET, Is.Not.Null.Or.Empty, "External wallet address and key are empty!");

                    var transferSupported = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTestingTransfersSupported;
                    if (transferSupported != null && transferSupported.Value)
                    {
                        Step("Make EW - Dw transfer using /testing/transfers", () =>
                        {
                            TestingTransferRequest request = new TestingTransferRequest() { amount = AMOUNT, assetId = ASSET_ID, fromAddress = EXTERNAL_WALLET, fromPrivateKey = EXTERNAL_WALLET_KEY, toAddress = wallet.PublicAddress };
                            var response = blockchainApi.Testing.PostTestingTransfer(request);
                        });
                    }
                    else
                    {
                        var usedOperationId = Guid.NewGuid();

                        Step("Make POST /transactions/broadcast and validate response status", () =>
                        {
                            var (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(EXTERNAL_WALLET, EXTERNAL_WALLET_ADDRESS_CONTEXT, EXTERNAL_WALLET_KEY, wallet.PublicAddress);

                            while (response.StatusCode == HttpStatusCode.BadRequest && response.Content.Contains("buildingShouldBeRepeated") && REBUILD_ATTEMPT_COUNT > 0)
                            {
                                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                                REBUILD_ATTEMPT_COUNT--;
                                (response, operationId, transactionContext, signedTransaction) = BuildSignBroadcastSingleTransaction(wallet.PublicAddress, wallet.AddressContext, wallet.PrivateKey, HOT_WALLET);
                            }
                            response.Validate.StatusCode(HttpStatusCode.OK);
                            usedOperationId = operationId;
                        });

                        Step("Make GET /transactions/broadcast/single/{operationId} and validate response", () =>
                        {
                            var getResponse = blockchainApi.Operations.GetOperationId(usedOperationId.ToString());

                            Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(usedOperationId));
                        });
                    }
                }
            }
        }
    }
}
