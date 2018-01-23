using LykkePay.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.LykkePayTests
{
    public class PurchaseTests
    {
        public class PostPurchaseRequiredParamsOnly : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchaseRequiredParamsOnlyTest()
            {
                var address = new OrderMerchantModel("").BlockChainAddress;
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 100m;


                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount);
                var purchaceJson = JsonConvert.SerializeObject(purchaseModel);
                var merchant = new OrderMerchantModel(purchaceJson);
                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //TODO: Check purchase
            }
        }

        public class PostPurchaseAllParams : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchaseAllParamsTest()
            {
                var address = new OrderMerchantModel("").BlockChainAddress;
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 10M;


                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                    orderId = "1231123123412",
                    markup = new PostMarkup(20,0,0)
                };

                var json = JsonConvert.SerializeObject(purchaseModel);

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(purchase.Content, Does.Contain("TRANSFER_INPROGRESS"), "Purchase response doesn't contain 'TRANSFER_INPROGRESS'");
                //TODO: Check purchase
            }
        }

        public class PostPurchaseSample : LykkepPayBaseTest
        {
            [Test]
            public void PostPurchaseSampleTest()
            {
                var markUp = new MarkupModel(20, 10);
                var merchant = new OrderMerchantModel(markUp);
                var address = new OrderMerchantModel("").BlockChainAddress;
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 100;
                var postAssetsPairRates = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp).GetResponseObject();

                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                    orderId = "1231123123412",
                    markup = new PostMarkup(20, 0, null)
                };

                var purchaceJson = JsonConvert.SerializeObject(purchaseModel);

                merchant = new OrderMerchantModel(purchaceJson);
                var result = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Purchase was unsuccessfull");
                Assert.That(result.Content, Does.Contain("TRANSFER_INPROGRESS"), "Purchase response doesn't contain 'TRANSFER_INPROGRESS'");
            }
        }

        #region negative tests

        public class PostPurchaseEmptyBody : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchaseEmptyBodyTest()
            {
                var purchaceJson = "{}";
                var merchant = new OrderMerchantModel(purchaceJson);
                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            }
        }

        public class PostPurchaseDestAddress : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchaseDestAddressTest()
            {
                var purchaceJson = "{\"destinationAddress\":\"n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx\"}";
                var merchant = new OrderMerchantModel(purchaceJson);
                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            }
        }

        public class PostPurchaseAssetPair : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchasePostPurchaseAssetPairTest()
            {
                var purchaceJson = "{\"assetPair\":\"BTCUSD\"}";
                var merchant = new OrderMerchantModel(purchaceJson);
                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            }
        }

        public class PostPurchaseBaseAsset : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchasePostPurchaseBaseAssetTest()
            {
                var purchaceJson = "{\"baseAsset\":\"USD\"}";
                var merchant = new OrderMerchantModel(purchaceJson);
                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            }
        }

        public class PostPurchaseAmount : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchaseAmountTest()
            {
                var purchaceJson = "{\"paidAmount\":100.0}";
                var merchant = new OrderMerchantModel(purchaceJson);
                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostPurchasInvaldAddress : LykkepPayBaseTest
        {
            [TestCase("qweasd")]
            [TestCase("")]
            [TestCase("123")]
            [TestCase("!@#$%")]
            [Category("LykkePay")]
            public void PostPurchasInvaldAddressTest(string address)
            {
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 10M;


                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                    orderId = "1231123123412",
                    markup = new PostMarkup(20, 0, 0)
                };

                var json = JsonConvert.SerializeObject(purchaseModel);

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostPurchasInvaldAssetPair : LykkepPayBaseTest
        {
            [TestCase("USDBTC")]
            [TestCase("")]
            [TestCase("123")]
            [TestCase("!@#$%")]
            [Category("LykkePay")]
            public void PostPurchasInvaldAssetPairTest(string assetPair)
            {
                var address = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx";
                var baseAsset = "USD";
                decimal amount = 10M;


                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                    orderId = "1231123123412",
                    markup = new PostMarkup(20, 0, 0)
                };

                var json = JsonConvert.SerializeObject(purchaseModel);

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostPurchasInvaldBaseAsset : LykkepPayBaseTest
        {
            [TestCase("BTC")]
            [TestCase("")]
            [TestCase("123")]
            [TestCase("!@#$%")]
            [Category("LykkePay")]
            public void PostPurchasInvaldBaseAssetTest(string baseAsset)
            {
                var address = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx";
                var assetPair = "BTCUSD";
                decimal amount = 10M;


                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                    orderId = "1231123123412",
                    markup = new PostMarkup(20, 0, 0)
                };

                var json = JsonConvert.SerializeObject(purchaseModel);

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }

        public class PostPurchasInvaldAmount : LykkepPayBaseTest
        {
            [TestCase("BTC")]
            [TestCase("")]
            [TestCase("123,321")]
            [TestCase("!@#$%")]
            [Category("LykkePay")]
            public void PostPurchasInvaldAmountTest(string amount)
            {
                var json = $"{{\"destinationAddress\":\"n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx\",\"assetPair\":\"BTCUSD\",\"baseAsset\":\"USD\"," +
                    $"\"paidAmount\":{amount},\"successUrl\":\"http://tut.by\",\"errorUrl\":\"http://yandex.ru\",\"progressUrl\":\"http://www.google.com\"," +
                    $"\"orderId\":\"1231123123412\",\"markup\":{{\"percent\":20.0,\"pips\":0,\"fixedFee\":0.0}}}}";

                var merchant = new OrderMerchantModel(json);

                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, json);

                Assert.That(purchase.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            }
        }
        #endregion

    }
}
