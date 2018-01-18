using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.BlockchainsIntegration.LiteCoin
{
    class OperationsTests
    {

        public class GetOperationId : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void GetOperationIdTest()
            {
                Assert.Ignore("Get valid operation id");
                var response = litecoinApi.Operations.GetOperationId("testoperationId");
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class GetOperationIdInvalidId : LitecoinBaseTest
        {
            [TestCase("")]
            [TestCase("testId")]
            [TestCase("111222333")]
            [TestCase("!@%^&*(")]
            [Category("Litecoin")]
            public void GetOperationIdInvalidIdTest(string operationId)
            {
                var response = litecoinApi.Operations.GetOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class PostTransactions : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void PostTransactionsTest()
            {
                Assert.Ignore("How to get TO adress, From address, OperationID???");
                var model = new BuildTransactionRequest()
                {
                    Amount = "10",
                    AssetId = "LTC",
                    FromAddress = "testAddress",
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = "testAddress1"
                };

                var response = litecoinApi.Operations.PostTransactions(model);
            }
        }

        public class PostTransactionsInvalidAddress : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
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

                var response = litecoinApi.Operations.PostTransactions(model);
                response.Validate.StatusCode(HttpStatusCode.InternalServerError);
                Assert.That(response.Content, Does.Contain("Invalid ToAddress"));
            }
        }

        public class PostTransactionsEmptyObject : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void PostTransactionsEmptyObjectTest()
            {
                var model = new BuildTransactionRequest()
                {
                };

                var response = litecoinApi.Operations.PostTransactions(model);
                response.Validate.StatusCode(HttpStatusCode.InternalServerError);
                Assert.That(response.Content, Does.Contain("Technical"));
            }
        }

        public class PostTransactionsBroadcast : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void PostTransactionsBroadcastTest()
            {
                Assert.Ignore("How to get valid OperationId and sTransaction???");
                string operationId = "1234566";
                string sTransaction = Guid.NewGuid().ToString("N");

                var response = litecoinApi.Operations.PostTransactionsBroadcast(operationId, sTransaction);

                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostTransactionsBroadcastInvalidTransaction : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void PostTransactionsBroadcastInvalidTransactionTest()
            {
                string operationId = "1234566";
                string sTransaction = Guid.NewGuid().ToString("N");

                var response = litecoinApi.Operations.PostTransactionsBroadcast(operationId, sTransaction);

                response.Validate.StatusCode(HttpStatusCode.InternalServerError);
                Assert.That(response.Content, Does.Contain("Invalid transaction hex"));
            }
        }

        public class DeleteOperationId : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void DeleteOperationIdTest()
            {
                Assert.Ignore("How to get valid OperationId");
                var response = litecoinApi.Operations.DeleteOperationId("testOperationId");
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class DeleteOperationIdInvalidOId : LitecoinBaseTest
        {
            [TestCase("")]
            [TestCase("testOId")]
            [TestCase("1234")]
            [TestCase("!@%^&*()")]
            [Category("Litecoin")]
            public void DeleteOperationIdInvalidOIdTest(string operationId)
            {
                var response = litecoinApi.Operations.DeleteOperationId(operationId);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
