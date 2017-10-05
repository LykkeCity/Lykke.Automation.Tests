using System;
using System.Collections.Generic;
using System.Text;

namespace AFTMatchingEngine.DTOs
{
    public class RabbitMQCashOperation
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public string dateTime { get; set; }
        public string volume { get; set; }
        public string asset { get; set; }

    }
}
