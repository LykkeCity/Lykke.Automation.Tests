using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class AssetSettingsDTO
    {
        public string Id => Asset;
        public string Asset { get; set; }
        public string CashinCoef { get; set; }
        public string ChangeWallet { get; set; }
        public string Dust { get; set; }
        public string HotWallet { get; set; }
        public string MaxBalance { get; set; }
        public int MaxOutputsCount { get; set; }
        public int MaxOutputsCountInTx { get; set; }
        public string MinBalance { get; set; }
        public int MinOutputsCount { get; set; }
        public string OutputSize { get; set; }
        public int PrivateIncrement { get; set; }
    }

    public class AllAssetSettingsDTO
    {
        public List<AssetSettingsDTO> Items { get; set; }
    }
}
