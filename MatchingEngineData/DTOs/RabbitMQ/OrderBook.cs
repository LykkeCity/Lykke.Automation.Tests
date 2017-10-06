using System;
using System.Collections.Generic;
using System.Text;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class OrderBook
    {
        public string assetPair { get; set; }
        public bool isBuy { get; set; }
        public DateTime timestamp { get; set; }
        public List<VolumePrice> prices { get; set; }


        public class VolumePrice
        {
            public string volume { get; set; }
            public string price { get; set; }
        }
    }
}
