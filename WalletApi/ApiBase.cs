using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi
{
    public class ApiBase
    {
        //TODO: To config
        protected string URL = Environment.GetEnvironmentVariable("WalletApiUrl") 
                               ?? "https://api-test.lykkex.net/api";

        protected IRequestBuilder Request => Requests.For(URL);
    }
}
