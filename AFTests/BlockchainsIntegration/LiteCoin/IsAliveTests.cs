using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegration.LiteCoin
{
    class IsAliveTests
    {
        public class GetIsAlive : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void GetIsAliveTest()
            {
                var response = litecoinApi.IsAlive.GetIsAlive();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Version, Is.Not.Null, "Unexpected Version");
                Assert.That(response.GetResponseObject().Name, Does.Contain("LiteCoin"), "Unexpected Name");
            }
        }
    }
}
