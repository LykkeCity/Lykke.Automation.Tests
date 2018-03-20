using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;
using Lykke.Client.AutorestClient.Models;

namespace WalletApi.Api
{
    public class AssetDisclaimers : ApiBase
    {
        public IResponse<ResponseModelAssetDisclaimerResponceModel> Get(string token) =>
            Request.Get("/AssetDisclaimers").WithBearerToken(token)
                .Build().Execute<ResponseModelAssetDisclaimerResponceModel>();

        public IResponse<ResponseModel> PostApproveById(string disclaimerId, string token) =>
            Request.Post($"/AssetDisclaimers/{disclaimerId}/approve").WithBearerToken(token)
                .Build().Execute<ResponseModel>();

        public IResponse<ResponseModel> PostDeclineById(string disclaimerId, string token) =>
            Request.Post($"/AssetDisclaimers/{disclaimerId}/decline").WithBearerToken(token)
                .Build().Execute<ResponseModel>();
    }
}
