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

    public class AssetsCategories : WalletApi
    {
        string resource = "/assetcategories";

        public IResponse<ResponseModelGetAssetCategoriesResponseModel> GetAssetsCategories(string authorization)
        {
            return Request.Get(resource).WithBearerToken(authorization).Build().Execute<ResponseModelGetAssetCategoriesResponseModel>();
        }
    }

    public class AssetDescription : WalletApi
    {
        string resource = "/AssetDescription/";

        public IResponse<ResponseModelAssetDescriptionModel> GetAssetDescription(string assetId, string token)
        {
            return Request.Get(resource + assetId).WithBearerToken(token).Build().Execute<ResponseModelAssetDescriptionModel>();
        }
    }

    public class AssetPair : WalletApi
    {
        string resource = "/AssetPair/";

        public IResponse<ResponseModelGetAssetPairResponseModel> GetAssetPair(string assetPairName, string token)
        {
            return Request.Get(resource + assetPairName).WithBearerToken(token).Build().Execute<ResponseModelGetAssetPairResponseModel>();
        }
    }
}
