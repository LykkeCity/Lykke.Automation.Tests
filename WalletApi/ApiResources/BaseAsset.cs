using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BaseAsset : WalletApi
    {
        string resource = "/BaseAsset";

        public IResponse<ResponseModelGetClientBaseAssetRespModel> GetBaseAsset(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelGetClientBaseAssetRespModel>();
        }

        public IResponse<ResponseModelGetClientBaseAssetRespModel> PostBaseAsset(PostClientBaseCurrencyModel model, string token)
        {
            return Request.Post(resource).WithBearerToken(token).AddJsonBody(model).Build().Execute<ResponseModelGetClientBaseAssetRespModel>();
        }
    }
}
