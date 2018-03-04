using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.AutorestClient.Models;
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

        public (string token, string encodedPrivateKey) Login(string email, string password, string pin)
        {
            Assert.That(api.ClientState.GetClientState(email, null)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                .GetResponseObject().Result
                .IsRegistered, Is.True, $"Account {email} doesn't exist");

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

            return (token, encodedPrivateKey);
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
    }
}
