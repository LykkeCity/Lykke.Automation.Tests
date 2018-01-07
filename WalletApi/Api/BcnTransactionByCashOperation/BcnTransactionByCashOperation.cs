using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BcnTransactionByCashOperation : ApiBase
    {
        string resource = "/BcnTransactionByCashOperation";

        public IResponse<ResponseModelBlockchainTransactionRespModel> GetBcnTransactionByCashOperation(string id, string token)
        {
            return Request.Get(resource).WithBearerToken(token).AddQueryParameter("id", id).Build().Execute<ResponseModelBlockchainTransactionRespModel>();
        }
    }
}
