namespace HFT.Api
{
    using LykkeAutomationPrivate;
    using System;
    using XUnitTestCommon.RestRequests;
    using XUnitTestCommon.RestRequests.Interfaces;
    using XUnitTestCommon.TestsCore;

    public class ApiBase
    {
        protected string URL = 
            EnvConfig.Env == Env.Test ? "https://hft-api-test.lykkex.net/api" :
            EnvConfig.Env == Env.Dev ? "https://hft-service-dev.lykkex.net/api" :
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
