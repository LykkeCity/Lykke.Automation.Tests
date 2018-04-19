using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace PrivateServices.Resources.Assets
{
    public class Assets
    {
        private const string BaseUrl = "http://assets.lykke-service.svc.cluster.local/api/v2";

        public IResponse<List<AssetPair>> GetAssetPairs()
        {
            return Requests.For(BaseUrl).Get("/asset-pairs").Build().Execute<List<AssetPair>>();
        }

        public IResponse<List<Asset>> GetAssets(bool includeNonTradable = true)
        {
            return Requests.For(BaseUrl).Get("/assets").AddQueryParameter("includeNonTradable", includeNonTradable).Build().Execute<List<Asset>>();
        }
    }
}
