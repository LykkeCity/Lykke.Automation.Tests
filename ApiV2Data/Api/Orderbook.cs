using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Orderbook : ApiBase
    {
        public IResponse<List<OrderBookModel>> GetOrderbook(string assetPairId)
        {
            return Request.Get("/orderbook").AddQueryParameter("assetPairId", assetPairId).Build().Execute<List<OrderBookModel>>();
        }
    }
}
