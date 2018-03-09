using AFTests.BlockchainsIntegrationTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegration
{
    class CapabilitiesTests 
    {
        public class GetCapabilities : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetCapabilitiesTest()
            {
                var response = blockchainApi.Capabilities.GetCapabilities();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().IsTransactionsRebuildingSupported, Is.False.Or.False);
            }
        }
    }
}
