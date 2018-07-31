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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void GetOperationIdTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString(); // Stefan/ with dashes

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                var getResponse = blockchainApi.Operations.GetOperationId(operationId);

                Assert.That(() => blockchainApi.Operations.GetOperationId(operationId).StatusCode, Is.EqualTo(HttpStatusCode.OK).After((int)BLOCKCHAIN_MINING_TIME * 60*1000, 1*1000));

                Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(model.OperationId));
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
                var response = blockchainApi.Operations.GetOperationId(operationId);

                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed));
            }
        }

        public class PostTransactionsInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsInvalidAddressTest()
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
            }
        }

        public class PostTransactionsEmptyObject : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsEmptyObjectTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                };

                var response = blockchainApi.Operations.PostTransactions(model);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsBroadcastTest()
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
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() {PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response.Validate.StatusCode(HttpStatusCode.OK);
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsBroadcastInvalidTransactionTest()
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

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                string sTransaction = Guid.NewGuid().ToString();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest()
                { OperationId = model.OperationId, SignedTransaction = sTransaction });

                response.Validate.StatusCode(HttpStatusCode.BadRequest);
                Assert.That(response.Content, Does.Contain("errorMessage").IgnoreCase);
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DeleteOperationIdTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response.Validate.StatusCode(HttpStatusCode.OK);

                var responseDelete = blockchainApi.Operations.DeleteOperationId(operationId);
                responseDelete.Validate.StatusCode(HttpStatusCode.OK);
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
                var response = blockchainApi.Operations.DeleteOperationId(operationId);
                Assert.That(response.StatusCode, Is.AnyOf(new object[] { HttpStatusCode.BadRequest, HttpStatusCode.NoContent, HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed }));
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

                var response = blockchainApi.Operations.GetTransactionsManyOutputs(operationId);
                Assert.That(new object[] { HttpStatusCode.NoContent, HttpStatusCode.NotImplemented, HttpStatusCode.BadRequest, HttpStatusCode.MethodNotAllowed, HttpStatusCode.NotFound }, Does.Contain(response.StatusCode));
            }
        }

        public class GetTransactionsManyOutputs: BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyOutputsTest()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many outputs are not supported by blockchain");

                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null, $"Wallet {wallet.PublicAddress} balance is null. Fail test");

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                var request = new BuildTransactionWithManyOutputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = model.OperationId,
                    FromAddress = wallet.PublicAddress,
                    Outputs = new List<TransactionOutputContract>() { new TransactionOutputContract() { Amount = AMOUNT, ToAddress = HOT_WALLET } }
                };

                var response = blockchainApi.Operations.PostTransactionsManyOutputs(request);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = response.GetResponseObject().TransactionContext, PrivateKeys = new List<string>() { wallet.PrivateKey } });

                var broadcastRequset = new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
                var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);

                var getResponse = blockchainApi.Operations.GetTransactionsManyOutputs(model.OperationId.ToString());
                getResponse.Validate.StatusCode(HttpStatusCode.OK);
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

                var response = blockchainApi.Operations.GetTransactionsManyInputs(operationId);
                Assert.That(response.StatusCode, Is.AnyOf(new object[]
                {
                    HttpStatusCode.NoContent, HttpStatusCode.NotImplemented, HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.MethodNotAllowed
                }));
            }
        }

        public class GetTransactionsManyInputs : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyInputsTest()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs are not supported by blockchain");

                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null, $"Wallet {wallet.PublicAddress} balance is null. Fail test");

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                var request = new BuildTransactionWithManyInputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = model.OperationId,
                    ToAddress = HOT_WALLET,
                    Inputs = new List<TransactionInputContract>() { new TransactionInputContract()
                    {
                        Amount = AMOUNT, FromAddress = wallet.PublicAddress
                    } }
                };

                var response = blockchainApi.Operations.PostTransactionsManyInputs(request);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = response.GetResponseObject().TransactionContext, PrivateKeys = new List<string>() { wallet.PrivateKey } });

                var broadcastRequset = new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
                var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);

                var getResponse = blockchainApi.Operations.GetTransactionsManyInputs(model.OperationId.ToString());
                getResponse.Validate.StatusCode(HttpStatusCode.OK);

            }
        }

        //post many inputs

        public class PostTransactionsManyInputsInvalidOperationId : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void PostTransactionsManyInputsInvalidOperationIdTest(string operationId)
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs not supported by blockchain");

                var request = new BuildTransactionWithManyInputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    Inputs = new List<TransactionInputContract>() { new TransactionInputContract()
                    {
                        Amount = AMOUNT,
                        FromAddress = wallet.PublicAddress
                    } }
                };

                var json = JsonConvert.SerializeObject(request);
                json = json.Replace(request.OperationId.ToString(), operationId);

                var response = blockchainApi.Operations.PostTransactionsManyInputs(json);
                Assert.That(new ArrayList() {HttpStatusCode.BadRequest, HttpStatusCode.NotImplemented }, Has.Member(response.StatusCode));
            }
        }

        public class EWDWTransfer : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void EWDWTransferTest()
            {
                Assert.That(EXTERNAL_WALLET, Is.Not.Null.Or.Empty, "External wallet address and key are empty!");

                blockchainApi.Balances.PostBalances(wallet.PublicAddress);

                var transferSupported = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTestingTransfersSupported;
                if (transferSupported!= null && transferSupported.Value)
                {
                    TestingTransferRequest request = new TestingTransferRequest() { amount = AMOUNT, assetId = ASSET_ID, fromAddress = EXTERNAL_WALLET, fromPrivateKey = EXTERNAL_WALLET_KEY, toAddress = wallet.PublicAddress };
                    var response = blockchainApi.Testing.PostTestingTransfer(request);
                }
                else
                {
                    var model = new BuildSingleTransactionRequest()
                    {
                        Amount = AMOUNT,
                        AssetId = ASSET_ID,
                        FromAddress = EXTERNAL_WALLET,
                        IncludeFee = false,
                        OperationId = Guid.NewGuid(),
                        ToAddress = wallet.PublicAddress,
                        FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT
                    };

                    var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                    string operationId = model.OperationId.ToString();

                    var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                    var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                    var getResponse = blockchainApi.Operations.GetOperationId(operationId);
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(model.OperationId));
                }
            }
        }

        public class HwEwTransfer : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void HwEwTransferTest()
            {
                Assert.That(EXTERNAL_WALLET, Is.Not.Null.Or.Empty, "External wallet address and key are empty!");

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = HOT_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = EXTERNAL_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { HOT_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                var getResponse = blockchainApi.Operations.GetOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(model.OperationId));
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
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

                var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                var secondResponseTransaction = blockchainApi.Operations.PostTransactions(model);

                responseTransaction.Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(responseTransaction.GetResponseObject().TransactionContext, Is.Not.Null.Or.Empty, "Transaction Context is null");
                Assert.That(secondResponseTransaction.GetResponseObject().TransactionContext, Is.Not.Null.Or.Empty, "Transaction Context is null");
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            [Description("Transfer all balance from DW to EW")]
            public void DWHWTransferTest()
            {
                Assert.That(HOT_WALLET, Is.Not.Null.Or.Empty, "Hot wallet address and key are empty!");

                blockchainApi.Balances.PostBalances(wallet.PublicAddress);
                var take = "500";

                var currentBalance = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                Assert.That(currentBalance, Is.Not.Null, $"{wallet.PublicAddress} does not present in GetBalances or its balance is null");

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = currentBalance,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = true,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                var getResponse = blockchainApi.Operations.GetOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(model.OperationId));
                
                WaitForOperationGotCompleteStatus(operationId);
                
                if(getResponse.GetResponseObject().State == BroadcastedTransactionState.Completed)
                {
                    Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.EqualTo("0").Or.Null.After((int)BLOCKCHAIN_MINING_TIME*60 * 1000, 2 * 1000), $"Unexpected balance for wallet {wallet.PublicAddress}");
                }
                else
                {
                    //validate balance is 0 or null
                    Assert.That(() => blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.EqualTo("0").Or.Null.After((int)BLOCKCHAIN_MINING_TIME * 60 * 1000, 2 * 1000), $"Unexpected balance for wallet {wallet.PublicAddress}");
                }   
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWHWTransferDoubleBroadcastTest()
            {
                string take = "500";

                var currentBalance = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = currentBalance,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = true,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });
                response.Validate.StatusCode(HttpStatusCode.OK);

                var response1 = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response1.Validate.StatusCode(HttpStatusCode.Conflict);
            }
        }

        public class HWEWTransferDoubleBroadcast : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void HWEWTransferDoubleBroadcastTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = HOT_WALLET,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = EXTERNAL_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { HOT_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response.Validate.StatusCode(HttpStatusCode.OK);

                var response1 = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response1.Validate.StatusCode(HttpStatusCode.Conflict);
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWHWPartialTransferDoubleBroadcastTest()
            {
                string take = "500";

                var currentBalance = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                var startBlock = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Block;

                var partialBalance = Math.Round(long.Parse(currentBalance) * 0.9).ToString();

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = partialBalance,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    IncludeFee = true,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    FromAddressContext = wallet.AddressContext
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { wallet.PrivateKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });
                response.Validate.StatusCode(HttpStatusCode.OK);

                var response1 = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response1.Validate.StatusCode(HttpStatusCode.Conflict);

                WaitForOperationGotCompleteStatus(model.OperationId.ToString());

                WaitForBalanceBlockIncreased(wallet.PublicAddress, startBlock.Value);

                var balanceAfterTransaction = blockchainApi.Balances.GetBalances(take, null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance;

                Assert.That(balanceAfterTransaction, Is.EqualTo(Math.Round(long.Parse(currentBalance) * 0.1).ToString()), "Unexpected Balance after partial transaction");
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
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DoublePostTransactionSingleReturnConflictTest()
            {
                //
                var operationId = Guid.NewGuid();

                var modelSingle = new BuildSingleTransactionRequest
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    FromAddressContext = wallet.AddressContext,
                    OperationId = operationId,
                    ToAddress = HOT_WALLET,
                    IncludeFee = false
                };
                var request = blockchainApi.Operations.PostTransactions(modelSingle);
                var sign = blockchainSign.PostSign(new SignRequest { TransactionContext = request.GetResponseObject().TransactionContext, PrivateKeys = new List<string> { wallet.PrivateKey } });
                var broadcast = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest { OperationId = operationId, SignedTransaction = sign.GetResponseObject().SignedTransaction });
                Assert.That(broadcast.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //

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

                var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));    
            }
        }

        public class DoublePostTransactionSingleRecieveReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DoublePostTransactionSingleRecieveConflictTest()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired.HasValue &&
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsReceiveTransactionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not require recieve transaction");
                //
                var operationId = Guid.NewGuid();

                var model = new BuildSingleTransactionRequest
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    FromAddressContext = wallet.AddressContext,
                    OperationId = operationId,
                    ToAddress = HOT_WALLET,
                    IncludeFee = false
                };
                var request = blockchainApi.Operations.PostTransactions(model);
                var sign = blockchainSign.PostSign(new SignRequest {TransactionContext = request.GetResponseObject().TransactionContext, PrivateKeys = new List<string> { wallet.PrivateKey} });
                var broadcast = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest {OperationId = operationId, SignedTransaction = sign.GetResponseObject().SignedTransaction });
                Assert.That(broadcast.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //
                var recieve = new BuildSingleReceiveTransactionRequest
                {
                    operationId = operationId,
                    sendTransactionHash = ""
                };

                var responseTransaction = blockchainApi.Operations.PostTranstactionSingleRecieve(recieve);
                Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            }
        }

        public class DoublePostTransactionManyInputsReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DoublePostTransactionManyInputsReturnConflictTest()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported.HasValue &&
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support many-inputs transaction");

                //
                var operationId = Guid.NewGuid();

                var modelSingle = new BuildSingleTransactionRequest
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    FromAddressContext = wallet.AddressContext,
                    OperationId = operationId,
                    ToAddress = HOT_WALLET,
                    IncludeFee = false
                };
                var request = blockchainApi.Operations.PostTransactions(modelSingle);
                var sign = blockchainSign.PostSign(new SignRequest { TransactionContext = request.GetResponseObject().TransactionContext, PrivateKeys = new List<string> { wallet.PrivateKey } });
                var broadcast = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest { OperationId = operationId, SignedTransaction = sign.GetResponseObject().SignedTransaction });
                Assert.That(broadcast.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //

                var model = new BuildTransactionWithManyInputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = operationId,
                    ToAddress = EXTERNAL_WALLET,
                    Inputs = new List<TransactionInputContract> { new TransactionInputContract { Amount = AMOUNT, FromAddress = HOT_WALLET } }
                };

                var responseTransaction = blockchainApi.Operations.PostTransactionsManyInputs(model);
                Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));         
            }
        }

        public class DoublePostTransactionManyOutputsReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DoublePostTransactionManyOutputsReturnConflictTest()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported.HasValue &&
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support many-outputs transaction");

                //
                var operationId = Guid.NewGuid();

                var modelSingle = new BuildSingleTransactionRequest
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    FromAddressContext = wallet.AddressContext,
                    OperationId = operationId,
                    ToAddress = HOT_WALLET,
                    IncludeFee = false
                };
                var request = blockchainApi.Operations.PostTransactions(modelSingle);
                var sign = blockchainSign.PostSign(new SignRequest { TransactionContext = request.GetResponseObject().TransactionContext, PrivateKeys = new List<string> { wallet.PrivateKey } });
                var broadcast = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest { OperationId = operationId, SignedTransaction = sign.GetResponseObject().SignedTransaction });
                Assert.That(broadcast.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //

                var model = new BuildTransactionWithManyOutputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = operationId,
                    FromAddress = HOT_WALLET,
                    Outputs = new List<TransactionOutputContract> { new TransactionOutputContract { Amount = AMOUNT, ToAddress = HOT_WALLET } }
                };

                var responseTransaction = blockchainApi.Operations.PostTransactionsManyOutputs(model);
                Assert.That(responseTransaction.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));             
            }
        }

        public class DoublePutTransactionReturnConflict : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DoublePutTransactionReturnConflictTest()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTransactionsRebuildingSupported.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTransactionsRebuildingSupported.HasValue &&
    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTransactionsRebuildingSupported.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support PUT /transaction");

                //
                var operationId = Guid.NewGuid();

                var modelSingle = new BuildSingleTransactionRequest
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = wallet.PublicAddress,
                    FromAddressContext = wallet.AddressContext,
                    OperationId = operationId,
                    ToAddress = HOT_WALLET,
                    IncludeFee = false
                };
                var request = blockchainApi.Operations.PostTransactions(modelSingle);
                var sign = blockchainSign.PostSign(new SignRequest { TransactionContext = request.GetResponseObject().TransactionContext, PrivateKeys = new List<string> { wallet.PrivateKey } });
                var broadcast = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest { OperationId = operationId, SignedTransaction = sign.GetResponseObject().SignedTransaction });
                Assert.That(broadcast.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //

                var model = new RebuildTransactionRequest()
                {
                    OperationId = operationId,
                    FeeFactor = 1
                };

                var responseTransaction = blockchainApi.Operations.PutTransactions(model);
                Assert.That(responseTransaction.StatusCode, Is.AnyOf(HttpStatusCode.Conflict, HttpStatusCode.NotImplemented));        
            }
        }

        // security

        public class DWHWWithoutHWKey : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWHWWithoutHWKeyTest()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue &&
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support address context");

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
                var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                var signResponse = blockchainSign.PostSign(new SignRequest { PrivateKeys = new List<string> { "fake_deposit_key" }, TransactionContext = responseTransaction.GetResponseObject().TransactionContext });

                Assert.That(signResponse.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK), $"DW HW transaction has been successfully signed with DW pkey {key} for blockchains with address extension");
            }
        }

        public class DWHWWithHWKey : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWHWWithHWKeyTest()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue &&
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support address context");

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

                var responseTransaction = blockchainApi.Operations.PostTransactions(model);
                var signResponse = blockchainSign.PostSign(new SignRequest { PrivateKeys = new List<string> { HOT_WALLET_KEY }, TransactionContext = responseTransaction.GetResponseObject().TransactionContext });

                Assert.That(signResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "DW HW transaction has NOT been successfully signed with DW pkey for blockchains with address extension");
            }
        }

        public class DWDWWithoutHWKey : BlockchainsIntegrationBaseTest
        {
            WalletCreationResponse wallet;
            WalletCreationResponse wallet1;

            [SetUp]
            public void SetUp()
            {
                wallet = Wallets().Dequeue();
                TestContext.Out.WriteLine($"wallet {wallet.PublicAddress} balance: {blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance}");
                Assert.That(blockchainApi.Balances.GetBalances("500", null).GetResponseObject().Items.FirstOrDefault(w => w.Address == wallet.PublicAddress)?.Balance, Is.Not.Null.Or.Empty.And.Not.EqualTo("0"), "Unxpected balance");
                wallet1 = Wallets().Dequeue();
            }

            [TearDown]
            public void TearDown()
            {
                blockchainApi.Balances.DeleteBalances(wallet.PublicAddress);
            }

            [Test]
            [Category("BlockchainIntegration")]
            public void DWDWWithoutHWKeyTest()
            {
                if (!blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue || (blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.HasValue &&
                    !blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired.Value))
                    Assert.Ignore($"Blockchain {BlockChainName} does not support address context");

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

                var responseTransaction = blockchainApi.Operations.PostTransactions(model);

                var key = blockchainSign.PostWallet().GetResponseObject().PrivateKey;
                var signResponse = blockchainSign.PostSign(new SignRequest { PrivateKeys = new List<string> { key }, TransactionContext = responseTransaction.GetResponseObject().TransactionContext });

                Assert.That(signResponse.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK), "DW DW transaction has been successfully signed with DW pkey for blockchains with address extension");
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

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = AMOUNT,
                    AssetId = ASSET_ID,
                    FromAddress = HOT_WALLET,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = EXTERNAL_WALLET,
                    FromAddressContext = HOT_WALLET_CONTEXT
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { HOT_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(SIGN_EXPIRATION_SECONDS));

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }
    }
}
