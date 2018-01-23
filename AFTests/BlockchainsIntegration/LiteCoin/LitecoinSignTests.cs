using BlockchainsIntegration.LitecoinSign;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.Tests;

namespace AFTests.BlockchainsIntegration.LiteCoin
{
    class LitecoinSignTests
    {
        public class GetLitecoinSignAlive : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void GetLitecoinSignAliveTest()
            {
                var signService = new LitecoinSign();

                var response = signService.GetIsAlive();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Name, Does.Contain("Sign"));
                Assert.That(response.GetResponseObject().Version, Is.Not.Null);
            }
        }


        public class PostWallet : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void PostWalletTest()
            {
                var signService = new LitecoinSign();

                var response = signService.PostWallet();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().PublicAddress, Is.Not.Empty);
                Assert.That(response.GetResponseObject().PrivateKey, Is.Not.Empty);
            }
        }

        public class PostSign : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void PostSignTest()
            {
                var signService = new LitecoinSign();

                var req = new SignRequest()
                {
                    PrivateKeys = new List<string>() { PKey },
                    TransactionHex = "testHex"
                };

                var response = signService.PostSign(req);
            }
        }
    }
}
