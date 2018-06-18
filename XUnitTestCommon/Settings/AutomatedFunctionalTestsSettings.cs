using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;

namespace XUnitTestCommon.Settings
{
    public class AutomatedFunctionalTestsSettings
    {
        public AssetsSettings Assets { get; set; }
        public FixSettings FIX { get; set; }
        public ServicesSettings Services { get; set; }
        public ClientAccountSettings ClientAccount { get; set; }
        public MatchingEngineSettings MatchingEngine { get; set; }
        public ApiV2Settings ApiV2 { get; set; }
        public BlueApiSettings BlueApi { get; set; }
        public BalancesSettings Balances { get; set; }
        public AlgoStoreSettings AlgoStore { get; set; }
    }
}
