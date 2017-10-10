using System;
using System.Collections.Generic;
using System.Text;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class CashOperation : IRabbitMQOperation
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public DateTime dateTime { get; set; }
        public string volume { get; set; }
        public string asset { get; set; }

    }
}
