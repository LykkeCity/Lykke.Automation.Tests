using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BroadcastTransaction : ApiBase
    {
        string resource = "/BroadcastTransaction";

        public IResponse<ResponseModel> PostBroadcastTransaction(ApiTransaction model, string token)
        {
            return Request.Post(resource).AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModel>();
        }
    }
}
