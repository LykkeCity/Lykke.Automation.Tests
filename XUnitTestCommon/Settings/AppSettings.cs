using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings
{
    public class AppSettings
    {
        public AlgoApiSettings AlgoApi { get; set; }
        public AutomatedFunctionalTestsSettings AutomatedFunctionalTests { get; set; }
        public BlockchainIntegrationSettings BlockchainsIntegration { get; set; }
        public AlgoApiClientSettings AlgoApiClient { get; set; }
    }
}
