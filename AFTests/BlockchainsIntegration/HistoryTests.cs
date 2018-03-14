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
                var response = blockchainApi.Operations.GetTransactionHistorFromAddress(WALLET_ADDRESS, null);
                response.Validate.StatusCode(HttpStatusCode.BadRequest, "Take is required, Should be BadRequest");
            }
        }

        public class GetHistoryFrom : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryFromTest()
            {
                var response = blockchainApi.Operations.GetTransactionHistorFromAddress(WALLET_ADDRESS, "1");
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class GetHistoryTo : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryToTest()
            {
                var response = blockchainApi.Operations.GetTransactionHistorToAddress(WALLET_ADDRESS);
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
                blockchainApi.Operations.PostHistoryFromToAddress("to", WALLET_ADDRESS);
                var response = blockchainApi.Operations.PostHistoryFromToAddress("to", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.Conflict);
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
                //enable if disabled
                blockchainApi.Operations.PostHistoryFromToAddress("from", WALLET_ADDRESS);
                var response = blockchainApi.Operations.PostHistoryFromToAddress("from", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.Conflict);
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
