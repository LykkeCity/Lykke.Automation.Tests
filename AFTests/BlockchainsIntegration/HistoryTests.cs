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
        public class HistoryBaseTest : BlockchainsIntegrationBaseTest
        {
            [SetUp]
            public void SkipHistoryTests()
            {
                if (SKIP_HISTORY_TESTS)
                    Assert.Ignore("History tests are skipped. if you want to run - set 'SkipHistoryTests' blockChain option to 'false'");
            }
        }
        public class GetHistoryFromTakeIsRequiredRequest : HistoryBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            [Description("take is requered!")]
            public void GetHistoryFromTakeIsRequiredTest()
            {
                Step($"Make GT /transactions/history/to/<address> without take and validate response status is BadRequest", ()=>
                {
                    var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;
                    var response = blockchainApi.Operations.GetTransactionHistorFromAddress(newWallet, null);
                    response.Validate.StatusCode(HttpStatusCode.BadRequest, "Take is required, Should be BadRequest");
                });
            }
        }

        public class GetHistoryFrom : HistoryBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryFromTest()
            {
                Step("Make GET /transactions/history/to/{address} with valid parameters and validate Status code is OK", () => 
                {
                    var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                    var response = blockchainApi.Operations.GetTransactionHistorFromAddress(newWallet, "1");
                    response.Validate.StatusCode(HttpStatusCode.OK);
                });
            }
        }

        public class GetHistoryTo : HistoryBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryToTest()
            {
                Step("Make GET /transactions/history/to/{address} to wallet without transaction and validate response", () => 
                {
                    var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                    var response = blockchainApi.Operations.GetTransactionHistorToAddress(newWallet, "10");
                    response.Validate.StatusCode(HttpStatusCode.OK);

                    var empty = JsonConvert.DeserializeObject<TransactionHistory[]>("[]");
                    Assert.That(JsonConvert.DeserializeObject<TransactionHistory[]>(response.Content), Is.EqualTo(empty), "Unexpected count for invalid address");
                });
            }
        }

        public class PostHistoryToConflict : HistoryBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryToConflictTest()
            {
                Step("Validate, that double POST /transactions/history/{To}/{address}/observation produce status code Conflict or OK", () => 
                {
                    var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                    blockchainApi.Operations.PostHistoryFromToAddress("to", newWallet);
                    var response = blockchainApi.Operations.PostHistoryFromToAddress("to", newWallet);
                    Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.Conflict, HttpStatusCode.OK), "Unexpected status code");
                });
            }
        }

        public class PostHistoryTo : HistoryBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryToTest()
            {
                Step("Validate POST /transactions/history/To/{address}/observation with valid parameters and validate response status code is OK", () => 
                {
                    var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                    var response = blockchainApi.Operations.PostHistoryFromToAddress("to", newWallet);
                    response.Validate.StatusCode(HttpStatusCode.OK);
                });
            }
        }

        public class PostHistoryFromConflict : HistoryBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryFromConflictTest()
            {
                Step("Validate Double POST /transactions/history/from/{address}/observation with valid parameters and validate response status code is Conflict or OK", () =>
                {
                    var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                    blockchainApi.Operations.PostHistoryFromToAddress("from", newWallet);
                    var response = blockchainApi.Operations.PostHistoryFromToAddress("from", newWallet);
                    Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.Conflict, HttpStatusCode.OK));
                }); 
            }
        }

        public class PostHistoryFrom : HistoryBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryFromTest()
            {
                Step("Validate POST /transactions/history/from/{address}/observation with valid parameters and validate response status code is OK", () =>
                {
                    var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                    var response = blockchainApi.Operations.PostHistoryFromToAddress("from", newWallet);
                    response.Validate.StatusCode(HttpStatusCode.OK);
                });      
            }
        }
    }
}
