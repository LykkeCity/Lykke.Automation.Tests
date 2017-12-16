using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;
using TestsCore.TestsCore;

namespace LykkePay.Resources.GenerateAddress
{
    public class GenerateAddress : RestApi
    {
        private string resource = "/generateAddress";


        public override void SetAllureProperties()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Service", client.BaseUrl.AbsoluteUri + resource);
        }

        public (IRestResponse Response, GetGenerateAddressResponseModel Data) GetGenerateAddress(string id)
        {
            IRestRequest request = new RestRequest($"{resource}/{id}", Method.GET);
            string urlToSign = (client.BaseUrl + $"{resource}/{id}").Replace("https:", "http:");
            var merchant = new MerchantModel(urlToSign);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);

            var response = client.Execute(request);
            try
            {
                var data = JsonConvert.DeserializeObject<GetGenerateAddressResponseModel>(response.Content);
                return (response, data);
            }
            catch (JsonReaderException)
            {
                return (response, null);
            }
        }
    }
}
