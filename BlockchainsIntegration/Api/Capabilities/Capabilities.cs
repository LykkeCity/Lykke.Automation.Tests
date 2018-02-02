using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.Api
{
    public class Capabilities : ApiBase
    {
        public Capabilities() : base() { }

        public Capabilities(string BaseUrl) : base(BaseUrl) { }

        public IResponse<CapabilitiesResponse> GetCapabilities()
        {
            return Request.Get("/capabilities").Build().Execute<CapabilitiesResponse>();
        }
    }
}
