using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace HFT.Api
{
    public class Orders : ApiBase
    {

        public IResponse<List<LimitOrderState>> GetOrders(OrderStatus status, string skip, string take, string apiKey)
        {
            return Request.Get("/Orders")
                .AddQueryParameterIfNotNull("status", status)
                .AddQueryParameterIfNotNull("skip", skip)
                .AddQueryParameterIfNotNull("take", take)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<List<LimitOrderState>>();
        }

        public IResponse<LimitOrderState> GetOrderById(string id, string apiKey)
        {
            return Request.Get($"/Orders/{id}")
                .WithHeaders("api-key", apiKey)
                .Build().Execute<LimitOrderState>();
        }

        public IResponse<ResponseModelDouble> PostOrdersMarket(MarketOrderRequest request, string apiKey)
        {
            return Request.Post($"/Orders/market")
                .AddJsonBody(request)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<ResponseModelDouble>();
        }

        public IResponse<ResponseModelDouble> PostOrdersLimitOrder(LimitOrderRequest request, string apiKey)
        {
            return Request.Post($"/Orders/limit")
                .AddJsonBody(request)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<ResponseModelDouble>();
        }

        public IResponse<ResponseModelDouble> PostOrdersCancelOrder(string id, string apiKey)
        {
            return Request.Post($"/Orders/{id}/Cancel")
                .WithHeaders("api-key", apiKey)
                .Build().Execute<ResponseModelDouble>();
        }
    }
}
