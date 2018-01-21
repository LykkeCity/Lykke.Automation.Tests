using BlockchainsIntegration.BlockchainWallets;
using BlockchainsIntegration.LiteCoin;
using BlockchainsIntegration.LitecoinSign;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;

namespace AFTests.BlockchainsIntegration.LiteCoin
{
    class LitecoinBaseTest : BaseTest
    {
        protected LitecoinApi litecoinApi = new LitecoinApi();
        protected LitecoinSign litecoinSign = new LitecoinSign();
        protected BlockchainWallets blockchainWallets = new BlockchainWallets();

        protected static string HOT_WALLET = "mwy2LRNecLfHxatdAxz1XQP2sqv8Nk3PFV";
        protected static string WALLET_ADDRESS = "muk6Vxdd7Uy629ivnLbADfFrFekX3RDZcK";
        protected static string PKey = "cRTB3eAajJchgNuybhH5SwC9L5PFoTwxXBjgB8vRNJeJ4EpcXmAP";


        protected static string CLIENT_ID = "b623b171-a307-4485-897c-f3a70b763217";
        //fill here http://faucet.thonguyen.net/ltc
    }
}
