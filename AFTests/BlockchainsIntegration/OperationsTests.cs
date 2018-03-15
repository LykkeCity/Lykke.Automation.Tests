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

namespace AFTests.BlockchainsIntegrationTests
{
    class OperationsTests
    {

        public class GetOperationId : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetOperationIdTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = ASSET_ID,
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString(); // Stefan/ with dashes

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { PKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                var getResponse = blockchainApi.Operations.GetOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.OK);
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

                Assert.That(new ArrayList() { HttpStatusCode.BadRequest, HttpStatusCode.NotFound }, Has.Member(response.StatusCode));
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
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsBroadcastTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100002",
                    AssetId = ASSET_ID,
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = true,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString("N");

                var signResponse = blockchainSign.PostSign(new SignRequest() {PrivateKeys = new List<string>() { PKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostTransactionsBroadcastInvalidTransaction : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsBroadcastInvalidTransactionTest()
            {
                string sTransaction = Guid.NewGuid().ToString();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest()
                { OperationId = Guid.NewGuid(), SignedTransaction = sTransaction });

                response.Validate.StatusCode(HttpStatusCode.BadRequest);
                Assert.That(response.Content, Does.Contain("errorMessage").IgnoreCase);
            }
        }

        public class DeleteOperationId : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void DeleteOperationIdTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = ASSET_ID,
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString("N");

                var signResponse = blockchainSign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { PKey }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                response.Validate.StatusCode(HttpStatusCode.OK);

                var responseDelete = blockchainApi.Operations.DeleteOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class DeleteOperationIdInvalidOId : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Description("Here two possible Status code: NoContent in case transaction not found and BadRequest in case operationId is not valid")]
            [Category("BlockchainIntegration")]
            public void DeleteOperationIdInvalidOIdTest(string operationId)
            {
                var response = blockchainApi.Operations.DeleteOperationId(operationId);
                Assert.That(new object[] { HttpStatusCode.BadRequest, HttpStatusCode.NoContent, HttpStatusCode.NotFound}, Has.Member(response.StatusCode));
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
                    Assert.Ignore("Many outputs are not supported by blockachain");

                var response = blockchainApi.Operations.GetTransactionsManyOutputs(operationId);
                Assert.That(new object[] { HttpStatusCode.NoContent, HttpStatusCode.NotImplemented, HttpStatusCode.BadRequest }, Does.Contain(response.StatusCode));
            }
        }

