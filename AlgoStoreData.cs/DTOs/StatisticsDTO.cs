using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class StatisticsDTO
    {
        public string InstanceId { get; set; }
        public int TotalNumberOfTrades { get; set; }
        public int TotalNumberOfStarts { get; set; }
        public double InitialWalletBalance { get; set; }
        public double LastWalletBalance { get; set; }
        public double AssetOneBalance { get; set; }
        public double AssetTwoBalance { get; set; }
        public string UserCurrencyBaseAssetId { get; set; }
        public double NetProfit { get; set; }
        public string TradedAssetName { get; set; }
        public string AssetTwoName { get; set; }
    }
}
