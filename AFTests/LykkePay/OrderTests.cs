using LykkePay.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TestsCore.TestsData;

namespace LykkePay.Tests
{
    public class OrderTests
    {

        const string successURL = "http://lykkePostBack.pythonanywhere.com/successURL";
        const string progressURL = "http://lykkePostBack.pythonanywhere.com/progressURL";
        const string errorURL = "http://lykkePostBack.pythonanywhere.com/errorURL";

        public class OrderResponseValidate : BaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order response json")]
            public void OrderResponseValidateTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() {currency = "USD", amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0)};
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.currency, Is.EqualTo(orderRequest.exchangeCurrency), "Unexpected currency in order response");
                Assert.That(orderResponse.exchangeRate * orderResponse.amount, Is.EqualTo(orderRequest.amount), "Exchange rate * amount in order response not equals to request amount");
            }
        }

        public class OrderPostBackSuccessResponse : BaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback response")]
            public void OrderPostBackSuccessResponseTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() { currency = "USD", amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.currency, Is.EqualTo(orderRequest.exchangeCurrency), "Unexpected currency in order response");
                Assert.That(orderResponse.exchangeRate * orderResponse.amount, Is.EqualTo(orderRequest.amount).Within("0.00000000001"), "Exchange rate * amount in order response not equals to request amount");

                var transfer = new TransferRequestModel() {amount = orderResponse.amount, destinationAddress=orderResponse.address, assetId="BTC", sourceAddress = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx" };
                var transferJson = JsonConvert.SerializeObject(transfer);
                var merch = new OrderMerchantModel(transferJson);
                var convertTransfer = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                var getB = lykkePayApi.getBalance.GetGetBalance("BTC", merch);
                Assert.Fail("No Postabck yet");
            }
        }

        public class OrderPostCurrencyNotValid : BaseTest
        {
            [TestCase("XYZ")]
            [TestCase("BTC")]
            [Category("LykkePay")]
            [Description("Validate Order postback negative response")]
            public void OrderPostCurrencyNotValidTest(object currency)
            {
                var assetPair = "BTCUSD";
                var currentCurrency = currency.ToString();

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() { currency = currentCurrency, amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrder(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Unexpected status code in case currency not valid");            
            }
        }

        public class OrderPostCurrencyValid : BaseTest
        {
            [TestCase("USD")]
            [TestCase("CHF")]
            [TestCase("EUR")]
            [Category("LykkePay")]
            [Description("Validate Order postback positive response")]
            public void OrderPostCurrencyValidTest(object currency)
            {
                var assetPair = "BTCUSD";
                var currentCurrency = currency.ToString();

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() { currency = currentCurrency, amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrder(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code in case currency is valid");
            }
        }

        public class OrderPostOnlyRequiredParams : BaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback only required params")]
            public void OrderPostOnlyRequiredParamsTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below
                var orderRequestJson = "{\"currency\":\"USD\",\"amount\":10.0}";
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrder(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
            }
        }

        public class OrderPostExchangeCurrencyNotValid : BaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback negative response")]
            public void OrderPostExchangeCurrencyNotValidTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() { currency = "USD", amount = 10, exchangeCurrency = "XYZ", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrder(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code in case currency not valid");
            }
        }

        public class OrderPostBackErrorResponse : BaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback response")]
            public void OrderPostBackErrorResponseTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below
                var oId = TestData.GenerateNumbers(5);
                var orderRequest = new OrderRequestModel() { currency = "USD", amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL + $"?orderId={oId}", progressURL = progressURL, orderId = oId, markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.currency, Is.EqualTo(orderRequest.exchangeCurrency), "Unexpected currency in order response");
                Assert.That(orderResponse.exchangeRate * orderResponse.amount, Is.EqualTo(orderRequest.amount).Within("0.00000000001"), "Exchange rate * amount in order response not equals to request amount");

                var transfer = new TransferRequestModel() { amount = orderResponse.amount/2, destinationAddress = orderResponse.address, assetId = "BTC", sourceAddress = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx" };
                var transferJson = JsonConvert.SerializeObject(transfer);
                var merch = new OrderMerchantModel(transferJson);
                var convertTransfer = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                var getB = lykkePayApi.getBalance.GetGetBalance("BTC", merch);
                Assert.Fail("No Postback Yet");
            }
        }
    }
}
