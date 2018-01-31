using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LiteCoin.Api
{
    public class Assets : ApiBase
    {
        public Assets(string url) : base(url) { }

        public Assets() : base() { }

        string resource = "/assets";

        public IResponse<PaginationResponseAssetContract> GetAssets(string take, string continuation)
        {
            return Request.Get(resource).AddQueryParameterIfNotNull("take", take).AddQueryParameterIfNotNull("continuation", continuation).Build().Execute<PaginationResponseAssetContract>();
        }

        public IResponse<AssetResponse> GetAsset(string assetId)
        {
            return Request.Get(resource + $"/{assetId}").Build().Execute<AssetResponse>();
        }
    }
}
