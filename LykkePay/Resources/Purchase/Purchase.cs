using Lykke.Client.AutorestClient.Models;
using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestsCore.RestRequests.Interfaces;
using TestsCore.TestsCore;

namespace LykkePay.Resources.Purchase
{
    public class Purchase : LykkePayApi
    {
        private const string resource = "/purchase";
/*
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
*/
        public IResponse<PostPurchaseResponseModel> PostPurchaseResponse(MerchantModel merchantModel, PostPurchaseModel purchaseModel)
        {
            var request = Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchantModel.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign);

            if (merchantModel.LykkeMerchantSessionId != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);

            return request.AddJsonBody(purchaseModel).Build().Execute<PostPurchaseResponseModel>();
        }

        public IResponse<PostPurchaseResponseModel> PostPurchaseResponse(MerchantModel merchantModel, string purchaseModel)
        {
            var request = Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchantModel.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign);

            if (merchantModel.LykkeMerchantSessionId != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);

            return request.AddJsonBody(purchaseModel).Build().Execute<PostPurchaseResponseModel>();
        }
    }
}
