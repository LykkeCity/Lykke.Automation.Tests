using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.BlockchainWallets
{
    public class BlockchainWallets : ApiBase
    {
        public BlockchainWallets() : base("http://blockchain-wallets.autotests-service.svc.cluster.local/api") { }

        public IResponse GetWalletByAddress(string integrationLayerId, string integrationLayerAssetId, string address)
        {
            return Request.Get($"/wallets/{integrationLayerId}/{integrationLayerAssetId}/by-addresses/{address}/client-id").Build().Execute();
        }

        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get($"/isalive").Build().Execute<IsAliveResponse>();
        }

        public IResponse GetWalletByClientId(string integrationLayerId, string integrationLayerAssetId, string clientId)
        {
            return Request.Get($"/wallets/{integrationLayerId}/{integrationLayerAssetId}/by-client-ids/{clientId}/address").Build().Execute();
        }

        public IResponse PostWalletByClientId(string integrationLayerId, string integrationLayerAssetId, string clientId)
        {
            return Request.Post($"/wallets/{integrationLayerId}/{integrationLayerAssetId}/by-client-ids/{clientId}").Build().Execute();
        }

        public IResponse DeleteWalletByClientId(string integrationLayerId, string integrationLayerAssetId, string clientId)
        {
            return Request.Delete($"/wallets/{integrationLayerId}/{integrationLayerAssetId}/by-client-ids/{clientId}").Build().Execute();
        }
    }
}
