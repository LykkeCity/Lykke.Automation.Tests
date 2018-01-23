using WalletApi.Api;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;

namespace AFTests.WalletApiTests
{
    class WalletApiBaseTest : BaseTest
    {
       protected WalletApi.Api.WalletApi walletApi = new WalletApi.Api.WalletApi();
    }
}
