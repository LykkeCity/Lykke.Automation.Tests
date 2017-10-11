using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class OrderBook : IRabbitMQOperation
    {
        public string id => (prices.FirstOrDefault() ?? new VolumePrice()).id; //TODO
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
