using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ClientFirstNameLastName : ApiBase
    {
        string resource = "/ClientFirstNameLastName";

        public IResponse<ResponseModel> PostClientFirstNameLastName(PostClientFirstNameLastNameModel model, string token)
        {
            return Request.Post(resource).WithBearerToken(token).AddJsonBody(model).Build().Execute<ResponseModel>();
        }
    }
}
