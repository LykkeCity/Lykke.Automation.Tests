using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;

namespace LykkePay.Resources.Convert
{
    public class Convert : RestApi
    {
        public override void SetAllureProperties()
        {
        }

        private string resource = "/convert/transfer";

        public IRestResponse PostConvert(AbstractMerchant merchant, ConvertRequestModel requestModel)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);

            request.AddJsonBody(requestModel);
            var response = client.Execute(request);
            return response;
        }

        public IRestResponse PostConvert(AbstractMerchant merchant, string requestModel)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);

            request.AddParameter("application/json", requestModel, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        public TransferResponseModel PostConvertModel(AbstractMerchant merchant, ConvertRequestModel requestModel) =>
            JsonConvert.DeserializeObject<TransferResponseModel>(PostConvert(merchant, requestModel).Content);

        public TransferResponseModel PostConvertModel(AbstractMerchant merchant, string requestModel) =>
           JsonConvert.DeserializeObject<TransferResponseModel>(PostConvert(merchant, requestModel).Content);
    }
}
