using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.BlockchainIntegration
{
    public class BlockchainSettings
    {
        public String Type { get; set; }
        public String ApiUrl { get; set; }
        public String SignServiceUrl { get; set; }
        public String HotWalletAddress { get; set; }
        public MonitoringSettings Monitoring { get; set; }
    }
}
