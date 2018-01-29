using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;

namespace BlockchainsIntegration
{
    public class ApiBase
    {
        protected string URL = "http://litecoin-api.autotests-service.svc.cluster.local/api";

        protected IRequestBuilder Request => Requests.For(URL);

        public ApiBase() { AllurePropertiesBuilder.Instance.AddPropertyPair("service", URL); }

        public ApiBase(string URL)
        {
            if(URL !=null)
                this.URL = URL;
            AllurePropertiesBuilder.Instance.AddPropertyPair("service", URL);
        }
    }
}
