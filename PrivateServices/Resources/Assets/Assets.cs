using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace PrivateServices.Resources.Assets
{
    public class Assets
    {
        private string BaseUrl =
              EnvConfig.Env == Env.Test ? "http://assets.lykke-service.svc.cluster.local/api/v2" :
              EnvConfig.Env == Env.Dev ? "http://assets.lykke-service.svc.cluster.local/api/v2" :
            throw new Exception("Undefined env");

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
