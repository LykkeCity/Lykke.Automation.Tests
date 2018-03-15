using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LiteCoin.Api
{
    public class IsAlive : ApiBase
    {
        public IsAlive(string url) : base(url) { }

        public IsAlive() : base() { }

        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/isalive").Build().Execute<IsAliveResponse>();
        }
    }
}
