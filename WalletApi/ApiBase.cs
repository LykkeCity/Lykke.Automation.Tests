using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate;
using NUnit.Framework;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi
{
    public class ApiBase
    {
        private static readonly string Url = 
            EnvConfig.Env == Env.Test ? "https://api-test.lykkex.net/api" :
            EnvConfig.Env == Env.Dev ? "https://api-test.lykkex.net/api" :
            throw new Exception("Undefined env");

        public static string ApiUrl => Url;

        protected IRequestBuilder Request => Requests.For(Url);
    }
}
