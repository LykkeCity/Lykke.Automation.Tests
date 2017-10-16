using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.MatchingEngine
{
    public interface IMarketOrderEntity : IDictionaryItem
    {
        string Id { get; set; }
        string AssetPairId { get; set; }
        string ClientId { get; set; }
        DateTime CreatedAt { get; set; }
        bool? Registered { get; set; }
        string Status { get; set; }
        bool Straight { get; set; }
        double Volume { get; set; }
        DateTime MatchedAt { get; set; }
        double Price { get; set; }
        string DustSize { get; set; }
    }
}
