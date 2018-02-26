using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class PinSecurity : ApiBase
    {
        public IResponse<ResponseModelPinSecurityCheckResultModel> GetPinSecurity(string pin, string token)
        {
            return Request.Get("/PinSecurity").WithBearerToken(token)
                .AddQueryParameter(nameof(pin), pin)
                .Build().Execute<ResponseModelPinSecurityCheckResultModel>();
        }

        public IResponse<ResponseModel> PostPinSecurity(PinSecurityChangeModel pinSecurityChange, string token)
        {
            return Request.Post("/PinSecurity").WithBearerToken(token)
                .AddJsonBody(pinSecurityChange)
                .Build().Execute<ResponseModel>();
        }
    }
}
