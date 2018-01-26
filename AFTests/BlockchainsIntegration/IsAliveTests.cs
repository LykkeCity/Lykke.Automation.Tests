using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegrationTests.LiteCoin
{
    class IsAliveTests
    {
        public class GetIsAlive : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void GetIsAliveTest()
            {
                var response = blockchainApi.IsAlive.GetIsAlive();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Version, Is.Not.Null, "Unexpected Version");
                Assert.That(response.GetResponseObject().Name, Does.Contain("LiteCoin"), "Unexpected Name");
            }
        }
    }
}
