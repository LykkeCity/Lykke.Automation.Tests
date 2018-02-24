using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Offchain : ApiBase
    {
        public IResponse<ResponseModel> PostLimitCancel(OffchainLimitCancelModel cancelModel, string token) =>
            Request.Post("/offchain/limit/cancel").WithBearerToken(token)
                .AddJsonBody(cancelModel).Build().Execute<ResponseModel>();

        //TODO: Add other implementation
    }
}
