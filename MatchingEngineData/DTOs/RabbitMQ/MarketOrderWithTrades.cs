using System;
using System.Collections.Generic;
using System.Text;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class MarketOrderWithTrades : IRabbitMQOperation
    {
        public string id => order.id;
        public MarketOrder order { get; set; }
        public List<TradeInfo> trades { get; set; }


        public class MarketOrder
        {
            public string id { get; set; } //internal id from ME
            public string externalId { get; set; }//order id
            public string assetPairId { get; set; }
            public string clientId { get; set; }
            public string volume { get; set; }
            public string price { get; set; }
            public string status { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime registered { get; set; }
            public DateTime? matchedAt { get; set; }
            public bool straight { get; set; }
            public string reservedLimitVolume { get; set; }
            public string dustSize { get; set; }

        }

        public class TradeInfo
        {
            public string marketClientId { get; set; }
            public string marketVolume { get; set; }
            public string marketAsset { get; set; }
            public string limitClientId { get; set; }
            public string limitVolume { get; set; }
            public string limitAsset { get; set; }
            public string price { get; set; }
            public string limitOrderId { get; set; }
            public string limitOrderExternalId { get; set; }
            public DateTime timestamp { get; set; }

        }
    }
}
