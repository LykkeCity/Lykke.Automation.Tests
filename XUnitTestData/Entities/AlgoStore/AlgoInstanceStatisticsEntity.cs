using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class AlgoInstanceStatisticsEntity : TableEntity, IAlgoInstanceStatistics
    {
        public string Id => this.RowKey;
        public bool IsStarted { get; set; }
        public double AssetOneBalance { get; set; }
        public double AssetTwoBalance { get; set; }
        public double InitialWalletBalance { get; set; }
        public double LastWalletBalance { get; set; }
        public int TotalNumberOfStarts { get; set; }
        public int TotalNumberOfTrades { get; set; }
        public string UserCurrencyBaseAssetId { get; set; }
        public bool IsBuy { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public string InstanceId { get; set; }
        public double InitialTradedAssetBalance { get; set; }
        public double InitialAssetTwoBalance { get; set; }
        public double LastTradedAssetBalance { get; set; }
        public double LastAssetTwoBalance { get; set; }
    }
}
