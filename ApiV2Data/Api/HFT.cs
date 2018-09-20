using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class HFT : ApiBase
    {
        public IResponse<CreateApiKeyResponse> PutHFWalletRegenerateKey(string walletId, string token)
        {
            return Request.Put($"/hft/{walletId}/regenerateKey").WithBearerToken(token).Build().Execute<CreateApiKeyResponse>();
        }
    }
}
