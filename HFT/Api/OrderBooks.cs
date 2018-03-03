using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace HFT.Api
{
    public class OrderBooks : ApiBase
    {

        public IResponse<List<OrderBook>> GetOrderBooks()
        {
            return Request.Get("/OrderBooks").Build().Execute<List<OrderBook>>();
        }

        public IResponse<List<OrderBook>> GetOrderBooks(string assetPairId)
        {
            return Request.Get($"/OrderBooks/{assetPairId}").Build().Execute<List<OrderBook>>();
        }
    }
}
