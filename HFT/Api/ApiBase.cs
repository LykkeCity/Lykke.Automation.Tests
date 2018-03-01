using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace HFT.Api
{
    public class ApiBase
    {
        protected string URL = "https://hft-api-test.lykkex.net/api";

        public IRequestBuilder Request => Requests.For(URL);

        public ApiBase() { }

        public ApiBase(string URL)
        {
            if (URL != null)
                this.URL = URL;
        }
    }
}
