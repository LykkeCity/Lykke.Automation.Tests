using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class CashOut : ApiBase
    {
        string resource = "/CashOut";

        public IResponse<ResponseModel> PostCashOut(CashOutPostModel model, string token)
        {
            return Request.Post(resource).WithBearerToken(token).AddJsonBody(model).Build().Execute<ResponseModel>();
        }
    }
}
