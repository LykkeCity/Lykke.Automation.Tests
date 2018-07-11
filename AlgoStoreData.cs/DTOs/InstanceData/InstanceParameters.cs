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
        public int FunctionCapacity { get; set; }
        public List<FunctionType> InstanceFunctions { get; set; }

        public InstanceParameters() { }

        public InstanceParameters(string assetPair, string tradedAsset, double instanceTradeVolume, 
                                  CandleTimeInterval instanceCandleInterval, CandleTimeInterval functionCandleInterval,
                                  CandleOperationMode functionCandleOperationMode, int functionCapacity, List<FunctionType> instanceFunctions)
        {
            AssetPair = assetPair;
            TradedAsset = tradedAsset;
            InstanceTradeVolume = instanceTradeVolume;
            InstanceCandleInterval = instanceCandleInterval;
            FunctionCandleInterval = functionCandleInterval;
            FunctionCandleOperationMode = functionCandleOperationMode;
            FunctionCapacity = functionCapacity;
            InstanceFunctions = InstanceFunctions;
        }
    }
}
