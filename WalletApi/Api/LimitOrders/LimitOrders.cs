using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class LimitOrders : ApiBase
    {
        public IResponse<ResponseModelOffchainLimitOrdersRespModel> GetOffchainLimitList(string token, string assetPair = null)
        {
            return Request.Get("/offchain/limit/list").WithBearerToken(token)
                .AddQueryParameterIfNotNull(nameof(assetPair), assetPair)
                .Build().Execute<ResponseModelOffchainLimitOrdersRespModel>();
        }

        public IResponse<ResponseModelOffchainLimitOrdersCountRespModel> GetOffchainLimitCount(string token)
        {
            return Request.Get("/offchain/limit/count").WithBearerToken(token)
                .Build().Execute<ResponseModelOffchainLimitOrdersCountRespModel>();
        }
    }
}
