using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.Sign
{
    public class BlockchainSign : ApiBase
    {
        const string BASE_URL = "http://litecoin-sign.autotests-service.svc.cluster.local/api";

        public BlockchainSign(string URL) : base(URL) { }
        public BlockchainSign() : base(BASE_URL) { }

        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/isalive").Build().Execute<IsAliveResponse>();
        }

        public IResponse<SignOkTransactionResponce> PostSign(SignRequest model)
        {
            return Request.Post("/sign").AddJsonBody(model).Build().Execute<SignOkTransactionResponce>();
        }

        public IResponse<WalletCreationResponse> PostWallet()
        {
            return Request.Post("/wallets").Build().Execute<WalletCreationResponse>();
        }
    }
}
