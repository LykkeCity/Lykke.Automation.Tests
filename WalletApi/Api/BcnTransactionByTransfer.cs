using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BcnTransactionByTransfer : ApiBase
    {
        string resource = "/BcnTransactionByTransfer";

        public IResponse<ResponseModelBlockchainTransactionRespModel> GetBcnTransactionByTransfer(string id, string token)
        {
            return Request.Get(resource).AddQueryParameter("id", id).Build().Execute<ResponseModelBlockchainTransactionRespModel>();
        }
    }
}
