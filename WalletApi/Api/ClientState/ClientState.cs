using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ClientState : ApiBase
    {
        public IResponse<ResponseModelClientStateModel> GetClientState(string email, string partnerId)
        {
            return Request.Get("ClientState")
                .AddQueryParameterIfNotNull(nameof(email), email)
                .AddQueryParameterIfNotNull(nameof(partnerId), partnerId)
                .Build().Execute<ResponseModelClientStateModel>();
        }
    }
}
