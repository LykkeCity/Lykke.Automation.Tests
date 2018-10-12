namespace HFT.Api
{
    using Lykke.Client.AutorestClient.Models;
    using System.Collections.Generic;
    using XUnitTestCommon.RestRequests.Interfaces;

    public class History : ApiBase
    {
        public IResponse<List<HistoryTradeModel>> GetHistory(string assetId, string assetPairId, int skip, int take, string apiKey)
        {
            return Request.Get("/History/trades")
                .AddQueryParameterIfNotNull("assetId", assetId)
                .AddQueryParameterIfNotNull("assetPairId", assetPairId)
                .AddQueryParameterIfNotNull("skip", skip)
                .AddQueryParameterIfNotNull("take", take)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<List<HistoryTradeModel>>();
        }
    }
}
