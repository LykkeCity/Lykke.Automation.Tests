using Microsoft.WindowsAzure.Storage.Table;
using System;
using XUnitTestData.Domains;
using XUnitTestData.Domains.MatchingEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Entities.MatchingEngine
{
    public class LimitOrderEntity : TableEntity, ILimitOrderEntity
    {
        public string AssetPairId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
        public string MatchingId { get; set; }
        public double Price { get; set; }
        public double RemainingVolume { get; set; }
        public string Status { get; set; }
        public bool Straight { get; set; }
        public double Volume { get; set; }
    }
}
