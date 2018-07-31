using Newtonsoft.Json;
using System.Collections.Generic;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class InstanceParameters
    {
        public string AssetPair { get; set; }
        public string TradedAsset { get; set; }
        public double InstanceTradeVolume { get; set; }
        public CandleTimeInterval InstanceCandleInterval { get; set; }
        public CandleTimeInterval FunctionCandleInterval { get; set; }
        public CandleOperationMode FunctionCandleOperationMode { get; set; }
        public int FunctionPeriod { get; set; }
        public List<FunctionType> InstanceFunctions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool UseInvalidAlgoId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool WithoutMetadata { get; set; }

        public InstanceParameters() { }

        public InstanceParameters(string assetPair, string tradedAsset, double instanceTradeVolume, 
                                  CandleTimeInterval instanceCandleInterval, CandleTimeInterval functionCandleInterval,
                                  CandleOperationMode functionCandleOperationMode, int functionPeriod, List<FunctionType> instanceFunctions,
                                  bool userInvalidAlgoId, bool withoutMedatada)
        {
            AssetPair = assetPair;
            TradedAsset = tradedAsset;
            InstanceTradeVolume = instanceTradeVolume;
            InstanceCandleInterval = instanceCandleInterval;
            FunctionCandleInterval = functionCandleInterval;
            FunctionCandleOperationMode = functionCandleOperationMode;
            FunctionPeriod = functionPeriod;
            InstanceFunctions = InstanceFunctions;
            UseInvalidAlgoId = userInvalidAlgoId;
            WithoutMetadata = withoutMedatada;
        }
    }
}
