using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ClientPhone : ApiBase
    {
        string resource = "/ClientPhone";
        
        public IResponse<ResponseModel> PostClientPhone(PostClientPhoneModel model, string token)
        {
            return Request.Post(resource).AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModel>();
        }
    }
}
