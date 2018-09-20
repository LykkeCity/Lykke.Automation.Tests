using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class IsAlive : ApiBase
    {
        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/IsAlive").Build().Execute<IsAliveResponse>();
        }
    }
}
