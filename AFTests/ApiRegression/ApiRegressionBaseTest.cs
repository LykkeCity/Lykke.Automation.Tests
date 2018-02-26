using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;

namespace AFTests.ApiRegression
{
    public class ApiRegressionBaseTest : BaseTest
    {
        protected WalletApi.Api.WalletApi walletApi = new WalletApi.Api.WalletApi();
        //TODO: Move to config
        protected string email = "untest005@test.com";
        protected string password = "1234567";
        protected string pin = "1111";
    }
}
