using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BlockchainTransaction : ApiBase
    {
        string resource = "/BlockchainTransaction";

        public IResponse<ResponseModelBlockchainTransactionRespModel> GetBlockchainTransaction(string blockChainHash, string token)
        {
            return Request.Get(resource).WithBearerToken(token).AddQueryParameter("blockChainHash", blockChainHash).Build().Execute<ResponseModelBlockchainTransactionRespModel>();
        }
    }
}
