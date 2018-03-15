using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LiteCoin.Api
{
    public class Balances : ApiBase
    {
        public Balances(string url) : base(url) { }

        public Balances() : base() { }

        string resource = "/balances";

        public IResponse<PaginationResponseWalletBalanceContract> GetBalances(string take, string continuation)
        {
            return Request.Get(resource)
                .AddQueryParameterIfNotNull("take", take).AddQueryParameterIfNotNull("continuation", continuation).Build().Execute<PaginationResponseWalletBalanceContract>();
        }

        public IResponse PostBalances(string address)
        {
            return Request.Post(resource + $"/{address}/observation").Build().Execute();
        }

        public IResponse DeleteBalances(string address)
        {
            return Request.Delete(resource + $"/{address}/observation").Build().Execute();
        }
    }
}
