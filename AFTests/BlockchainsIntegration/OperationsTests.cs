using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.BlockchainsIntegrationTests.LiteCoin
{
    class OperationsTests
    {

        public class GetOperationId : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetOperationIdTest()
            {
                var model = new BuildTransactionRequest()
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
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }

        public class PostTransactions : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostTransactionsTest()
            {
                var model = new BuildTransactionRequest()
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
                var model = new BuildTransactionRequest()
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
                var model = new BuildTransactionRequest()
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
                var model = new BuildTransactionRequest()
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
                var model = new BuildTransactionRequest()
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
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
