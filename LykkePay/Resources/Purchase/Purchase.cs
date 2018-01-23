using Lykke.Client.AutorestClient.Models;
using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;

namespace LykkePay.Resources.Purchase
{
    public class Purchase : LykkePayApi
    {
        private const string resource = "/purchase";

        public IResponse<PostPurchaseResponseModel> PostPurchaseResponse(AbstractMerchant merchantModel, PostPurchaseModel purchaseModel)
        {
            var request = Request.Post(resource).
                WithHeaders("Lykke-Merchant-Id", merchantModel.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchantModel.LykkeMerchantSign);

            if (merchantModel.LykkeMerchantSessionId != null)
                request.WithHeaders("Lykke-Merchant-Session-Id", merchantModel.LykkeMerchantSessionId);

            return request.AddJsonBody(purchaseModel).Build().Execute<PostPurchaseResponseModel>();
        }

        public IResponse<PostPurchaseResponseModel> PostPurchaseResponse(AbstractMerchant merchantModel, string purchaseModel)
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
