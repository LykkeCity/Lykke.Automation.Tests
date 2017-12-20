using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;
using TestsCore.TestsCore;

namespace LykkePay.Resources.PurchaseStatus
{
    public class PurchaseStatus : LykkePayApi
    {
        private const string resource = "/purchase/{transactionId}/status";

        public IResponse<PostPurchaseResponseModel> GetPurchaseStatusResponse(string transactionId)
        {
            string urlToSign = BaseURL + resource + $"/?transactionId={transactionId}";
            var merchantModel = new MerchantModel(urlToSign.Replace("https:", "http:"));

            var request = Request.Get(resource + $"/?transactionId={transactionId}").
                WithHeaders("Lykke-Merchant-Id", merchantModel.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign);

            if (merchantModel.LykkeMerchantSessionId != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);
            return request.Build().Execute<PostPurchaseResponseModel>();

        }


    }
}
