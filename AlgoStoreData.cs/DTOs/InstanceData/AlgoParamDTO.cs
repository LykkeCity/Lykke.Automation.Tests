using System;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class AlgoParamsDTO
    {
        public DateTime StartFrom { get; set; }
        public DateTime EndOn { get; set; }
        public string AssetPair { get; set; }
        public string TradedAsset { get; set; }
        public double Volume { get; set; }
        public CandleTimeInterval CandleInterval { get; set; }

        public AlgoParamsDTO() { }

        public AlgoParamsDTO(DateTime startFrom, DateTime endOn, string assetPair, string tradedAsset, double volume, CandleTimeInterval candleTimeInterval)
        {
            StartFrom = startFrom;
            EndOn = endOn;
            AssetPair = assetPair;
            TradedAsset = tradedAsset;
            Volume = volume;
            CandleInterval = candleTimeInterval;
        }

        public AlgoParamsDTO(int startDaysOffset, int endDaysOffset, string assetPair, string tradedAsset, double volume, CandleTimeInterval candleTimeInterval)
        {
            var dateTimeNow = DateTime.Now;
            StartFrom = dateTimeNow.AddDays(startDaysOffset);
            EndOn = dateTimeNow.AddDays(endDaysOffset);
            AssetPair = assetPair;
            TradedAsset = tradedAsset;
            Volume = volume;
            CandleInterval = candleTimeInterval;
        }
    }
}
