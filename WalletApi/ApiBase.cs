using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi
{
    public class ApiBase
    {
        protected string URL = "https://api-test.lykkex.net/api";

        protected IRequestBuilder Request => Requests.For(URL);
    }
}
