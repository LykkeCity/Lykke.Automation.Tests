using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace HFT.Api
{
    public class Orders : ApiBase
    {

        public IResponse<List<LimitOrderStateModel>> GetOrders(OrderStatus status, string skip, string take, string apiKey)
        {
            return Request.Get("/Orders")
                .AddQueryParameterIfNotNull("status", status)
                .AddQueryParameterIfNotNull("skip", skip)
                .AddQueryParameterIfNotNull("take", take)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<List<LimitOrderStateModel>>();
        }

        public IResponse<LimitOrderStateModel> GetOrderById(string id, string apiKey)
        {
            return Request.Get($"/Orders/{id}")
                .WithHeaders("api-key", apiKey)
                .Build().Execute<LimitOrderStateModel>();
        }

        public IResponse<MarketOrderResponseModel> PostOrdersMarket(PlaceMarketOrderModel request, string apiKey)
        {
            return Request.Post($"/Orders/v2/market")
                .AddJsonBody(request)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<MarketOrderResponseModel>();
        }

        public IResponse<LimitOrderResponseModel> PostOrdersLimitOrder(PlaceLimitOrderModel request, string apiKey)
        {
            return Request.Post($"/Orders/v2/limit")
                .AddJsonBody(request)
                .WithHeaders("api-key", apiKey)
                .Build().Execute<LimitOrderResponseModel>();
        }

        public IResponse<string[]> DeleteOrders(string apiKey)
        {
            return Request.Delete($"/Orders")
                .WithHeaders("api-key", apiKey)
                .Build().Execute<string[]>();
        }

        public IResponse<ResponseModel> DeleteOrder(string orderId, string apiKey)
        {
            return Request.Delete($"/Orders/{orderId}")
                .WithHeaders("api-key", apiKey)
                .Build().Execute<ResponseModel>();
        }
    }
}
