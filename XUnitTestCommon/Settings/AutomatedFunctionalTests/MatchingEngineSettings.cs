using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests
{
    public class MatchingEngineSettings
    {
        public String BaseUrl { get; set; }
        public String Port { get; set; }
        public String RabbitMQHost { get; set; }
        public String RabbitMQPort { get; set; }
        public String RabbitMQamqpPort { get; set; }
        public String RabbitMQUsername { get; set; }
        public String RabbitMQPassword { get; set; }
        public String RabbitMQNode { get; set; }
        public String RabbitMQMessageWait { get; set; }
        public String AssetPrecission { get; set; }
        public String BalancesInfoConnectionString { get; set; }
        public String LimitOrdersConnectionString { get; set; }
        public String DictionariesConnectionString { get; set; }
    }
}
