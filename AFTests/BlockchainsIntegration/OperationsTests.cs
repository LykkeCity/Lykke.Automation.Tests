using Lykke.Client.AutorestClient.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

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
                    AssetId = CurrentAssetId(),
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString("N");

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
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class PostTransactions : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = CurrentAssetId(),
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var response = blockchainApi.Operations.PostTransactions(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().TransactionContext, Is.Not.Null);
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
                    AssetId = CurrentAssetId(),
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
                Assert.That(response.Content, Does.Contain("errorMessage").IgnoreCase);
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
                    AssetId = CurrentAssetId(),
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
                string sTransaction = Guid.NewGuid().ToString("N");

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
                    AssetId = CurrentAssetId(),
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
            [Category("BlockchainIntegration")]
            public void DeleteOperationIdInvalidOIdTest(string operationId)
            {
                var response = blockchainApi.Operations.DeleteOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
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
                var response = blockchainApi.Operations.GetTransactionsManyOutputs(operationId);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class GetTransactionsManyOutputs: BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyOutputsTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = CurrentAssetId(),
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString("N");

                var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = responseTransaction.TransactionContext, PrivateKeys = new List<string>() { PKey } });

                var broadcastRequset = new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
                var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);

                var response = blockchainApi.Operations.GetTransactionsManyOutputs(operationId);
                response.Validate.StatusCode(HttpStatusCode.OK);
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
                var response = blockchainApi.Operations.GetTransactionsManyInputs(operationId);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class GetTransactionsManyInputs : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetTransactionsManyInputsTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = CurrentAssetId(),
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString("N");

                var signResponse = blockchainSign.PostSign(new SignRequest() { TransactionContext = responseTransaction.TransactionContext, PrivateKeys = new List<string>() { PKey } });

                var broadcastRequset = new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.GetResponseObject().SignedTransaction };
                var broadcatedResponse = blockchainApi.Operations.PostTransactionsBroadcast(broadcastRequset);

                var response = blockchainApi.Operations.GetTransactionsManyInputs(operationId);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }


        //post many inputs
        public class PostTransactionsManyInputs : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsManyInputsTest()
            {
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = CurrentAssetId(),
                    FromAddress = WALLET_ADDRESS,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = HOT_WALLET
                };

                var responseTransaction = blockchainApi.Operations.PostTransactions(model).GetResponseObject();

                var request = new BuildTransactionWithManyInputsRequest()
                {
                    AssetId = ASSET_ID,
                    OperationId = model.OperationId,
                    ToAddress = HOT_WALLET,
                    Inputs = new List<TransactionInputContract>() { new TransactionInputContract() {Amount = "100001", FromAddress = WALLET_ADDRESS } }
                };

                var response = blockchainApi.Operations.PostTransactionsManyInputs(request);
                response.Validate.StatusCode(HttpStatusCode.OK);

                //??
                //var responseManyInputs = blockchainApi.Operations.GetTransactionsManyInputs(model.OperationId.ToString("N"));
                //response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostTransactionsManyInputsInvalidOperationId : BlockchainsIntegrationBaseTest
        {
            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("BlockchainIntegration")]
            public void PostTransactionsManyInputsInvalidOperationIdTest(string operationId)
            {
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
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }
    }
}
