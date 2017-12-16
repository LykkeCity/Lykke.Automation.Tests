using LykkePay.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LykkePay.Tests
{
    public class PurchaseTests
    {
        public class PostPurchaseRequiredParamsOnly : BaseTest
        {
            [Test]
            [Category("LykkePay")]
            public void PostPurchaseRequiredParamsOnlyTest()
            {
                var address = new OrderMerchantModel("").BlockChainAddress;
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 100;


                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount);
                var purchaceJson = JsonConvert.SerializeObject(purchaseModel);
                var merchant = new MerchantModel(purchaceJson);
                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaceJson);

                Assert.That(purchase.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //TODO: Check purchase
            }
        }

        public class PostPurchaseAllParams : BaseTest
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

                var merchant = new MerchantModel(purchaseModel);

                var purchase = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaseModel);

                Assert.That(purchase.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //TODO: Check purchase
            }
        }

        public class PostPurchaseSample : BaseTest
        {
            [Test]
            public void PostPurchaseSampleTest()
            {
                var markUp = new MarkupModel(20, 10);
                var merchant = new MerchantModel(markUp);
                var address = new OrderMerchantModel("").BlockChainAddress;
                var assetPair = "BTCUSD";
                var baseAsset = "USD";
                decimal amount = 100;
                var postAssetsPairRates = lykkePayApi.assetPairRates.PostAssetsPairRatesModel(assetPair, merchant, markUp);

                var purchaseModel = new PostPurchaseModel(address, assetPair, baseAsset, amount)
                {
                    successUrl = "http://tut.by",
                    errorUrl = "http://yandex.ru",
                    progressUrl = "http://www.google.com",
                    orderId = "1231123123412",
                    markup = new PostMarkup(20, 0, null)
                };

                merchant.LykkeMerchantSessionId = postAssetsPairRates.LykkeMerchantSessionId;

                var result = lykkePayApi.purchase.PostPurchaseResponse(merchant, purchaseModel);
            }
        }
    }
}