        public class GetTransactionsManyOutputs: BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyOutputsTest()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyOutputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many outputs are not supported by blockachain");

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = ASSET_ID,
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var request = new BuildTransactionWithManyOutputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = model.OperationId,
                    FromAddress = WALLET_ADDRESS,
                    Outputs = new List<TransactionOutputContract>() { new TransactionOutputContract() { Amount = "100001", ToAddress = HOT_WALLET } }
                };

                var response = blockchainApi.Operations.PostTransactionsManyOutputs(request);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = response.GetResponseObject().TransactionContext, PrivateKeys = new List<string>() { PKey } });

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
                    Assert.Ignore("Many inputs not supported by blockachain");

                var response = blockchainApi.Operations.GetTransactionsManyInputs(operationId);
                Assert.That(new object[] { HttpStatusCode.NoContent, HttpStatusCode.NotImplemented, HttpStatusCode.BadRequest, HttpStatusCode.NotFound }, Has.Member(response.StatusCode));
            }
        }

        public class GetTransactionsManyInputs : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyInputsTest()
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs are not supported by blockachain");

                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = ASSET_ID,
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var request = new BuildTransactionWithManyInputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = model.OperationId,
                    ToAddress = HOT_WALLET,
                    Inputs = new List<TransactionInputContract>() { new TransactionInputContract() { Amount = "100001", FromAddress = WALLET_ADDRESS } }
                };

                var response = blockchainApi.Operations.PostTransactionsManyInputs(request);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = response.GetResponseObject().TransactionContext, PrivateKeys = new List<string>() { PKey } });

                var broadcastRequset = new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
                var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);

                var getResponse = blockchainApi.Operations.GetTransactionsManyInputs(model.OperationId.ToString());
                getResponse.Validate.StatusCode(HttpStatusCode.OK);

            }
        }

        //post many inputs

        public class PostTransactionsManyInputsInvalidOperationId : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void PostTransactionsManyInputsInvalidOperationIdTest(string operationId)
            {
                var run = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().AreManyInputsSupported;
                if (!run.Value)
                    Assert.Ignore("Many inputs not supported by blockachain");

                var request = new BuildTransactionWithManyInputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET,
                    Inputs = new List<TransactionInputContract>() { new TransactionInputContract() { Amount = "100001", FromAddress = WALLET_ADDRESS } }
                };

                var json = JsonConvert.SerializeObject(request);
                json = json.Replace(request.OperationId.ToString(), operationId);

                var response = blockchainApi.Operations.PostTransactionsManyInputs(json);
                Assert.That(new ArrayList() {HttpStatusCode.BadRequest, HttpStatusCode.NotImplemented }, Has.Member(response.StatusCode));
            }
        }

        public class DeleteTranstactionsObservationFromInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void DeleteTranstactionsObservationFromInvalidAddressTest(string address)
            {
                var response = blockchainApi.Operations.DeleteTranstactionsObservationFromAddress(address);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class DeleteTranstactionsObservationFromValidAddress : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void DeleteTranstactionsObservationFromValidAddressTest()
            {
                var address = blockchainSign.PostWallet().GetResponseObject().PublicAddress;
                var response = blockchainApi.Operations.DeleteTranstactionsObservationFromAddress(address);
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }

        public class DeleteTranstactionsObservationToInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void DeleteTranstactionsObservationToInvalidAddressTest(string address)
            {
                var response = blockchainApi.Operations.DeleteTranstactionsObservationToAddress(address);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public class DeleteTranstactionsObservationToValidAddress : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void DeleteTranstactionsObservatioToValidAddressTest()
            {
                var address = blockchainSign.PostWallet().GetResponseObject().PublicAddress;
                var response = blockchainApi.Operations.DeleteTranstactionsObservationToAddress(address);
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }

        public class DeleteTranstactionsObservationFromValidAddressPositive : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void DeleteTranstactionsObservationFromValidAddressPositiveTest()
            {
                //remove in case it is enabled
                blockchainApi.Operations.DeleteTranstactionsObservationFromAddress(WALLET_ADDRESS);

                var response = blockchainApi.Operations.PostHistoryFromToAddress("from", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var delete = blockchainApi.Operations.DeleteTranstactionsObservationFromAddress(WALLET_ADDRESS);
                delete.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class DeleteTranstactionsObservationToValidAddressPositive : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void DeleteTranstactionsObservationToValidAddressPositiveTest()
            {
                //remove in case it is enabled
                blockchainApi.Operations.DeleteTranstactionsObservationToAddress(WALLET_ADDRESS);

                var response = blockchainApi.Operations.PostHistoryFromToAddress("to", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var delete = blockchainApi.Operations.DeleteTranstactionsObservationToAddress(WALLET_ADDRESS);
                delete.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class EWDWTransfer : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void EWDWTransferTest()
            {
                Assert.That(EXTERNAL_WALLET, Is.Not.Null.Or.Empty, "External wallet address and key are empty!");

                bool transferSupported = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsTestingTransfersSupported;
                if (transferSupported)
                {
                    blockchainApi.Balances.PostBalances(EXTERNAL_WALLET);
                    var balanceBefore = blockchainApi.Balances.GetBalances("1000", null).GetResponseObject().
                        Items.First(w => w.Address == EXTERNAL_WALLET).Balance;
                    TestingTransferRequest request = new TestingTransferRequest() { amount = "100001", assetId = ASSET_ID, fromAddress = EXTERNAL_WALLET, fromPrivateKey = EXTERNAL_WALLET_KEY, toAddress = WALLET_ADDRESS };
                    var response = blockchainApi.Testing.PostTestingTransfer(request);
                    Assert.That(() => long.Parse(blockchainApi.Balances.GetBalances("1000", null).GetResponseObject().
                        Items.First(w => w.Address == EXTERNAL_WALLET).Balance), Is.GreaterThan(long.Parse(balanceBefore)).After(5*60*1000, 1*1000), "Balance after 5 minute after transaction not greater then berfore transaction");
                }
                else
                {
                    var model = new BuildSingleTransactionRequest()
                    {
                        Amount = "100001",
                        AssetId = ASSET_ID,
                        FromAddress = EXTERNAL_WALLET,
                        IncludeFee = false,
                        OperationId = Guid.NewGuid(),
                        ToAddress = WALLET_ADDRESS
                    };

                    var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                    string operationId = model.OperationId.ToString("N");

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
                    Amount = "100001",
                    AssetId = ASSET_ID,
                    FromAddress = HOT_WALLET,
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
    }
}
