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
            }
        }
    }
}
