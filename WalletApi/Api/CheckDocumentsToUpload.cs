using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class CheckDocumentsToUpload : ApiBase
    {
        string resource = "/CheckDocumentsToUpload";

        public IResponse<ResponseModelCheckDocumentsToUploadModel> GetCheckDocumentsToUpload(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelCheckDocumentsToUploadModel>();
        }
    }
}
