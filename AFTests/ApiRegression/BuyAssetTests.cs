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
            double assetBalance = 0;
            string assetToBuy = "BTC";
            double assetToBuyBalance = 0;
            double volume = 0.0001;
            string assetPair = "BTCEUR"; //TODO What about vise versa?
            double assetPairPrice = 0;
            string orderId = null;
            string token = null;
            Key key = null;

            Step($"Login as {email} user", () =>
            {
                var loginStep = new MobileSteps(walletApi).Login(email, password, pin);
                var encodedPrivateKey = loginStep.encodedPrivateKey;
                var privateKey = AesUtils.Decrypt(encodedPrivateKey, password);
                token = loginStep.token;
                key = Key.Parse(privateKey);
            });

            Step("Get current wallets and balances", () =>
            {
                var walltes = walletApi.Wallets.GetWalltes(token)
                    .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                    .GetResponseObject().Result;

                var assetBalanceNullable = walltes.Lykke.Assets
                    .FirstOrDefault(wallet => wallet.Id == asset)?.Balance;
                Assert.That(assetBalanceNullable, Is.Not.Null);
                assetBalance = assetBalanceNullable.Value;
                //TODO: Should 100 eur be enough???
                Assert.That(assetBalance, Is.GreaterThan(100), $"Less than 100 {asset} at the wallet!");

                var assetToBuyBalanceNullable = walltes.Lykke.Assets
                    .FirstOrDefault(wallet => wallet.Id == assetToBuy)?.Balance;
                Assert.That(assetToBuyBalanceNullable, Is.Not.Null);
                assetToBuyBalance = assetToBuyBalanceNullable.Value;
            });

            Step("Find price to purchase", () =>
            {
                var assetPairRates = walletApi.AssetPairRates.GetById(assetPair, token)
                    .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                    .GetResponseObject().Result;
                Assert.That(assetPairRates.Rate.Ask, Is.Not.Null);
                assetPairPrice = assetPairRates.Rate.Ask.Value;
            });

            Step($"Buy {assetToBuy} for {asset}", () =>
            {
                string message = walletApi.SignatureVerificationToken
                    .GetKeyConfirmation(email, token)
                    .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                    .GetResponseObject().Result?.Message;
                Assert.That(message, Is.Not.Null);

                var signedMessage = key.SignMessage(message);

                string accessToken = walletApi.SignatureVerificationToken
                    .PostKeyConfirmation(new RecoveryTokenChallangeResponse(email, signedMessage), token)
                    .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                    .GetResponseObject().Result.AccessToken;
                Assert.That(accessToken, Is.Not.Null);

                orderId = walletApi.HotWallet
                    .PostLimitOrder(new HotWalletLimitOperation
                    {
                        AssetId = assetToBuy,
                        AssetPair = assetPair,
                        Price = assetPairPrice,
                        Volume = volume
                    }, accessToken, token)
                    .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                    .GetResponseObject().Result.Order?.Id;
                Assert.That(orderId, Is.Not.Null);
            });

            Step("Waiting for 1 minute until asset has been sold", () =>
            {
                Assert.That(() => walletApi.LimitOrders.GetOffchainLimitList(token, assetPair)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result.Orders,
                    Is.Empty.After(60 * 1000, 1000));
            });

            Step($"Assert that {assetToBuy} balance has been increased, and {asset} balance decreased", () =>
            {
                Assert.That(walletApi.Wallets.GetWalletsById(assetToBuy, token)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result.Balance,
                    Is.EqualTo(assetToBuyBalance + volume).Within(assetToBuyBalance * 0.01));

                Assert.That(walletApi.Wallets.GetWalletsById(asset, token)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result.Balance,
                    Is.EqualTo(assetBalance - volume * assetPairPrice).Within(assetBalance * 0.01));
            });

            Step("Asserting history", () =>
            {
                Assert.That(()=> walletApi.History.GetByAssetId("", token)
                                 .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                                 .GetResponseObject().Result
                                 .Any(record => record.Trade?.OrderId == orderId && record.Trade?.Asset == asset),
                    Is.True.After(60 * 1000, 1000));
            });
        }
    }
}
