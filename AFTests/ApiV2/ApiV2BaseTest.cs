using System;
using System.Collections.Generic;
using System.Text;
using ApiV2Data.Api;
using BlockchainsIntegration.Sign;
using LykkeAutomationPrivate.Api;
using XUnitTestCommon.Tests;


namespace AFTests.ApiV2
{
    class ApiV2BaseTest : BaseTest
    {
        protected ApiV2Client apiV2 = new ApiV2Client();
        protected Wallet wallet = new Wallet();
        protected LykkeApi lykkePrivateApi = new LykkeApi();
        protected BlockchainSign blockchainSign;
    }

    public class Wallet
    {
        public string WalletAddress
        {
            get
            {
                return "lykke_autotest_021b074415@lykke.com";
            }
        }

        public string WalletKey
        {
            get
            {
                return "0fc1dbf03917f8eeb8d5e0722cf473141ba2fe048e1820b5743ba054d090f425";
            }
        }

        public string AuthorizationToken
        {
            get
            {
                return "";
            }
        }
    }
}
