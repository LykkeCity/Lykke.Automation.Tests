using Lykke.Client.AutorestClient.Models;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;
using TestsCore.TestsCore;

namespace LykkePay.Resources.AssetPairRates
{
    public class AssetPairRates : RestApi
    {
        private string resource = "/assetPairRates";

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

        public IRestResponse GetAssetPairRates(string assetPair)
        {
            IRestRequest request = new RestRequest($"{resource}/{assetPair}", Method.GET);
            var respose = client.Execute(request);
            return respose;
        }

        public AssetsPaiRatesResponseModel GetAssetPairRatesModel(string assetPair) => 
            JsonConvert.DeserializeObject<AssetsPaiRatesResponseModel>(GetAssetPairRates(assetPair).Content);

        #region POST
        public IRestResponse PostAssetPairRates(string assetPair, MerchantModel merchant, MarkupModel markup)
        {
            IRestRequest request = new RestRequest($"{resource}/{assetPair}", Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (markup != null)
            {
                var body = JsonConvert.SerializeObject(markup, Formatting.Indented);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }
   
            var respose = client.Execute(request);
            return respose;
        }

        public IRestResponse PostAssetPairRatesWithJsonBody(string assetPair, MerchantModel merchant, string body)
        {
            IRestRequest request = new RestRequest($"{resource}/{assetPair}", Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (body != null)
                request.AddParameter("application/json", body, ParameterType.RequestBody);

            var respose = client.Execute(request);
            return respose;
        }

        public PostAssetsPairRatesModel PostAssetsPairRatesModel(string assetPair, MerchantModel merchant, MarkupModel markup) =>
            JsonConvert.DeserializeObject<PostAssetsPairRatesModel>(PostAssetPairRates(assetPair, merchant, markup).Content);
        #endregion
    }
}
