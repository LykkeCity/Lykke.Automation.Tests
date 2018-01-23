using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;

namespace AFTests.ApiRegression
{
    public class ApiRegressionBaseTest : BaseTest
    {
        protected WalletApi.Api.WalletApi walletApi = new WalletApi.Api.WalletApi();
    }
}
