using BlockchainsIntegration.LitecoinSign;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegrationTests.LiteCoin
{
    class AddressTests
    {
        public class GetAddressInvalidAddress : BlockchainsIntegrationBaseTest
        {
            [TestCase("testAddress")]
            [TestCase("1234567")]
            [TestCase("!@$%^&*(")]
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void GetAddressInvalidAddressTest(string address)
            {
                var response = blockchainApi.Address.GetAddress(address);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().IsValid, Is.False);
            }
        }

        public class GetAddressValidAddress : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("Litecoin")]
            [Category("Dash")]
            [Category("Zcash")]
            public void GetAddressValidAddressTest()
            {
                var wallet = blockchainSign.PostWallet();
                wallet.Validate.StatusCode(HttpStatusCode.OK);
                var address = wallet.GetResponseObject().PublicAddress;

                var response = blockchainApi.Address.GetAddress(address);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().IsValid, Is.True);
            }
        }
    }
}
