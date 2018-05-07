using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.Api
{
    public class Constants: ApiBase
    {
        public Constants(string URL) : base(URL)
        {
        }

        public IResponse<ConstantsResponse> GetConstants()
        {
            return Request.Get("/constants").Build().Execute<ConstantsResponse>();
        }
    }
}
