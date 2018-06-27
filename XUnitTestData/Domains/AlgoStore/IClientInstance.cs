using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IClientInstance : IDictionaryItem
    {
        string AlgoClientId { get; set; }
        string AlgoId { get; set; }
        DateTime AlgoInstanceRunDate { get; set; }
        string AlgoInstanceStatusValue { get; set; }
        string AlgoInstanceTypeValue { get; set; }
        dynamic AlgoMetaDataInformation { get; set; }
        string AssetPair { get; set; }
        string AssetPairId { get; set; }
        string AuthToken { get; set; }
        string ClientId { get; set; }
        string HftApiKey { get; set; }
        string InstanceName { get; set; }
        bool IsStraight { get; set; }
        int Margin { get; set; }
        string OppositeAssetId { get; set; }
        string TradedAsset { get; set; }
        string TradedAssetId { get; set; }
        int Volume { get; set; }
        string WalletId { get; set; }
        string InstanceId { get; set; }
    }
}
