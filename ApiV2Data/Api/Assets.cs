using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Assets : ApiBase
    {
        public IResponse<AssetsModel> GetAssets()
        {
            return Request.Get("/assets").Build().Execute<AssetsModel>();
        }

        public IResponse<AssetRespModel> GetAssetsById(string id)
        {
            return Request.Get($"/assets/{id}").Build().Execute<AssetRespModel>();
        }

        public IResponse<AssetAttributesModel> GetAssetAttributesByid(string assetId)
        {
            return Request.Get($"/assets/{assetId}/attributes").Build().Execute<AssetAttributesModel>();
        }

        public IResponse<KeyValue> GetAssetAttributeKeyById(string assetId, string assetAttributeKey)
        {
            return Request.Get($"GET /api/assets/{assetId}/attributes/{assetAttributeKey}").Build().Execute<KeyValue>();
        }

        public IResponse<AssetDescriptionsModel> GetAssetsDescription()
        {
            return Request.Get("/assets/description").Build().Execute<AssetDescriptionsModel>();
        }

        public IResponse<AssetDescriptionModel> GetAssetDescription(string assetId)
        {
            return Request.Get($"/assets/{assetId}/description").Build().Execute<AssetDescriptionModel>();
        }

        public IResponse<AssetCategoriesModel> GetAssetsCategories()
        {
            return Request.Get("/assets/categories").Build().Execute<AssetCategoriesModel>();
        }

        public IResponse<AssetCategoriesModel> GetAssetsCategoriesId(string id)
        {
            return Request.Get($"/assets/categories/{id}").Build().Execute<AssetCategoriesModel>();
        }

        public IResponse<BaseAssetClientModel> GetBaseAsset(string token)
        {
            return Request.Get($"/assets/baseAsset").WithBearerToken(token).Build().Execute<BaseAssetClientModel>();
        }

        public IResponse PostAssetBaseAsset(BaseAssetUpdateModel baseAsset, string token)
        {
            return Request.Post($"/assets/baseAsset").WithBearerToken(token).AddJsonBody(baseAsset).Build().Execute();
        }

        public IResponse<AssetIdsModel> GetAssetsAvailable(string token)
        {
            return Request.Get("/assets/available").WithBearerToken(token).Build().Execute<AssetIdsModel>();
        }
    }
}
