using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LitecoinSign
{
    public class LitecoinSign : ApiBase
    {
        public LitecoinSign() : base("http://blockchain-sign-service-litecoin.lykke-service.svc.cluster.local/api") { }

        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/IsAlive").Build().Execute<IsAliveResponse>();
        }

        public IResponse<SignTransactionResponse> PostSign(SignTransactionRequest model)
        {
            return Request.Post("/Sign").AddJsonBody(model).Build().Execute<SignTransactionResponse>();
        }

        public IResponse<WalletCreationResponse> PostWallet()
        {
            return Request.Post("/Wallet").Build().Execute<WalletCreationResponse>();
        }
    }
}
