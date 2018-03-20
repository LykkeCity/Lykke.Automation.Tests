using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Issuers : ApiBase
    {
        public IResponse<ResponseModelIEnumerableApiIssuer> Get(string token)
        {
            return Request.Get("/Issuers").WithBearerToken(token)
                .Build().Execute<ResponseModelIEnumerableApiIssuer>();
        }

        public IResponse<ResponseModelApiIssuer> GetById(string id, string token)
        {
            return Request.Get($"/Issuers/{id}").WithBearerToken(token)
                .Build().Execute<ResponseModelApiIssuer>();
        }
    }
}
