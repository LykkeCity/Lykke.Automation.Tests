using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Dialogs : ApiBase
    {
        string resource = "/Dialogs";

        public IResponse<ResponseModelClientDialogsModel> GetDialogs(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelClientDialogsModel>();
        }

        public IResponse<ResponseModel> PostDialogs(ClientDialogSubmitModel model, string token)
        {
            return Request.Post(resource).AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModel>();
        }
    }
}
