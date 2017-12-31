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
            [TestCase("")]
            [Category("LykkePay")]
            public void PostTransferDestinationAddressNegativeTest(object address)
            {
                string destionationAddress = address.ToString();

                var transfer = new TransferRequestModel() { amount = 0.0018m, destinationAddress = destionationAddress };
                var transferJson = JsonConvert.SerializeObject(transfer, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
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
            public void PostTransferAmountNegativeTest(object amount)
            {
                string testAmount = amount.ToString();

                var transferJson = $"{{\"destinationAddress\":\"{validAddress}\",\"amount\":{testAmount}}}";
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
            public void PostTransferAssetIdNegativeTest(object assetId)
            {
                string testAsset = assetId.ToString();

                var transfer = new TransferRequestModel() { amount = 0.005m, destinationAddress = validAddress, assetId = testAsset };
                var transferJson = JsonConvert.SerializeObject(transfer, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var merch = new OrderMerchantModel(transferJson);
                var transferResponse = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(transferResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostTransferOrderIdNegative : LykkepPayBaseTest
        {        
            [TestCase("!@#$%&")]
            [Category("LykkePay")]
            public void PostTransferOrderIdNegativeTest(object orderId)
            {
                string testOrderId = orderId.ToString();

                var transfer = new TransferRequestModel() { amount = 0.005m, destinationAddress = validAddress, orderId = testOrderId };
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
