using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class InstanceTradeDTO
    {
        public string InstanceId { get; set; }
        public bool IsBuy { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public string OrderId { get; set; }
        public string AssetPair { get; set; }
        public string TradedAssetName { get; set; }
        public string WalletId { get; set; }
        public DateTime DateOfTrade { get; set; }
    }
}
