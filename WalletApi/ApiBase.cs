using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi
{
    public class ApiBase
    {
        private static readonly string Url = Environment.GetEnvironmentVariable("WalletApiUrl")
                                             ?? TestContext.Parameters["WalletApiUrl"]
                                             ?? "https://api-dev.lykkex.net/api";
        public static string ApiUrl => Url;

        protected IRequestBuilder Request => Requests.For(Url);
    }
}
