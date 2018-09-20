using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class AssetPairs : ApiBase
    {
        public IResponse<AssetPairResponseModel> GetAssetPairs()
        {
            return Request.Get("/AssetPairs").Build().Execute<AssetPairResponseModel>();
        }

        public IResponse<AssetPairResponseModel> GetAssetPairsAvailable(string token)
        {
            return Request.Get("/AssetPairs/available").WithBearerToken(token).Build().Execute<AssetPairResponseModel>();
        }

        public IResponse<AssetPairResponseModel> GetAssetPairsId(string id)
        {
            return Request.Get($"/AssetPairs/{id}").Build().Execute<AssetPairResponseModel>();
        }

        public IResponse<AssetPairRatesResponseModel> GetAssetPairRates()
        {
            return Request.Get("/AssetPairs/rates").Build().Execute<AssetPairRatesResponseModel>();
        }
    }
}
