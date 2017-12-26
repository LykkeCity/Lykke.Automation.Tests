using LykkePay.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.LykkePayTests
{
    public class OrderTests
    {

        const string successURL = "http://lykkePostBack.pythonanywhere.com/successURL";
        const string progressURL = "http://lykkePostBack.pythonanywhere.com/progressURL";
        const string errorURL = "http://lykkePostBack.pythonanywhere.com/errorURL";

        public class OrderResponseValidate : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order response json")]
            public void OrderResponseValidateTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() {currency = "USD", amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0)};
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId).GetResponseObject();
                Assert.That(orderResponse.currency, Is.EqualTo(orderRequest.exchangeCurrency), "Unexpected currency in order response");
                Assert.That(orderResponse.exchangeRate * orderResponse.amount, Is.EqualTo(orderRequest.amount), "Exchange rate * amount in order response not equals to request amount");
            }
        }

        public class OrderPostBackSuccessResponse : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback response")]
            public void OrderPostBackSuccessResponseTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below
                var oId = TestData.GenerateNumbers(5);
                var orderRequest = new OrderRequestModel() { currency = "USD", amount = 10, exchangeCurrency = "BTC", successURL = successURL + $"?orderId={oId}", errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId).GetResponseObject();
                Assert.That(orderResponse.currency, Is.EqualTo(orderRequest.exchangeCurrency), "Unexpected currency in order response");
                Assert.That(orderResponse.exchangeRate * orderResponse.amount, Is.EqualTo(orderRequest.amount).Within("0.00000000001"), "Exchange rate * amount in order response not equals to request amount");

                var transfer = new TransferRequestModel() {amount = orderResponse.amount + 0.000476m/*temp value - with fee*/, destinationAddress=orderResponse.address, assetId="BTC", sourceAddress = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx" };
                var transferJson = JsonConvert.SerializeObject(transfer);
                var merch = new OrderMerchantModel(transferJson);
                var convertTransfer = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                var tId = convertTransfer.GetResponseObject().transferResponse.transactionId;
                Assert.That(() => lykkePayApi.postBack.GetCallBackByTransactionID(tId).Content, Does.Contain("paymentResponse").And.Contain("PAYMENT_CONFIRMED").And.Contain("PAYMENT_INPROGRESS").After(5 * 60 * 1000, 3 * 1000), $"postback for order id {orderRequest.orderId} is not correct");
            }
        }

        public class OrderPostCurrencyNotValid : LykkepPayBaseTest
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
                var response = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() { currency = currentCurrency, amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest), "Unexpected status code in case currency not valid");            
            }
        }

        public class OrderPostCurrencyValid : LykkepPayBaseTest
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
                var response = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() { currency = currentCurrency, amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code in case currency is valid");
            }
        }

        public class OrderPostOnlyRequiredParams : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback only required params")]
            public void OrderPostOnlyRequiredParamsTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");

                Assert.That(response.GetResponseObject().LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below
                var orderRequestJson = "{\"currency\":\"USD\",\"amount\":10.0}";
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, response.GetResponseObject().LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
            }
        }

        public class OrderPostExchangeCurrencyNotValid : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback negative response")]
            public void OrderPostExchangeCurrencyNotValidTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new MerchantModel(markUp);
                var response = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below

                var orderRequest = new OrderRequestModel() { currency = "USD", amount = 10, exchangeCurrency = "XYZ", successURL = successURL, errorURL = errorURL, progressURL = progressURL, orderId = TestData.GenerateNumbers(5), markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new MerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId);
                Assert.That(orderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code in case currency not valid");
            }
        }

        public class OrderPostBackErrorResponse : LykkepPayBaseTest
        {
            [Test]
            [Category("LykkePay")]
            [Description("Validate Order postback response")]
            public void OrderPostBackErrorResponseTest()
            {
                var assetPair = "BTCUSD";

                MarkupModel markUp = new MarkupModel(50, 30);

                var merchant = new OrderMerchantModel(markUp);

                var balance = lykkePayApi.getBalance.GetGetBalance("n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx", merchant);
                var response = lykkePayApi.assetPairRates.PostAssetsPairRates(assetPair, merchant, markUp);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");
                var postModel = JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(response.Content);
                Assert.That(postModel.LykkeMerchantSessionId, Is.Not.Null, "LykkeMerchantSessionId not present in response");

                // order request below
                var oId = TestData.GenerateNumbers(5);
                var orderRequest = new OrderRequestModel() { currency = "USD", amount = 10, exchangeCurrency = "BTC", successURL = successURL, errorURL = errorURL + $"?orderId={oId}", progressURL = progressURL, orderId = oId, markup = new PostMarkup(markUp, 0) };
                var orderRequestJson = JsonConvert.SerializeObject(orderRequest);
                merchant = new OrderMerchantModel(orderRequestJson);

                var orderResponse = lykkePayApi.order.PostOrderModel(merchant, orderRequestJson, postModel.LykkeMerchantSessionId).GetResponseObject();
                Assert.That(orderResponse.currency, Is.EqualTo(orderRequest.exchangeCurrency), "Unexpected currency in order response");
                Assert.That(orderResponse.exchangeRate * orderResponse.amount, Is.EqualTo(orderRequest.amount).Within("0.00000000001"), "Exchange rate * amount in order response not equals to request amount");

                var transfer = new TransferRequestModel() { amount = orderResponse.amount + 0.00051m/* temp value > then need - will produce error*/, destinationAddress = orderResponse.address, assetId = "BTC", sourceAddress = "n1gDxgVtJmTxaXupcFNd8AeKmdJaihTacx" };
                var transferJson = JsonConvert.SerializeObject(transfer);
                var merch = new OrderMerchantModel(transferJson);
                var convertTransfer = lykkePayApi.transfer.PostTransferModel(merch, transferJson);

                Assert.That(() => lykkePayApi.postBack.GetCallBackByOrderID(oId).Content, Does.Contain("paymentResponse").And.Contain("PAYMENT_ERROR").And.Contain("PAYMENT_INPROGRESS").After(5*60*1000, 3*1000), $"Postback for order id {orderRequest.orderId} is not correct");
            }
        }
    }
}
