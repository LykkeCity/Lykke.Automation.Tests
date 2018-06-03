using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IAlgoInstanceStatistics : IDictionaryItem
    {
        bool IsStarted { get; set; }
        double AssetOneBalance { get; set; }
        double AssetTwoBalance { get; set; }
        double InitialWalletBalance { get; set; }
        double LastWalletBalance { get; set; }
        int TotalNumberOfStarts { get; set; }
        int TotalNumberOfTrades { get; set; }
        string UserCurrencyBaseAssetId { get; set; }
        bool IsBuy { get; set; }
        double Price { get; set; }
        double Amount { get; set; }
        string InstanceId { get; set; }
    }
}
