using System;
using System.Collections.Generic;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class OrderBook : IRabbitMQOperation
    {
        public string assetPair { get; set; }
        public bool isBuy { get; set; }
        public DateTime timestamp { get; set; }
        public List<VolumePrice> prices { get; set; }


        public class VolumePrice
        {
            public string id { get; set; }
            public string clientId { get; set; }
            public double volume { get; set; }
            public double price { get; set; }
        }
    }
}
