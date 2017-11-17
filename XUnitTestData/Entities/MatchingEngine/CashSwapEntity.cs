using Microsoft.WindowsAzure.Storage.Table;
using System;
using XUnitTestData.Domains.MatchingEngine;

namespace XUnitTestData.Entities.MatchingEngine
{
    public class CashSwapEntity : TableEntity, ICashSwap
    {
        public string Id => ExternalId;
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public DateTime DateTime { get; set; }
        public string ExternalId { get; set; }
        public string FromClientId { get; set; }
        public string ToClientId { get; set; }

        public double Amount1 { get; set; }
        public double Amount2 { get; set; }
        public string AssetId1 { get; set; }
        public string AssetId2 { get; set; }
        public string ClientId1 { get; set; }
        public string ClientId2 { get; set; }

    }
}
