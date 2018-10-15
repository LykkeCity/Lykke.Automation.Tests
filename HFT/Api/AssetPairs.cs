namespace HFT.Api
{
    using Lykke.Client.AutorestClient.Models;
    using System.Collections.Generic;
    using XUnitTestCommon.RestRequests.Interfaces;

    public class AssetPairs : ApiBase
    {
        public IResponse<List<AssetPairModel>> GetAssetPairs()
        {
            return Request.Get("/AssetPairs").Build().Execute<List<AssetPairModel>>();
        }

        public IResponse<AssetPairModel> GetAssetPairs(string id)
        {
            return Request.Get($"/AssetPairs/{id}").Build().Execute<AssetPairModel>();
        }
    }
}
