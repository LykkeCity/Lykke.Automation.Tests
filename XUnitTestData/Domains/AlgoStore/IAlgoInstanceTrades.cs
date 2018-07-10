using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IAlgoInstanceTrades : IDictionaryItem
    {
        string InstanceId { get; set; }
        bool IsBuy { get; set; }
        double Price { get; set; }
        double Amount { get; set; }
        string OrderId { get; set; }
        string WalletId { get; set; }
        double Fee { get; set; }
        string AssetPairId { get; set; }
        string AssetId { get; set; }
        DateTime DateOfTrade { get; set; }
    }
}
