using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class AssetPairRates : ApiBase
    {
        public IResponse<ResponseModelGetAssetPairsRatesModel> Get(string token) =>
            Request.Get("/AssetPairRates/{id}").WithBearerToken(token)
                .Build().Execute<ResponseModelGetAssetPairsRatesModel>();

        public IResponse<ResponseModelGetAssetPairRateModel> GetById(string id, string token) =>
            Request.Get($"/AssetPairRates/{id}").WithBearerToken(token)
                .Build().Execute<ResponseModelGetAssetPairRateModel>();
    }
}
