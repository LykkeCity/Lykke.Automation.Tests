using System;
using System.Collections.Generic;
using System.Text;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class CashTransferOperation : IRabbitMQOperation
    {
        public string id { get; set; }
        public string fromClientId { get; set; }
        public string toClientId { get; set; }
        public DateTime dateTime { get; set; }
        public string volume { get; set; }
        public string asset { get; set; }
    }
}
