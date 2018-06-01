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
        string AlgoId { get; set; }
        dynamic AlgoMetaDataInformation { get; set; }
        string ClientId { get; set; }
        string InstanceName { get; set; }
        string WalletId { get; set; }
        string AlgoInstanceStatusValue { get; set; }
        string AlgoInstanceTypeValue { get; set; }
        bool IsStraight { get; set; }
        string OppositeAssetId { get; set; }
        string AuthToken { get; set; }
    }
}
