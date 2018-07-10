using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;
using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Enums;

namespace XUnitTestData.Entities.AlgoStore
{
    public class ClientInstanceEntity : TableEntity, IClientInstance
    {
        public string Id => RowKey;
        public string AlgoClientId { get; set; }
        public string AlgoId { get; set; }
        public DateTime AlgoInstanceCreateDate { get; set; }
        public DateTime? AlgoInstanceRunDate { get; set; }
        public string AlgoInstanceStatusValue { get; set; }
        public string AlgoInstanceTypeValue { get; set; }

        public AlgoInstanceStatus AlgoInstanceStatus
        {
            get
            {
                AlgoInstanceStatus type = 0;
                Enum.TryParse(AlgoInstanceStatusValue, out type);
                return type;
            }
            set => AlgoInstanceStatusValue = value.ToString();
        }

        public AlgoInstanceType AlgoInstanceType
        {
            get
            {
                AlgoInstanceType type = 0;
                Enum.TryParse(AlgoInstanceTypeValue, out type);
                return type;
            }
            set => AlgoInstanceTypeValue = value.ToString();
        }

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
        public DateTime? AlgoInstanceStopDate { get; set; }
        public string InstanceId { get; set; }
        public string WalletId { get; set; }
        public string InstanceId { get; set; }
    }
}
