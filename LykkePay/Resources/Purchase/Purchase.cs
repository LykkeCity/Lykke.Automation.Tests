using Lykke.Client.AutorestClient.Models;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestsCore.ApiRestClient;
using TestsCore.TestsCore;

namespace LykkePay.Resources.Purchase
{
    public class Purchase : RestApi
    {
        private const string resource = "/purchase";

        public IsAliveResponse GetIsAlive()
        {
            var request = new RestRequest("/IsAlive", Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return new IsAliveResponse();

            var isAlive = JsonConvert.DeserializeObject<IsAliveResponse>(response.Content);
            return isAlive;
        }

        public override void SetAllureProperties()
        {
            var isAlive = GetIsAlive();
            AllurePropertiesBuilder.Instance.AddPropertyPair("Service", client.BaseUrl.AbsoluteUri + resource);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Environment", isAlive.Env);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
        }

        public (IRestResponse Response, PostPurchaseResponseModel Data) 
            PostPurchaseResponse(MerchantModel merchantModel, PostPurchaseModel purchaseModel)
        {
            var request = new RestRequest(resource, Method.POST);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string jsonBody = JsonConvert.SerializeObject(purchaseModel, Formatting.Indented, settings);
            merchantModel.Sign(jsonBody);

            request.AddParameter("application/json", jsonBody, "application/json", ParameterType.RequestBody);
            request.AddHeader("Lykke-Merchant-Id", merchantModel.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign);
            if (merchantModel.LykkeMerchantSessionId != null)
                request.AddHeader("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);

            var response = client.Execute(request);
            try
            {
                var data = JsonConvert.DeserializeObject<PostPurchaseResponseModel>(response.Content);
                return (response, data);
            }
            catch (JsonReaderException)
            {
                return (response, null);
            }
        }

        public (IRestResponse Response, PostPurchaseResponseModel Data)
            PostPurchaseResponse(MerchantModel merchantModel, string purchaseModel)
        {
            var request = new RestRequest(resource, Method.POST);
  
            request.AddParameter("application/json", purchaseModel, "application/json", ParameterType.RequestBody);
            request.AddHeader("Lykke-Merchant-Id", merchantModel.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign);
            if (merchantModel.LykkeMerchantSessionId != null)
                request.AddHeader("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);

            var response = client.Execute(request);
            try
            {
                var data = JsonConvert.DeserializeObject<PostPurchaseResponseModel>(response.Content);
                return (response, data);
            }
            catch (JsonReaderException)
            {
                return (response, null);
            }
        }
    }
}
