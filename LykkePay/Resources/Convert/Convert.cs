using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;

namespace LykkePay.Resources.Convert
{
    public class Convert : LykkePayApi
    {
        private string resource = "/convert/transfer";

        /*
        public override void SetAllureProperties()
        {
        }



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

    */

        public IResponse<TransferResponseModel> PostConvertModel(AbstractMerchant merchant, ConvertRequestModel requestModel)
        {
            return Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).
                AddJsonBody(requestModel).
                Build().Execute<TransferResponseModel>();
        }
    }
}
