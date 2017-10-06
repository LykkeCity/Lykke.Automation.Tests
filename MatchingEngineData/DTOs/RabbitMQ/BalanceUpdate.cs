using System;
using System.Collections.Generic;
using System.Text;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class BalanceUpdate
    {
        public string id { get; set; }
        public string type { get; set; }
        public DateTime timestamp { get; set; }
        public List<ClientBalanceUpdate> ballances { get; set; }
        

        public class ClientBalanceUpdate
        {
            public string id { get; set; } // it's actually ClientId
            public string asset { get; set; }
            public string oldBalance { get; set; }
            public string newBalance { get; set; }
        }

    }
}
