using BlockchainsIntegration.Sign;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegrationTests
{
    class AddressTests
    {
        public class GetAddressInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("testAddress")]
            [TestCase("1234567")]
            [TestCase("!@$%^&*(")]
            [Category("BlockchainIntegration")]
            public void GetAddressInvalidAddressTest(string address)
            {
                Step($"Make GET request /addresses/{address}/validity", () =>
                {
                    var response = blockchainApi.Address.GetAddress(address);
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    Assert.That(response.GetResponseObject().IsValid, Is.False);
                });
            }
        }

        public class GetAddressValidAddress : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetAddressValidAddressTest()
            {
                var address = "";
                Step("Create wallet with POST /wallets", () => 
                {
                    var wallet = blockchainSign.PostWallet();
                    wallet.Validate.StatusCode(HttpStatusCode.OK);
                    address = wallet.GetResponseObject().PublicAddress;
                });

                Step($"Make GET request /addresses/{address}/validity and check that address is Valid", () => 
                {
                    var response = blockchainApi.Address.GetAddress(address);
                    response.Validate.StatusCode(HttpStatusCode.OK);
                    Assert.That(response.GetResponseObject().IsValid, Is.True);
                });
            }
        }
    }
}
