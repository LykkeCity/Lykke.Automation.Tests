using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;

namespace WalletApi.Api
{
    public class OrderBook : ApiBase
    {
        public IResponse<ResponseModelApiOrderBook> GetById(string id, string token) =>
            Request.Get($"/OrderBook/{id}").WithBearerToken(token)
                .Build().Execute<ResponseModelApiOrderBook>();
    }
}
