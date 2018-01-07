using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BaseAssets : WalletApi
    {
        string resource = "/BaseAssets";

        public IResponse<ResponseModelGetBaseAssetsRespModel> GetBaseAssets(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelGetBaseAssetsRespModel>();
        }
    }
}
