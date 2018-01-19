using BlockchainsIntegration.LitecoinSign;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegration.LiteCoin
{
    class AddressTests
    {
        public class GetAddressInvalidAddress : LitecoinBaseTest
        {
            [TestCase("testAddress")]
            [TestCase("1234567")]
            [TestCase("!@$%^&*(")]
            [Category("LiteCoin")]
            public void GetAddressInvalidAddressTest(string address)
            {
                var response = litecoinApi.Address.GetAddress(address);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().IsValid, Is.False);
            }
        }

        public class GetAddressValidAddress : LitecoinBaseTest
        {
            [Test]
            [Category("LiteCoin")]
            public void GetAddressValidAddressTest()
            {
                var signService = new LitecoinSign();
                var wallet = signService.PostWallet().GetResponseObject();

                var response = litecoinApi.Address.GetAddress(wallet.PublicAddress);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().IsValid, Is.True);
            }
        }
    }
}
