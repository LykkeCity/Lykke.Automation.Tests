using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.TestsCore;
using RestSharp;
using LykkePay.Models;
using Newtonsoft.Json;
using LykkePay.Api;
using TestsCore.RestRequests.Interfaces;

namespace LykkePay.Resources.GetBalance
{
    public class GetBalance : LykkePayApi
    {
        private string resource = "/getBalance";

        public IResponse<List<GetGetBalanceResponseModel>> GetGetBalance(string assetId)
        {
            string urlToSign = BaseURL + resource;
            var merchant = new MerchantModel(urlToSign.Replace("https:", "http:"));

            return Request.Get($"{resource}/{assetId}").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).Build().Execute<List<GetGetBalanceResponseModel>>();          
        }

        public IResponse<List<GetGetBalanceResponseModel>> GetGetBalanceNonEmpty(string assetId)
        {
            string urlToSign = BaseURL + resource;
            var merchant = new MerchantModel(urlToSign.Replace("https:", "http:"));

            return Request.Get($"{resource}/{assetId}/nonempty").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).Build().Execute<List<GetGetBalanceResponseModel>>();
        }

        public IResponse<List<GetGetBalanceResponseModel>> GetGetBalance(string assetId, AbstractMerchant merchant)
        {
            string urlToSign = BaseURL + resource;
            merchant.Sign(urlToSign.Replace("https:", "http:"));

            return Request.Get($"{resource}/{assetId}").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).Build().Execute<List<GetGetBalanceResponseModel>>();
        }

        public IResponse<List<GetGetBalanceResponseModel>> GetGetBalanceNonEmpty(string assetId, AbstractMerchant merchant)
        {
            string urlToSign = BaseURL + resource;
            merchant.Sign(urlToSign.Replace("https:", "http:"));

            return Request.Get($"{resource}/{assetId}/nonempty").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).Build().Execute<List<GetGetBalanceResponseModel>>();
        }
    }
}
