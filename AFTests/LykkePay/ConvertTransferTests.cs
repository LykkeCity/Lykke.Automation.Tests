using LykkePay.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LykkePay.Tests
{
    class ConvertTransferTests
    {
        public class PostConvertTransfer : BaseTest
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

                var merchant = new MerchantModel(convertTransfer);

                var purchase = lykkePayApi.convertTransfer.PostPurchaseResponse(merchant, convertTransfer);

                Assert.That(purchase.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                //TODO: Check purchase
            }
        }
    }
}
