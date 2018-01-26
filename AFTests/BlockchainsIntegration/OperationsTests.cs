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
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void GetOperationIdTest()
            {
                var model = new BuildTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = "LTC",
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
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void GetOperationIdInvalidIdTest(string operationId)
            {
                var response = blockchainApi.Operations.GetOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }

        public class PostTransactions : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void PostTransactionsTest()
            {
                var model = new BuildTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = "LTC",
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
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void PostTransactionsInvalidAddressTest()
            {
                var model = new BuildTransactionRequest()
                {
                    Amount = "10",
                    AssetId = "LTC",
                    FromAddress = "testAddress",
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = "testAddress1"
                };

                var response = blockchainApi.Operations.PostTransactions(model);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
                Assert.That(response.Content, Does.Contain("Invalid ToAddress"));
            }
        }

        public class PostTransactionsEmptyObject : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void PostTransactionsEmptyObjectTest()
            {
                var model = new BuildTransactionRequest()
                {
                };

                var response = blockchainApi.Operations.PostTransactions(model);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
                Assert.That(response.Content, Does.Contain("Technical"));
            }
        }

        public class PostTransactionsBroadcast : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void PostTransactionsBroadcastTest()
            {
                var model = new BuildTransactionRequest()
                {
                    Amount = "100002",
                    AssetId = "LTC",
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
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void PostTransactionsBroadcastInvalidTransactionTest()
            {
                string sTransaction = Guid.NewGuid().ToString("N");

                var response = blockchainApi.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest()
                { OperationId = Guid.NewGuid(), SignedTransaction = sTransaction });

                response.Validate.StatusCode(HttpStatusCode.BadRequest);
                Assert.That(response.Content, Does.Contain("Invalid transaction"));
            }
        }

        public class DeleteOperationId : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void DeleteOperationIdTest()
            {
                var model = new BuildTransactionRequest()
                {
                    Amount = "100001",
                    AssetId = "LTC",
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
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void DeleteOperationIdInvalidOIdTest(string operationId)
            {
                var response = blockchainApi.Operations.DeleteOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
