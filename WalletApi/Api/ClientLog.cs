using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ClientLog : ApiBase
    {
        string resource = "/ClientLog";

        public IResponse<ResponseModel> PostClientLog(WriteClientLogModel model)
        {
            return Request.Post(resource).AddJsonBody(model).Build().Execute<ResponseModel>();
        }
    }
}
