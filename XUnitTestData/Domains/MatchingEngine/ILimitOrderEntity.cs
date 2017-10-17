using System;

namespace XUnitTestData.Domains.MatchingEngine
{
    public interface ILimitOrderEntity : IDictionaryItem
    {
        string AssetPairId { get; set; }
        string ClientId { get; set; }
        DateTime CreatedAt { get; set; }
        string Id { get; set; }
        string MatchingId { get; set; }
        double Price { get; set; }
        double RemainingVolume { get; set; }
        string Status { get; set; }
        bool Straight { get; set; }
        double Volume { get; set; }
    }
}