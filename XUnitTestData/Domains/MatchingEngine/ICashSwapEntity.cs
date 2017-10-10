using System;

namespace XUnitTestData.Domains.MatchingEngine
{
    public interface ICashSwap : IDictionaryItem
    {
        double Amount { get; set; }
        double Amount1 { get; set; }
        double Amount2 { get; set; }
        string AssetId { get; set; }
        string AssetId1 { get; set; }
        string AssetId2 { get; set; }
        string ClientId1 { get; set; }
        string ClientId2 { get; set; }
        DateTime DateTime { get; set; }
        string ExternalId { get; set; }
        string FromClientId { get; set; }
        string ToClientId { get; set; }
    }
}