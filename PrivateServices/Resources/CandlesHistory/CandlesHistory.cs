using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using RestSharp;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace PrivateServices.Resources.CandlesHistory
{
    public class CandlesHistory
    {
        private IRequestBuilder Request = Requests.For("http://candles-history.prices.svc.cluster.local/api");// Requests.For("http://candles-history.lykke-service.svc.cluster.local/api");

        public IResponse<List<string>> GetAvailableAssetsPairs()
        {
            return Request.Get("/CandlesHistory/availableAssetPairs").Build().Execute<List<string>>();
        }

        public IResponse PostCandlesHistoryBatch(GetCandlesHistoryBatchRequest model)
        {
            return Request.Post("/CandlesHistory/batch").AddJsonBody(model).Build().Execute();
        }

        public IResponse<CandlesHistoryResponseModel> GetCandleHistory(string assetPairId, CandlePriceType priceType, CandleTimeInterval timeInterval, DateTime fromMoment, DateTime toMoment)
        {
            return Request.Get($"/CandlesHistory/{assetPairId}/{priceType}/{timeInterval}/{fromMoment.ToString("s")}/{toMoment.ToString("s")}").Build().Execute<CandlesHistoryResponseModel>();
        }

        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/IsAlive").Build().Execute<IsAliveResponse>();
        }
    }
}
