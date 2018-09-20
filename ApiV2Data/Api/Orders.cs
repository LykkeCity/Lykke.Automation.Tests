using System;
using System.Collections.Generic;
using System.Text;
using ApiV2Data.Models;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Orders : ApiBase
    {
        public IResponse<OrdersResponse> GetOrders(string offset, string limit, string token)
        {
            return Request.Get("/order").WithBearerToken(token).AddQueryParameter("offset", offset).AddQueryParameter("limit", limit).Build().Execute<OrdersResponse>();
        }

        public IResponse PostOrdersLimitOrderCancel(string orderId, string token)
        {
            return Request.Post($"/orders/limit/{orderId}/cancel").WithBearerToken(token).Build().Execute();
        }

        public IResponse PostOrdersMarket(MarketOrderRequest model, string token)
        {
            return Request.Post("/orders/market").AddJsonBody(model).WithBearerToken(token).Build().Execute();
        }

        public IResponse DeleteOrdersLimit(LimitOrderCancelMultipleRequest model, string token)
        {
            return Request.Delete("/orders/limit").AddJsonBody(model).WithBearerToken(token).Build().Execute();
        }

        public IResponse PostOrdersLimit(LimitOrderRequest model, string token)
        {
            return Request.Post("/orders/limit").WithBearerToken(token).AddJsonBody(model).Build().Execute();
        }

        public IResponse PostOrdersStopLimit(StopLimitOrderRequest model, string token)
        {
            return Request.Post("/orders/stoplimit").AddJsonBody(model).WithBearerToken(token).Build().Execute();
        }

        public IResponse DeleteOrdersLimitOrder(string orderId, string token)
        {
            return Request.Delete($"/orders/limit/{orderId}").WithBearerToken(token).Build().Execute();
        }
    }
}
