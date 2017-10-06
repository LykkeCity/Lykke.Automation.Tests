using System;
using System.Collections.Generic;
using System.Text;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    class CashSwapOperation
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
