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
                Step("Make GET /isalive and validate response status code is OK ", () => 
                {
                    var response = blockchainApi.IsAlive.GetIsAlive();
                    response.Validate.StatusCode(HttpStatusCode.OK);
                });
            }
        }
    }
}
