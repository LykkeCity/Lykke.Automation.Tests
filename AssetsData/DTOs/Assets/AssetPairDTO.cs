using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class AssetPairDTO : BaseDTO
    {
        public int Accuracy { get; set; }
        public string BaseAssetId { get; set; }
        public string Id { get; set; }
        public int InvertedAccuracy { get; set; }
        public bool IsDisabled { get; set; }
        public string Name { get; set; }
        public string QuotingAssetId { get; set; }
        public string Source { get; set; }
        public string Source2 { get; set; }
    }
}
