using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class SignatureVerificationToken : ApiBase
    {
        public IResponse<ResponseModelRecoveryTokenChallange> GetKeyConfirmation(string email, string token) =>
            Request.Get("/signatureVerificationToken/KeyConfirmation").WithBearerToken(token)
                .AddQueryParameter(nameof(email), email).Build().Execute<ResponseModelRecoveryTokenChallange>();

        public IResponse<ResponseModelRecoveryToken> PostKeyConfirmation(RecoveryTokenChallangeResponse tokenChallangeResponse,
            string token) =>
            Request.Post("/signatureVerificationToken/KeyConfirmation").WithBearerToken(token)
                .AddJsonBody(tokenChallangeResponse).Build().Execute<ResponseModelRecoveryToken>();
    }
}
