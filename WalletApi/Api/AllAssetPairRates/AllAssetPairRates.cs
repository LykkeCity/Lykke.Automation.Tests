using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class AllAssetPairRates : WalletApi
    {
        string resource = "/AllAssetPairRates";

        public IResponse<ResponseModelGetAssetPairsRatesModel> GetAllAssetPairRates()
        {
            return Request.Get(resource).Build().Execute<ResponseModelGetAssetPairsRatesModel>();
        }
    }
}
