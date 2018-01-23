using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AFTests.PrivateApiTests
{
    class IsAlive : PrivateApiBaseTest
    {
        [Test]
        [Category("IsAlive"), Category("ClientAccount"), Category("ServiceAll")]
        public void IsAliveTest()
        {
            Assert.That(lykkeApi.ClientAccount.IsAlive.GetIsAlive().StatusCode,
                Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
