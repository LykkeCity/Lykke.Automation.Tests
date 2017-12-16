using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;
using TestsCore.TestsCore;
using RestSharp;
using LykkePay.Models;
using Newtonsoft.Json;

namespace LykkePay.Resources.GetBalance
{
    public class GetBalance : RestApi
    {
        private string resource = "/getBalance";

        public override void SetAllureProperties()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Service", client.BaseUrl.AbsoluteUri + resource);
        }

        private void SetMerchantHeadersForGetRequest(ref IRestRequest request)
        {
            string urlToSign = client.BaseUrl + request.Resource;
            var merchant = new MerchantModel(urlToSign.Replace("https:", "http:"));
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
        }

        private void SetMerchantHeadersForGetRequest(ref IRestRequest request, AbstractMerchant merchant)
        {
            string urlToSign = client.BaseUrl + request.Resource;
            merchant.Sign(urlToSign.Replace("https:", "http:"));
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
        }

        public (IRestResponse Response, List<GetGetBalanceResponseModel> Data) GetGetBalance(string assetId)
        {
            IRestRequest request = new RestRequest($"{resource}/{assetId}", Method.GET);
            SetMerchantHeadersForGetRequest(ref request);

            var response = client.Execute(request);
            try
            {
                var data = JsonConvert.DeserializeObject<List<GetGetBalanceResponseModel>>(response.Content);
                return (response, data);
            }
            catch (JsonReaderException)
            {
                return (response, null);
            }
        }

        public (IRestResponse Response, List<GetGetBalanceResponseModel> Data) GetGetBalanceNonEmpty(string assetId)
        {
            IRestRequest request = new RestRequest($"{resource}/{assetId}/nonempty", Method.GET);
            SetMerchantHeadersForGetRequest(ref request);

            var response = client.Execute(request);
            try
            {
                var data = JsonConvert.DeserializeObject<List<GetGetBalanceResponseModel>>(response.Content);
                return (response, data);
            }
            catch (JsonReaderException)
            {
                return (response, null);
            }            
        }

        public (IRestResponse Response, List<GetGetBalanceResponseModel> Data) GetGetBalance(string assetId, AbstractMerchant merchant)
        {
            IRestRequest request = new RestRequest($"{resource}/{assetId}", Method.GET);
            SetMerchantHeadersForGetRequest(ref request, merchant);

            var response = client.Execute(request);
            var data = JsonConvert.DeserializeObject<List<GetGetBalanceResponseModel>>(response.Content);
            return (response, data);
        }

        public (IRestResponse Response, List<GetGetBalanceResponseModel> Data) GetGetBalanceNonEmpty(string assetId, AbstractMerchant merchant)
        {
            IRestRequest request = new RestRequest($"{resource}/{assetId}/nonempty", Method.GET);
            SetMerchantHeadersForGetRequest(ref request, merchant);

            var response = client.Execute(request);
            var data = JsonConvert.DeserializeObject<List<GetGetBalanceResponseModel>>(response.Content);
            return (response, data);
        }
    }
}
