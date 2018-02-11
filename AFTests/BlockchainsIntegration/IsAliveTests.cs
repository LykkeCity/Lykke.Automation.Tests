using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegrationTests
{
    class IsAliveTests
    {
        public class GetIsAlive : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetIsAliveTest()
            {
                var response = blockchainApi.IsAlive.GetIsAlive();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Version, Is.Not.Null, "Unexpected Version");
                Assert.That(response.GetResponseObject().Name.ToLower(), Does.Contain(SpecificBlockchain().ToLower()), "Unexpected Name");
            }
        }
    }
}
