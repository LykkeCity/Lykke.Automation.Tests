using ApiV2Data.DTOs;
using Newtonsoft.Json;
using XUnitTestCommon;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class InstanceDataDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InstanceId { get; set; }
        public string AlgoId { get; set; }
        public string AlgoClientId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WalletId { get; set; }

        public string InstanceName { get; set; }
        public AlgoInstanceType AlgoInstanceType { get; set; }
        public AlgoMetaDataInformation AlgoMetaDataInformation { get; set; }
        public AlgoInstanceStatus AlgoInstanceStatusValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FakeTradingTradingAssetBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? FakeTradingAssetTwoBalance { get; set; }

        public InstanceDataDTO() { }

        public InstanceDataDTO(AlgoDataDTO algoData, WalletDTO wallet, AlgoInstanceType algoInstanceType, AlgoMetaDataInformation algoMetaDataInformation, string instanceName = null)
        {
            AlgoId = algoData.Id;
            AlgoClientId = algoData.ClientId;
            InstanceName = instanceName ?? $"{algoInstanceType} {Helpers.GetTimestampIso8601()}{GlobalConstants.AutoTest}_IntanceName";
            AlgoInstanceType = algoInstanceType;
            AlgoMetaDataInformation = algoMetaDataInformation;

            if (algoInstanceType == AlgoInstanceType.Live)
            {
                WalletId = wallet.Id;
                FakeTradingTradingAssetBalance = null;
                FakeTradingAssetTwoBalance = null;
            }
            else
            {
                WalletId = null;
                FakeTradingTradingAssetBalance = 8192;
                FakeTradingAssetTwoBalance = 2.048;
            }
        }
    }
}
