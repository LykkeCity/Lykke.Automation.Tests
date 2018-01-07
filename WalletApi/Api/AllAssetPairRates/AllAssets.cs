using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
   public class AllAssets : ApiBase
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

    public class AssetsCategories : ApiBase
    {
        string resource = "/assetcategories";

        public IResponse<ResponseModelGetAssetCategoriesResponseModel> GetAssetsCategories(string authorization)
        {
            return Request.Get(resource).WithBearerToken(authorization).Build().Execute<ResponseModelGetAssetCategoriesResponseModel>();
        }
    }

    public class AssetDescription : ApiBase
    {
        string resource = "/AssetDescription/";

        public IResponse<ResponseModelAssetDescriptionModel> GetAssetDescription(string assetId, string token)
        {
            return Request.Get(resource + assetId).WithBearerToken(token).Build().Execute<ResponseModelAssetDescriptionModel>();
        }
    }

    public class AssetPair : ApiBase
    {
        string resource = "/AssetPair/";

        public IResponse<ResponseModelGetAssetPairResponseModel> GetAssetPair(string assetPairName, string token)
        {
            return Request.Get(resource + assetPairName).WithBearerToken(token).Build().Execute<ResponseModelGetAssetPairResponseModel>();
        }
    }

    public class AssetPairDetailedRates : ApiBase
    {
        string resource = "/AssetPairDetailedRates";

        public IResponse<ResponseModelGetAssetPairDetailedRateModel> GetAssetPairDetailedRates(string token, string period=null, string asseId = null, int points = -1, bool withBid = false)
        {
            var request = Request.Get(resource).WithBearerToken(token).AddQueryParameter("withBid", withBid);
            if (period != null)
                request.AddQueryParameter("period", period);
            if (asseId != null)
                request.AddQueryParameter("asseId", asseId);
            if (points != -1)
                request.AddQueryParameter("points", points);
            return request.Build().Execute<ResponseModelGetAssetPairDetailedRateModel>();
        }
    }

    public class AssetPairs : ApiBase
    {
        string resource = "/AssetPairs";

        public IResponse<ResponseModelGetAssetPairsResponseModel> GetAssetPairs(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelGetAssetPairsResponseModel>();
        }
    }
}
