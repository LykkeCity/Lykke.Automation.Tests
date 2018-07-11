using Microsoft.WindowsAzure.Storage.Table;
using System;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class AlgoInstanceTradesEntity : TableEntity, IAlgoInstanceTrades
    {
        public string Id { get => OrderId; set { } }
        public string InstanceId { get; set; }
        public bool IsBuy { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public string OrderId { get; set; }
        public string WalletId { get; set; }
        public double Fee { get; set; }
        public string AssetPairId { get; set; }
        public string AssetId { get; set; }
        public DateTime DateOfTrade { get; set; }
    }
}
