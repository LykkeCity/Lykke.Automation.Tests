using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MatchingEngineData.DTOs.RabbitMQ
{
    public class LimitOrdersResponse : IRabbitMQOperation
    {
        public List<LimitOrders> orders { get; set; }

    }
    public class LimitOrders
    {
        public NewLimitOrder order { get; set; }
        public List<LimitTradeInfo> trades { get; set; }

        public class NewLimitOrder
        {
            public string id { get; set; }
            public string externalId { get; set; }
            public string assetPairId { get; set; }
            public string clientId { get; set; }
            public Double volume { get; set; }
            public Double price { get; set; }
            public string status { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime registered { get; set; }
            public Double remainingVolume { get; set; }
            public DateTime? lastMatchTime { get; set; }
            public Double? reservedLimitVolume { get; set; }
        }

        public class LimitTradeInfo
        {
            public string clientId { get; set; }
            public string asset { get; set; }
            public string volume { get; set; }
            public Double price { get; set; }
            public DateTime timestamp { get; set; }
            public string oppositeOrderId { get; set; }
            public string oppositeOrderExternalId { get; set; }
            public string oppositeAsset { get; set; }
            public string oppositeClientId { get; set; }
            public String oppositeVolume { get; set; }
        }
    }
}
