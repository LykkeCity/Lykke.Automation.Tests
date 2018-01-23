using LykkePay.Api;
using LykkePay.Models;
using LykkePay.Models.ResponseModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkePay.Resources.ConvertTransfer
{
    public class ConvertTransfer : LykkePayApi
    {
        private const string resource = "/convert/transfer";
       
        public IResponse<PostConvertTransferResponseModel> PostPurchaseResponse(AbstractMerchant merchantModel, PostConvertTransferModel purchaseModel)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string jsonBody = JsonConvert.SerializeObject(purchaseModel, Formatting.Indented, settings);
            merchantModel.Sign(jsonBody);

            var request = Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchantModel.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign).
                AddJsonBody(jsonBody);

            if (merchantModel.LykkeMerchantSessionId != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);

            return request.Build().Execute<PostConvertTransferResponseModel>();
        }

        public IResponse<PostConvertTransferResponseModel> PostPurchaseResponse(AbstractMerchant merchantModel, string purchaseModelJson)
        {
            merchantModel.Sign(purchaseModelJson);

            var request = Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchantModel.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign).
                AddJsonBody(purchaseModelJson);

            if (merchantModel.LykkeMerchantSessionId != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);

            return request.Build().Execute<PostConvertTransferResponseModel>();
        }
    }
}
