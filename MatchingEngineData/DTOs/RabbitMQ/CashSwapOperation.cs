using System;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class CashSwapOperation : IRabbitMQOperation
    {
        public string id { get; set; }
        public DateTime dateTime { get; set; }

        public string clientId1 { get; set; }
        public string asset1 { get; set; }
        public string volume1 { get; set; }

        public string clientId2 { get; set; }
        public string asset2 { get; set; }
        public string volume2 { get; set; }
    }
}
