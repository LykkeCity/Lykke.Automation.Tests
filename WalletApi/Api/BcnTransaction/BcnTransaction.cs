using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BcnTransaction: ApiBase
    {
        string resource = "/BcnTransaction";

        public IResponse<ResponseModelBlockchainTransactionRespModel> GetBcnTransaction(string id, string token)
        {
            return Request.Get(resource).WithBearerToken(token).AddQueryParameter("id", id).Build().Execute<ResponseModelBlockchainTransactionRespModel>();
        }

        public IResponse<ResponseModelBlockchainTransactionRespModel> GetBcnTransactionOffChain(string id, string token)
        {
            return Request.Get(resource + "/offchain-trade").WithBearerToken(token).AddQueryParameter("id", id).Build().Execute<ResponseModelBlockchainTransactionRespModel>();
        }
    }
}
