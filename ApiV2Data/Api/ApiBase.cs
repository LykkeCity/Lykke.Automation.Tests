using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;

namespace ApiV2Data.Api
{
    public class ApiBase
    {
        protected string URL = 
            EnvConfig.Env == Env.Test ? "https://apiv2-test.lykkex.net/api" :
            EnvConfig.Env == Env.Dev ? "https://apiv2-dev.lykkex.net/api" :
            throw new Exception("Undefined env");

        public IRequestBuilder Request => Requests.For(URL);

        public ApiBase() { AllurePropertiesBuilder.Instance.AddPropertyPair("service", URL); }

        public ApiBase(string URL)
        {
            if (URL != null)
                this.URL = URL;
            AllurePropertiesBuilder.Instance.AddPropertyPair("service", URL);
        }
    }
}
