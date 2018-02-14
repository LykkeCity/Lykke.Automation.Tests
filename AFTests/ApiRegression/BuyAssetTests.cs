using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFTests.ApiRegression.Steps;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.DataGenerators;
using NBitcoin;
using NUnit.Framework;

namespace AFTests.ApiRegression
{
    class BuyAssetTests : ApiRegressionBaseTest
    {
        [Test]
        [Category("ApiRegression")]
        public void BuyAssetTest()
        {
            string email = "untest005@test.com";
            string password = "1234567";
            string pin = "1111";
            string assetPair = "EURUSD";

            var stepHelper = new MobileSteps(walletApi);

            var loggedResult = stepHelper.Login(email, password, pin);
            var privateKey = AesUtils.Decrypt(loggedResult.encodedPrivateKey, password);
            var token = loggedResult.token;
            Key key = Key.Parse(privateKey);

            //------------------

            var getOrderBook = walletApi.OrderBook.GetById(assetPair, token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

            var getVerificationToken = walletApi.SignatureVerificationToken.GetKeyConfirmation(email, token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();
            string message = getVerificationToken.GetResponseObject().Result.Message;
            Assert.That(message, Is.Not.Null);

            var signedMessage = key.SignMessage(message);
            var postVerificationToken = walletApi.SignatureVerificationToken
                .PostKeyConfirmation(new RecoveryTokenChallangeResponse(email, signedMessage), token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();
            string accessToken = postVerificationToken.GetResponseObject().Result.AccessToken;
            Assert.That(accessToken, Is.Not.Null);

            var postLimitOrder = walletApi.HotWallet
                .PostLimitOrder(new HotWalletLimitOperation
                {
                    AssetId = "EUR",
                    AssetPair = assetPair,
                    Price = getOrderBook.GetResponseObject().Result.SellOrders.First().Price,
                    Volume = 1
                }, accessToken, token).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();
        }
    }
}
