using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;

namespace LykkePay.Resources.Transfer
{
    public class Transfer : RestApi
    {
        private string resource = "/transfer";

        public override void SetAllureProperties()
        {
        }

        public IRestResponse PostTransfer(AbstractMerchant merchant, TransferRequestModel requestModel)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            request.AddJsonBody(requestModel);

            var response = client.Execute(request);
            return response;
        }

        public IRestResponse PostTransfer(AbstractMerchant merchant, string requestModel)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Lykke-Merchant-Id", merchant.LykkeMerchantId);
            request.AddHeader("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            request.AddParameter("application/json", requestModel, ParameterType.RequestBody);

            var response = client.Execute(request);
            return response;
        }

        public TransferResponseModel PostTransferModel(AbstractMerchant merchant, TransferRequestModel requestModel) =>
            JsonConvert.DeserializeObject<TransferResponseModel>(PostTransfer(merchant, requestModel).Content);

        public TransferResponseModel PostTransferModel(AbstractMerchant merchant, string requestModel) =>
            JsonConvert.DeserializeObject<TransferResponseModel>(PostTransfer(merchant, requestModel).Content);
    }
}
