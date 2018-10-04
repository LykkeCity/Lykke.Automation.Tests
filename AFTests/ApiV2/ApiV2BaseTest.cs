using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiV2Data.Api;
using BlockchainsIntegration.Sign;
using Lykke.Client.ApiV2.Models;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.Api;
using NUnit.Framework;
using XUnitTestCommon.Tests;


namespace AFTests.ApiV2
{
    public class BlockchainSettings
    {
        public bool AreCashinsDisabled { get; set; }
        public string Type { get; set; }
        public string ApiUrl { get; set; }
        public string SignServiceUrl { get; set; }
        public string HotWalletAddress { get; set; }
        public Monitoring Monitoring { get; set; }
    }

    public class Monitoring
    {
        public string InProgressOperationAlarmPeriod { get; set; }
    }

    public class ApiV2BaseTest : BaseTest
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
                return "lykke_autotest_021b074415@lykke.com";// ;"dev_dev@dev.com"
            }
        }

        public string WalletKey
        {
            get
            {
                return "0fc1dbf03917f8eeb8d5e0722cf473141ba2fe048e1820b5743ba054d090f425";//;"123456789qQ"
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
