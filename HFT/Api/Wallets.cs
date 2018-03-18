using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace HFT.Api
{
    public class Wallets: ApiBase
    {
        public IResponse<ClientBalanceResponseModel> GetWallets(string apiKey)
        {
            return Request.Get("/Wallets").Build().Execute<ClientBalanceResponseModel>();
        }
    }
}
