using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ChangePinAndPassword : ApiBase
    {
        string resource = "/ChangePinAndPassword";

        public IResponse<ResponseModel> PostChangePinAndPassword(PostChangePinAndPasswordModel model)
        {
            return Request.Post(resource).AddJsonBody(model).Build().Execute<ResponseModel>();
        }
    }
}
