using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.AutorestClient.Models;

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
    }
}
