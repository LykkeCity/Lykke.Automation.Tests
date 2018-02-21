using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Dicts : ApiBase
    {
        public IResponse<ResponseModelAssetDictRespModel> GetAssetts(string token)
        {
            return Request.Get("/Dicts/assets").WithBearerToken(token)
                .Build().Execute<ResponseModelAssetDictRespModel>();
        }

        public IResponse<ResponseModelDictionariesUpdatesRespModel> GetUpdates(string token)
        {
            return Request.Get("/Dicts/updates").WithBearerToken(token)
                .Build().Execute<ResponseModelDictionariesUpdatesRespModel>();
        }
    }
}
