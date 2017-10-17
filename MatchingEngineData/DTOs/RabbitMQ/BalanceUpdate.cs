using System;
using System.Collections.Generic;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class BalanceUpdate : IRabbitMQOperation
    {
        public string id { get; set; }
        public string type { get; set; }
        public DateTime timestamp { get; set; }
        public List<ClientBalanceUpdate> balances { get; set; }
        

        public class ClientBalanceUpdate
        {
            public string id { get; set; } // it's actually ClientId
            public string asset { get; set; }
            public double oldBalance { get; set; }
            public double newBalance { get; set; }
            public double oldReserved { get; set; }
            public double newReserved { get; set; }

        }

    }
}
