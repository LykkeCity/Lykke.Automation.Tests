using LykkePay.Api;
using LykkePay.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;
using TestsCore.TestsCore;

namespace LykkePay.Resources.GenerateAddress
{
    public class GenerateAddress : LykkePayApi
    {
        private string resource = "/generateAddress";

        /*
        public override void SetAllureProperties()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Service", client.BaseUrl.AbsoluteUri + resource);
        }
    */

        public IResponse<GetGenerateAddressResponseModel> GetGenerateAddress(string id)
        {
            string urlToSign = (BaseURL + $"{resource}/{id}").Replace("https:", "http:");
            var merchant = new MerchantModel(urlToSign);

            return Request.Get($"{resource}/{id}").
                WithHeaders("Lykke-Merchant-Id", merchant.LykkeMerchantId).
                WithHeaders("Lykke-Merchant-Sign", merchant.LykkeMerchantSign).
                Build().Execute<GetGenerateAddressResponseModel>();
        }
    }
}
