using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class PaymentMethods : ApiBase
    {
        public IResponse<PaymentMethodsResponse> GetPaymentsMethods(string token)
        {
            return Request.Get("/PaymentMethods").WithBearerToken(token).Build().Execute<PaymentMethodsResponse>();
        }
    }
}
