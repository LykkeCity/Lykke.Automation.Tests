using Lykke.Client.AutorestClient.Models;
using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkePay.Resources.AssetPairRates
{
    public class AssetPairRates : LykkePayApi
    {
        private string resource = "/assetPairRates";       

        public IResponse<AssetsPaiRatesResponseModel> GetAssetPairRates(string assetPair)
        {
            return Request.Get($"{resource}/{assetPair}").Build().Execute<AssetsPaiRatesResponseModel>();
        }

        #region POST
        public IResponse<PostAssetsPairRatesModel> PostAssetsPairRates(string assetPair, AbstractMerchant merchant, MarkupModel markup)
        {
            var request = Request.Post($"{resource}/{assetPair}").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (markup != null)
            {
                var body = JsonConvert.SerializeObject(markup, Formatting.Indented);
                request.AddJsonBody(body);
            }

            return request.Build().Execute<PostAssetsPairRatesModel>();
        }

        public IResponse<PostAssetsPairRatesModel> PostAssetsPairRates(string assetPair, AbstractMerchant merchant, string markup)
        {
            var request = Request.Post($"{resource}/{assetPair}").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign);
            if (markup != null)    
                request.AddJsonBody(markup);

            return request.Build().Execute<PostAssetsPairRatesModel>();
        }
        #endregion
    }
}
