namespace HFT.Api
{
    using Lykke.Client.AutorestClient.Models;
    using System.Collections.Generic;
    using XUnitTestCommon.RestRequests.Interfaces;

    public class OrderBooks : ApiBase
    {
        public IResponse<List<OrderBookModel>> GetOrderBooks()
        {
            return Request.Get("/OrderBooks").Build().Execute<List<OrderBookModel>>();
        }

        public IResponse<List<OrderBookModel>> GetOrderBooks(string assetPairId)
        {
            return Request.Get($"/OrderBooks/{assetPairId}").Build().Execute<List<OrderBookModel>>();
        }
    }
}
