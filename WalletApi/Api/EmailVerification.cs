using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class EmailVerification : ApiBase
    {
        public IResponse<ResponseModelEmailVerificationModel> GetEmailVerification(string email, string code, string partnerId)
        {
            return Request.Get("/EmailVerification")
                .AddQueryParameter(nameof(email), email)
                .AddQueryParameterIfNotNull(nameof(code), code)
                .AddQueryParameterIfNotNull(nameof(partnerId), partnerId)
                .Build().Execute<ResponseModelEmailVerificationModel>();
        }

        public IResponse<ResponseModel> PostEmailVerification(PostEmailModel emailModel)
        {
            return Request.Post("/EmailVerification").AddJsonBody(emailModel)
                .Build().Execute<ResponseModel>();
        }
    }
}
