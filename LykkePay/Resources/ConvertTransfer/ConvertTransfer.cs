using LykkePay.Models;
using LykkePay.Models.ResponseModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;

namespace LykkePay.Resources.ConvertTransfer
{
    public class ConvertTransfer : RestApi
    {
        private const string resource = "/convert/transfer";

        public override void SetAllureProperties()
        {
        }

        public (IRestResponse Response, PostConvertTransferResponseModel Data)
            PostPurchaseResponse(MerchantModel merchantModel, PostConvertTransferModel purchaseModel)
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
                var data = JsonConvert.DeserializeObject<PostConvertTransferResponseModel>(response.Content);
                return (response, data);
            }
            catch (JsonReaderException)
            {
                return (response, null);
            }
        }
    }
}
