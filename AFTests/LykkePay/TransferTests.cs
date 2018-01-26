using LykkePay.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.LykkePayTests
{
    class TransferTests
    {
        static string validAddress = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx";

        public class PostTransferDestinationAddressNegative : LykkepPayBaseTest
        {
            [TestCase("aaa")]
            [TestCase("Test test")]
            [TestCase("!@#$%&")]
            [Category("LykkePay")]
            public void PostTransferDestinationAddressNegativeTest(object address)
            {
                string destionationAddress = address.ToString();

                var transfer = new TransferRequestModel() { amount = 0.0018m, destinationAddress = destionationAddress };
                var transferJson = JsonConvert.SerializeObject(transfer, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(transferResponse.Content, Does.Contain("INVALID_ADDRESS"));
            }
        }

        public class PostTransferAmountNegative : LykkepPayBaseTest
        {
            [TestCase("50,52")]
            [TestCase("Test")]
            [TestCase("!@#$%&")]
            [TestCase("")]
            [TestCase("-50")]
            [Category("LykkePay")]
            public void PostTransferAmountNegativeTest(string amount)
            {
                var transferJson = $"{{\"destinationAddress\":\"{validAddress}\",\"amount\":{amount}}}";
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostTransferAssetIdNegative : LykkepPayBaseTest
        {
            [TestCase("USD")]
            [TestCase("Test")]
            [TestCase("!@#$%&")]
            [TestCase("")]
            [TestCase("50")]
            [Category("LykkePay")]
            public void PostTransferAssetIdNegativeTest(string assetId)
            {
                var transfer = new TransferRequestModel() { amount = 0.0018m, destinationAddress = "mxtrQzcgAa9FV3FFFy9WRQqD3W5vaAD1ov", assetId = assetId };
                var transferJson = JsonConvert.SerializeObject(transfer, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(transferResponse.Content, Does.Contain("INVALID_ADDRESS"));
            }
        }

        public class PostTransferOrderIdNegative : LykkepPayBaseTest
        {
            [TestCase("!@#$%&")]
            [Category("LykkePay")]
            public void PostTransferOrderIdNegativeTest(string orderId)
            {
                var transfer = new TransferRequestModel() { amount = 0.002m, destinationAddress = validAddress, orderId = orderId};
                var transferJson = JsonConvert.SerializeObject(transfer, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostTransferOnlyAmountNegative : LykkepPayBaseTest
        {
            [Category("LykkePay")]
            [Test]
            public void PostTransferOnlyAmountNegativeTest()
            {
                var transfer = new TransferRequestModel() { amount = 0.00090202326503065414m };
                var transferJson = JsonConvert.SerializeObject(transfer, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostTransferOnlyDestinationAddressNegative : LykkepPayBaseTest
        {
            [Category("LykkePay")]
            [Test]
            public void PostTransferOnlyDestinationAddressNegativeTest()
            {
                var transferJson = $"{{\"destinationAddress\":\"{validAddress}\"}}";
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostTransferEmptyBody : LykkepPayBaseTest
        {
            [Category("LykkePay")]
            [Test]
            public void PostTransferEmptyBodyTest()
            {
                var transferJson = "{}";
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }
    }
}
