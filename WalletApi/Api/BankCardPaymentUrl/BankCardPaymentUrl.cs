using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
   public class BankCardPaymentUrl : ApiBase
    {
        string resource = "/BankCardPaymentUrl";

        public IResponse<ResponseModelBankCardPaymentUrlResponceModel> PostBankCardPaymentUrl(BankCardPaymentUrlInputModel  model, string token)
        {
            return Request.Post(resource).WithBearerToken(token).AddJsonBody(model).Build().Execute<ResponseModelBankCardPaymentUrlResponceModel>();
        }
    }
}
