using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace HFT.Api
{
    public class History : ApiBase
    {
        public IResponse<List<HistoryTradeModel>> GetHistory(string assetId, string skip, string take, string apiKey)
        {
            return Request.Get("/history/trades")
                .AddQueryParameterIfNotNull("assetId", assetId)
                .AddQueryParameterIfNotNull("skip", skip)
                .AddQueryParameterIfNotNull("take", take)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<List<HistoryTradeModel>>();
        }

        public IResponse<HistoryTradeModel> GetHistoryTrade(string tradeId, string apiKey)
        {
            return Request.Get($"/history/trades/{tradeId}")
                .AddQueryParameter("tradeId", tradeId)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<HistoryTradeModel>();
        }
    }
}
