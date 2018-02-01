using AFTests.BlockchainsIntegrationTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegration
{
    class HistoryTests
    {
        string transaction = "3526277cff91afeeb6c7dac4052d800d3883a6b8546a3547b3fddc199cf83c94";


        public class GetHistoryFrom : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryFromTest()
            {
                var response = blockchainApi.History.GetHistoryFromToAddress("from", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Count, Is.EqualTo(0), "Unexpected count for invalid address");
            }
        }

        public class GetHistoryTo : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetHistoryToTest()
            {
                var response = blockchainApi.History.GetHistoryFromToAddress("to", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Count, Is.EqualTo(0), "Unexpected count for invalid address");
            }
        }

        public class PostHistoryTo : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryToTest()
            {
                var response = blockchainApi.History.PostHistoryFromToAddress("to", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostHistoryFrom : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostHistoryFromTest()
            {
                var response = blockchainApi.History.PostHistoryFromToAddress("from", WALLET_ADDRESS);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }
    }
}
