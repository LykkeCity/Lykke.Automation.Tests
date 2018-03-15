using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ClientFullName : ApiBase
    {
        string resource = "/ClientFullName";

        public IResponse<ResponseModel> PostClientFullName(PostClientFullNameModel model, string token)
        {
            return Request.Post(resource).WithBearerToken(token).AddJsonBody(model).Build().Execute<ResponseModel>();
        }
    }
}
