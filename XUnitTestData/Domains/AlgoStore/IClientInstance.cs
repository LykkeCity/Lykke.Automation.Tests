using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IClientInstance : IDictionaryItem
    {
        string AssetPair { get; set; }
        string HftApiKey { get; set; }
        int Margin { get; set; }
        string TradedAsset { get; set; }
        int Volume { get; set;}
    }
}
