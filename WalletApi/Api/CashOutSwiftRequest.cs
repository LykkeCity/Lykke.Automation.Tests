using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class CashOutSwiftRequest : ApiBase
    {
        string resource = "/CashOutSwiftRequest";

        public IResponse<ResponseModel> PostCashOutSwiftRequest(SwiftCashOutReqModel model, string token)
        {
            return Request.Post(resource).WithBearerToken(token).AddJsonBody(model).Build().Execute<ResponseModel>();
        }
    }
}
