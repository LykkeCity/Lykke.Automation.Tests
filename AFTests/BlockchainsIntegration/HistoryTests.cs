using AFTests.BlockchainsIntegrationTests;
using Lykke.Client.AutorestClient.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegrationTests
{
    class HistoryTests
    {
        public class GetHistoryFromTakeIsRequiredRequest : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            [Description("take is requered!")]
            public void GetHistoryFromTakeIsRequiredTest()
            {
                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;
                var response = blockchainApi.Operations.GetTransactionHistorFromAddress(newWallet, null);
                response.Validate.StatusCode(HttpStatusCode.BadRequest, "Take is required, Should be BadRequest");
            }
        }

        public class GetHistoryFrom : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryFromTest()
            {
                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                var response = blockchainApi.Operations.GetTransactionHistorFromAddress(newWallet, "1");
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class GetHistoryTo : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryToTest()
            {
                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                var response = blockchainApi.Operations.GetTransactionHistorToAddress(newWallet, "10");
                response.Validate.StatusCode(HttpStatusCode.OK);

                var empty = JsonConvert.DeserializeObject<TransactionHistory[]>("[]");
                Assert.That(JsonConvert.DeserializeObject<TransactionHistory[]>(response.Content), Is.EqualTo(empty), "Unexpected count for invalid address");
            }
        }

        public class PostHistoryToConflict : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryToConflictTest()
            {
                //enabled if disabled
                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                blockchainApi.Operations.PostHistoryFromToAddress("to", newWallet);
                var response = blockchainApi.Operations.PostHistoryFromToAddress("to", newWallet);
                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.Conflict, HttpStatusCode.OK), "Unexpected status code");
            }
        }

        public class PostHistoryTo : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryToTest()
            {
                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                var response = blockchainApi.Operations.PostHistoryFromToAddress("to", newWallet);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostHistoryFromConflict : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryFromConflictTest()
            {
                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                //enable if disabled
                blockchainApi.Operations.PostHistoryFromToAddress("from", newWallet);
                var response = blockchainApi.Operations.PostHistoryFromToAddress("from", newWallet);
                Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.Conflict, HttpStatusCode.OK));
            }
        }

        public class PostHistoryFrom : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryFromTest()
            {
                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                var response = blockchainApi.Operations.PostHistoryFromToAddress("from", newWallet);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }
    }
}
