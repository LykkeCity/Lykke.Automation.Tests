using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Wallets : ApiBase
    {
        public IResponse<ResponseModelGetWaletsRespModel> GetWallets(string token)
        {
            return Request.Get("/Wallets").WithBearerToken(token)
                .Build().Execute<ResponseModelGetWaletsRespModel>();
        }

        public IResponse<ResponseModelDepositAddressModel> PostWallets(SubmitKeysModel submitKeys, string token)
        {
            return Request.Post("/Wallets").WithBearerToken(token).AddJsonBody(submitKeys)
                .Build().Execute<ResponseModelDepositAddressModel>();
        }

        public IResponse<ResponseModelApiWalletAssetModel> GetWalletsById(string id, string token)
        {
            return Request.Get($"/Wallets/{id}").WithBearerToken(token)
                .Build().Execute<ResponseModelApiWalletAssetModel>();
        }
    }
}
