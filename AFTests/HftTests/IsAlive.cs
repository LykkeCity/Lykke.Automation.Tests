using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.HftTests
{
    class IsAlive
    {
        public class GetIsAlive : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetIsAliveTest()
            {
                var response = hft.IsAlive.GetIsAlive();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Version, Is.Not.Null.Or.Empty, "Version is null or empty");
            }
        }
    }
}
