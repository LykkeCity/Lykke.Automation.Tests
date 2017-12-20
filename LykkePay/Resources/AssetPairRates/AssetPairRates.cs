using Lykke.Client.AutorestClient.Models;
using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;
using TestsCore.RestRequests.RestSharpRequest;
using TestsCore.TestsCore;

namespace LykkePay.Resources.AssetPairRates
{
    public class AssetPairRates : LykkePayApi
    {
        private string resource = "/assetPairRates";
        /*
        public IsAliveResponse GetIsAlive()
        {
            var request = new RestRequest("/IsAlive", Method.GET);
            var response = client.Execute(request);
            var isAlive = JsonConvert.DeserializeObject<IsAliveResponse>(response.Content);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return new IsAliveResponse();
            return isAlive;
        }
        
        public override void SetAllureProperties()
        {
            var isAlive = GetIsAlive();
            AllurePropertiesBuilder.Instance.AddPropertyPair("Service", client.BaseUrl.AbsoluteUri + resource);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Environment", isAlive.Env);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
        }
        
        public IResponse<ErrorResponse> DeleteClientAccount(string id)
        {
            return Request.Delete($"/api/ClientAccount/{id}").Build().Execute<ErrorResponse>();
        }
        */

        public IResponse<AssetsPaiRatesResponseModel> GetAssetPairRates(string assetPair)
        {
            return Request.Get($"{resource}/{assetPair}").Build().Execute<AssetsPaiRatesResponseModel>();
        }

        #region POST
        public IResponse<PostAssetsPairRatesModel> PostAssetsPairRates(string assetPair, MerchantModel merchant, MarkupModel markup)
        {
            var request = Request.Post($"{resource}/{assetPair}").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (markup != null)
            {
                var body = JsonConvert.SerializeObject(markup, Formatting.Indented);
                request.AddJsonBody(body);
            }

            return request.Build().Execute<PostAssetsPairRatesModel>();
        }

        public IResponse<PostAssetsPairRatesModel> PostAssetsPairRates(string assetPair, MerchantModel merchant, string markup)
        {
            var request = Request.Post($"{resource}/{assetPair}").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (markup != null)    
                request.AddJsonBody(markup);

            return request.Build().Execute<PostAssetsPairRatesModel>();
        }
        #endregion
    }
}
