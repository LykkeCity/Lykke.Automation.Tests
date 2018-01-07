using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Assets : ApiBase
    {
        string resource = "/Assets";

        public IResponse<ResponseModelGetBaseAssetsRespModel> GetAssets()
        {
            return Request.Get(resource).Build().Execute<ResponseModelGetBaseAssetsRespModel>();
        }

        public IResponse<ResponseModelGetClientBaseAssetRespModel> GetAssetId(string id)
        {
            return Request.Get(resource + $"/{id}").Build().Execute<ResponseModelGetClientBaseAssetRespModel>();
        }

        public IResponse<ResponseModelAssetAttributesModel> GetAssetIdAttributes(string id, string token)
        {
            return Request.Get(resource + $"/{id}/attributes").WithBearerToken(token).Build().Execute<ResponseModelAssetAttributesModel>();
        }

        public IResponse<ResponseModelAssetAttribute> GetAssetIdAttribute(string id, string attribute, string token)
        {
            return Request.Get(resource + $"/{id}/attributes/{attribute}").WithBearerToken(token).Build().Execute<ResponseModelAssetAttribute>();
        }

        public IResponse<ResponseModelAssetDescriptionsListModel> PostAssetsDescriptionList(GetAssetDescriptionsListModel  listModel)
        {
            return Request.Post(resource + "/description/list").AddJsonBody(listModel).Build().Execute<ResponseModelAssetDescriptionsListModel>(); //why dont need any authorization here???
        }
    }
}
