using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class InstanceDataDTO
    {
        public string InstanceId { get; set; }
        public string AlgoId { get; set; }
        public string HftApiKey { get; set; }
        public string AssetPair { get; set; }
        public string TradedAsset { get; set; }
        public int Volume { get; set; }
        public int Margin { get; set; }
    }
}
