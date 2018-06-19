using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;
using Microsoft.WindowsAzure.Storage.Table;

namespace XUnitTestData.Entities.AlgoStore
{
    public class ClientInstanceEntity : TableEntity, IClientInstance
    {
        public string Id => RowKey;
        public string AlgoClientId { get; set; }
        public string AlgoId { get; set; }
        public DateTime AlgoInstanceRunDate { get; set; }
        public string AlgoInstanceStatusValue { get; set; }
        public string AlgoInstanceTypeValue { get; set; }
        public dynamic AlgoMetaDataInformation { get; set; }
        public string AssetPair { get; set; }
        public string AssetPairId { get; set; }
        public string AuthToken { get; set; }
        public string ClientId { get; set; }
        public string HftApiKey { get; set; }
        public string InstanceName { get; set; }
        public bool IsStraight { get; set; }
        public int Margin { get; set; }
        public string OppositeAssetId { get; set; }
        public string TradedAsset { get; set; }
        public string TradedAssetId { get; set; }
        public int Volume { get; set; }
        public string WalletId { get; set; }
    }
}
