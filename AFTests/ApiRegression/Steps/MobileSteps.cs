using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.DataGenerators;
using NBitcoin;
using NUnit.Framework;

namespace AFTests.ApiRegression.Steps
{
    public class MobileSteps
    {
        private WalletApi.Api.WalletApi api;

        public MobileSteps(WalletApi.Api.WalletApi walletApi)
        {
            api = walletApi;
        }

        public (string token, Key privateKey) Login(string email, string password, string pin)
        {

            string token = api.Auth.PostAuthResponse(new AuthenticateModel()
                {
                    Email = email,
                    Password = password
                }).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result.Token;

            api.PinSecurity.GetPinSecurity(pin, token).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

            api.Client.GetClientCodes(token).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

            string accessToken = api.Client.PostClientCodes(new SubmitCodeModel("0000"), token).Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError().GetResponseObject().Result.AccessToken;

            string encodedPrivateKey = api.Client
                .PostClientEncodedMainKey(new AccessTokenModel(accessToken), token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result.EncodedPrivateKey;

            string privateKeyStr = AesUtils.Decrypt(encodedPrivateKey, password);
            Key privateKey = Key.Parse(privateKeyStr);
            return (token, privateKey);
        }

        public void CancelAnyLimitOrder(string token)
        {
            int? limitOrderCount = api.LimitOrders.GetOffchainLimitCount(token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result.Count;
            Assert.That(limitOrderCount, Is.Not.Null);
            if (limitOrderCount.Value > 0) //Cancel any limit order
            {
                IList<ApiOffchainOrder> limitOrders = api.LimitOrders.GetOffchainLimitList(token, null)
                    .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                    .GetResponseObject().Result.Orders;
                foreach (ApiOffchainOrder limitOrder in limitOrders)
                {
                    api.Offchain.PostLimitCancel(new OffchainLimitCancelModel(limitOrder.Id), token)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();
                }
            }
        }

        public double GetAssetBalance(string assetId, string token)
        {
            var balance = api.Wallets.GetWalletsById(assetId, token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result.Balance;
            Assert.That(balance, Is.Not.Null, $"Balance for {assetId} has no value");

            Console.WriteLine($"{assetId} balance: {balance}");
            return balance.Value;
        }

        public string GetAccessToken(string email, string token, Key key)
        {
            string message = api.SignatureVerificationToken
                .GetKeyConfirmation(email, token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result?.Message;
            Assert.That(message, Is.Not.Null, "Message is null");

            var signedMessage = key.SignMessage(message);

            string accessToken = api.SignatureVerificationToken
                .PostKeyConfirmation(new RecoveryTokenChallangeResponse(email, signedMessage), token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result.AccessToken;

            Assert.That(accessToken, Is.Not.Null, "Access token is null");
            return accessToken;
        }

        public double FindPriceInOrderBook(BuyOrSell buyOrSell, double volume, string assetPair, string token)
        {
            var orderBook = api.OrderBook.GetById(assetPair, token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result;
            if (buyOrSell == BuyOrSell.Buy)
            {
                if (orderBook?.SellOrders == null || !orderBook.SellOrders.Any())
                    return FindPriceInAssetPairRates(buyOrSell, volume, assetPair, token);
                var price = orderBook.SellOrders
                    .Where(order => -order.Volume > volume && order.Price != null)
                    .OrderBy(order => order.Price).First().Price.Value;

                Console.WriteLine($"Found price to {buyOrSell} {assetPair}: {price}");
                return price;
            }
            else
            {
                if (orderBook?.BuyOrders == null || !orderBook.BuyOrders.Any())
                    return FindPriceInAssetPairRates(buyOrSell, volume, assetPair, token);
                var price =  orderBook.BuyOrders
                    .Where(order => order.Volume > volume && order.Price != null)
                    .OrderBy(order => order.Price).First().Price.Value;

                Console.WriteLine($"Found price to {buyOrSell} {assetPair}: {price}");
                return price;
            }
        }

        public double FindPriceInAssetPairRates(BuyOrSell buyOrSell, double volume, string assetPair, string token)
        {
            var assetPairRates = api.AssetPairRates.GetById(assetPair, token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result;
            Assert.That(assetPairRates?.Rate?.Ask, Is.Not.Null, "No Ask rate found");
            Assert.That(assetPairRates?.Rate?.Bid, Is.Not.Null, "No Bid rate found");
            var price = buyOrSell == BuyOrSell.Buy
                ? assetPairRates.Rate.Ask.Value //Buying for the highest price
                : assetPairRates.Rate.Bid.Value; //Selling for the lowest price

            Console.WriteLine($"Found price to {buyOrSell} {assetPair}: {price}");
            return price;
        }
    }
}
