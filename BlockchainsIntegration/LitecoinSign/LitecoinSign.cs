using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LitecoinSign
{
    public class LitecoinSign : ApiBase
    {
        public LitecoinSign() : base("http://litecoin-sign.autotests-service.svc.cluster.local/api") { }

        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/IsAlive").Build().Execute<IsAliveResponse>();
        }

        public IResponse<SignOkTransactionResponce> PostSign(SignRequest model)
        {
            return Request.Post("/Sign").AddJsonBody(model).Build().Execute<SignOkTransactionResponce>();
        }

        public IResponse<WalletCreationResponse> PostWallet()
        {
            return Request.Post("/Wallet").Build().Execute<WalletCreationResponse>();
        }
    }
}
