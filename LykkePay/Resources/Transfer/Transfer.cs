using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;

namespace LykkePay.Resources.Transfer
{
    public class Transfer : LykkePayApi
    {
        private string resource = "/transfer";
       
        public IResponse<TransferResponseModel> PostTransferModel(AbstractMerchant merchant, TransferRequestModel requestModel)
        {
           return Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).
                AddJsonBody(requestModel).Build().Execute<TransferResponseModel>();
        }

        public IResponse<TransferResponseModel> PostTransferModel(AbstractMerchant merchant, string requestModel)
        {
            return Request.Post(resource).
                 WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                 WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).
                 AddJsonBody(requestModel).Build().Execute<TransferResponseModel>();
        }
    }
}
