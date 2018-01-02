using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
   public class AllAssets : WalletApi
    {
        string resource = "/AllAssetPairs";

        public IResponse<ResponseModelGetAssetPairsResponseModel> GetAllAssetsPair()
        {
            return Request.Get(resource).Build().Execute<ResponseModelGetAssetPairsResponseModel>();
        }

        public IResponse<ResponseModelGetAssetPairResponseModel> GetAllAssetsPair(string id)
        {
            return Request.Get(resource + $"/{id}").Build().Execute<ResponseModelGetAssetPairResponseModel>();
        }
    }
}
