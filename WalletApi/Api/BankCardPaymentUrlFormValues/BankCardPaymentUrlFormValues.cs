using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BankCardPaymentUrlFormValues : ApiBase
    {
        string resource = "BankCardPaymentUrlFormValues";

        public IResponse<ResponseModelBankCardPaymentUrlInputModel> GetBankCardPaymentUrlFormValues(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelBankCardPaymentUrlInputModel>();
        }
    }
}
