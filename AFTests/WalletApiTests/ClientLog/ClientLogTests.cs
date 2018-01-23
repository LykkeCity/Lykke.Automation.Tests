using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.ClientLog
{
    class ClientLogTests
    {
        public class PostClientLog : WalletApiBaseTest
        {
            [Test]
            [Description("Get test logic??")]
            [Category("WalletApi")]
            public void PostClientLogTest()
            {
                var response = walletApi.ClientLog.PostClientLog(new WriteClientLogModel() { Data = TestData.GenerateString(12) });
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostClientLogEmpty : WalletApiBaseTest
        {
            [Test]
            [Description("Get test logic??")]
            [Category("WalletApi")]
            public void PostClientLogEmptyTest()
            {
                var response = walletApi.ClientLog.PostClientLog(new WriteClientLogModel() {});
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }
    }
}
