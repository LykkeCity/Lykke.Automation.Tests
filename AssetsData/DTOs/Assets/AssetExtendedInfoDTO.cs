using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class AssetExtendedInfoDTO : BaseDTO
    {
        public string AssetClass { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Id { get; set; }
        public string MarketCapitalization { get; set; }
        public string NumberOfCoins { get; set; }
        public int? PopIndex { get; set; }
    }
}
