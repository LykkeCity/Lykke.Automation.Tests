using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkePay.Resources.Order
{
    public class Order : LykkePayApi
    {
        private const string resource = "/order";

        public IResponse<OrderResponse> PostOrderModel(MerchantModel merchant, OrderRequestModel orderRequest, string SessionID = null)
        {
            var request = Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).
                AddJsonBody(orderRequest);

            if (SessionID != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", SessionID);

            return request.Build().Execute<OrderResponse>();
        }

        public IResponse<OrderResponse> PostOrderModel(MerchantModel merchant, string orderRequest, string SessionID = null)
        {
            var request = Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).
                AddJsonBody(orderRequest);

            if (SessionID != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", SessionID);

            return request.Build().Execute<OrderResponse>();
        }
    }
}
