using LykkePay.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.LykkePayTests
{
    class ConvertTransferTests
    {
        public class PostConvertTransfer : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostConvertTransferTest()
            {
                var address = "mk8KW4VkUYHAbQPTFxQ1GrmoNhjxQsWB9g";
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 10M;


                var convertTransfer = new PostConvertTransferModel(address, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                };

                var merchant = new OrderMerchantModel(convertTransfer);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, convertTransfer);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //TODO: Check purchase
            }
        }

        public class PostConvertTransferNegativeAddress : LykkepPayBaseTest
        {
            [TestCase("")]
            [TestCase("qweasdzxc")]
            [TestCase("123")]
            [TestCase("!@#$%^")]
            [Category("LykkePay")]
            public void PostConvertTransferNegativeAddressTest(string address)
            {
                var testAddress = address.ToString();
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 10M;


                var convertTransfer = new PostConvertTransferModel(testAddress, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                };

                var merchant = new OrderMerchantModel(convertTransfer);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, convertTransfer);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        public class PostConvertTransferNegativeAssetPair : LykkepPayBaseTest
        {
            [TestCase("")]
            [TestCase("test")]
            [TestCase("USDBTC")]
            [TestCase("123")]
            [TestCase("!@#$%^")]
            [Category("LykkePay")]
            public void PostConvertTransferNegativeAssetPairTest(string assetPair)
            {
                var address = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx";
                var testAssetPair = assetPair;
                var baseAsset = "USD";
                decimal amount = 10M;


                var convertTransfer = new PostConvertTransferModel(address, testAssetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                };

                var merchant = new OrderMerchantModel(convertTransfer);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, convertTransfer);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        public class PostConvertTransferNegativebaseAssetPair : LykkepPayBaseTest
        {
            [TestCase("")]
            [TestCase("test")]
            [TestCase("123")]
            [TestCase("!@#$%^")]
            [Category("LykkePay")]
            public void PostConvertTransferNegativebaseAssetPairTest(string asset)
            {
                var address = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx";
                var testAssetPair = "BTCUSD";
                var baseAsset = asset;
                decimal amount = 10M;


                var convertTransfer = new PostConvertTransferModel(address, testAssetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                };

                var merchant = new OrderMerchantModel(convertTransfer);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, convertTransfer);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        public class PostConvertTransferNegativeAmount : LykkepPayBaseTest
        {
            [TestCase("")]
            [TestCase("test")]
            [TestCase("52,51")]
            [TestCase("!@#$%^")]
            [TestCase("-159")]
            [Category("LykkePay")]
            public void PostConvertTransferNegativeAmountTest(string amount)
            {
                var address = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx";
                var testAssetPair = "BTCUSD";
                var baseAsset = "USD";

                var json = $"{{\"destinationAddress\": \"{address}\",\"assetPair\": \"{testAssetPair}\",\"baseAsset\": \"{baseAsset}\",\"amount\": {amount},\"successUrl\": \"http://tut.by\",\"errorUrl\": \"http://yandex.ru\",\"progressUrl\": \"http://www.google.com\"}}";

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        public class PostConvertTransferNegativeEmpty : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostConvertTransferNegativeEmptyTest(string amount)
            {
                var json = "{}";

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        public class PostConvertTransferNegativeOnlyAddress : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostConvertTransferNegativeOnlyAddressTest()
            {
                var address = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx";

                var json = $"{{\"destinationAddress\": \"{address}\"}}";

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        public class PostConvertTransferNegativeOnlyAssetPair : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostConvertTransferNegativeOnlyAssetPairTest()
            {
                var testAssetPair = "BTCUSD";

                var json = $"{{\"assetPair\": \"{testAssetPair}\"}}";

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }

        public class PostConvertTransferNegativeOnlyAmount : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostConvertTransferNegativeOnlyAmountTest()
            {                
                var json = "{\"amount\":10.0 }";

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }
        }
    }
}
