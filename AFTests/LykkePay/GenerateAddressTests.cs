using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LykkePay.Tests
{
    public class GenerateAddressTests
    {
        public class GetGenerateAddress : BaseTest
        {
            [TestCase("BTC")]
            [TestCase("btc")]
            [TestCase("USD")]
            [Category("LykkePay")]
            public void GetGenerateAddressTest(string assetId)
            {
                var newAddress = lykkePayApi.generateAddress.GetGenerateAddress(assetId);

                Assert.That(newAddress.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(newAddress.Data?.Currency, Is.EqualTo(assetId).IgnoreCase, "Wrong wallet currecncy");

                var createdWalletBalance = lykkePayApi.getBalance.GetGetBalance(assetId).Data?
                    .FirstOrDefault(w => w.Address == newAddress.Data.Address);
                Assert.That(createdWalletBalance, Is.Not.Null, "New wallet addres not found");
                Assert.That(createdWalletBalance.Amount, Is.EqualTo(0), "Non zero balance in new wallet");
            }
        }

        public class GetGenerateAddressNegative : BaseTest
        {
            [TestCase("XYZ")]
            [TestCase("321")]
            [Category("LykkePay")]
            public void GetGenerateAddressNegativeTest(string assetId)
            {
                var newAddress = lykkePayApi.generateAddress.GetGenerateAddress(assetId);

                Assert.That(newAddress.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }
    }
}
