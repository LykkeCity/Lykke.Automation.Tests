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
