using BlockchainsIntegration.Sign;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.Tests;

namespace AFTests.BlockchainsIntegrationTests
{
    class BlockchainsIntegrationSignTests
    {
        public class PostWallet : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostWalletTest()
            {
                Step("Perform POST /wallets and validate response contains PublicAddress and PrivateKey", () => 
                {
                    var response = blockchainSign.PostWallet();
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    Assert.That(response.GetResponseObject().PublicAddress, Is.Not.Empty);
                    Assert.That(response.GetResponseObject().PrivateKey, Is.Not.Empty);
                });
            }
        }

        public class PostSign : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void PostSignTest()
            {
                Step("Create new wallet. Perform Sign request with invalid transactionContext. Validate statuscode is BadRequest", () => 
                {
                    var wallet = blockchainSign.PostWallet().GetResponseObject();

                    var req = new SignRequest()
                    {
                        PrivateKeys = new List<string>() { wallet.PrivateKey },
                        TransactionContext = "testHex"
                    };

                    var response = blockchainSign.PostSign(req);
                    response.Validate.StatusCode(HttpStatusCode.BadRequest);
                }); 
            }
        }
    }
}
