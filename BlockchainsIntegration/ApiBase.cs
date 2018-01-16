using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration
{
    public class ApiBase
    {
        protected string URL = "http://litecoin-api.lykke-service.svc.cluster.local/api";

        protected IRequestBuilder Request => Requests.For(URL);

        public ApiBase() { }

        public ApiBase(string URL)
        {
            this.URL = URL;
        }
    }
}
