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
        public void BuyAssetLimitOrderTest()
        {
            //TODO: Move to config
            string email = "untest005@test.com";
            string password = "1234567";
            string pin = "1111";
            string asset = "EUR";
            string assetToBuy = "BTC";
            double volume = 0.0001;
            string assetPair = "BTCEUR"; //TODO What about vise versa?

            var stepHelper = new MobileSteps(walletApi);

            var loginStep = stepHelper.Login(email, password, pin);
            var privateKey = AesUtils.Decrypt(loginStep.encodedPrivateKey, password);
            var token = loginStep.token;
            Key key = Key.Parse(privateKey);

            //------------------

            var walltes = walletApi.Wallets.GetWalltes(token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result;

            var assetBalance = walltes.Lykke.Assets
                .FirstOrDefault(wallet => wallet.Id == asset)?.Balance;
            Assert.That(assetBalance, Is.Not.Null);

            var assetToBuyBalance = walltes.Lykke.Assets
                .FirstOrDefault(wallet => wallet.Id == assetToBuy)?.Balance;
            Assert.That(assetToBuyBalance, Is.Not.Null);

            var assetPairRates = walletApi.AssetPairRates.GetById(assetPair, token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result;

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

            //TODO: Save id
            var postLimitOrder = walletApi.HotWallet
                .PostLimitOrder(new HotWalletLimitOperation
                {
                    AssetId = assetToBuy,
                    AssetPair = assetPair,
                    Price = assetPairRates.Rate.Ask, //Or Bid???
                    Volume = volume
                }, accessToken, token).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

            //Asset has been sold
            Assert.That(()=> walletApi.LimitOrders.GetOffchainLimitList(token, assetPair)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result.Orders, 
                Is.Empty.After(60 * 1000, 1000));

            Assert.That(walletApi.Wallets.GetWalletsById(assetToBuy, token)
                            .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                            .GetResponseObject().Result.Balance,
                Is.EqualTo(assetToBuyBalance + volume));

            //TODO: Asset euro
            //TODO: Asset history
        }
    }
}
