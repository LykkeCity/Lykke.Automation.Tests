using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;

namespace LykkePay.Resources.Order
{
    public class Order : RestApi
    {
        public override void SetAllureProperties()
        {
            //do nothing
        }

        private const string resource = "/order";

        public IRestResponse PostOrder(MerchantModel merchant, OrderRequestModel orderRequest, string SessionID = null)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (SessionID != null)
                request.AddHeader("Lykke-Merchant-Session-Id", SessionID);
            request.AddJsonBody(orderRequest);

            var response = client.Execute(request);
            return response;
        }


        public IRestResponse PostOrder(MerchantModel merchant, string orderRequest, string SessionID = null)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (SessionID != null)
                request.AddHeader("Lykke-Merchant-Session-Id", SessionID);
            request.AddParameter("application/json", orderRequest, ParameterType.RequestBody);

            var response = client.Execute(request);
            return response;
        }

        public OrderResponse PostOrderModel(MerchantModel merchant, OrderRequestModel orderRequest, string SessionID = null) =>
            JsonConvert.DeserializeObject<OrderResponse>(PostOrder(merchant, orderRequest, SessionID)?.Content);

        public OrderResponse PostOrderModel(MerchantModel merchant, string orderRequest, string SessionID = null) =>
            JsonConvert.DeserializeObject<OrderResponse>(PostOrder(merchant, orderRequest, SessionID)?.Content);
    }
}
