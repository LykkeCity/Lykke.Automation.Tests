using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestData.Domains.Assets
{
    public interface IAssetExtendedInfo : IDictionaryItem
    {
        string AssetClass { get; set; }
        string AssetDescriptionUrl { get; set; }
        string Description { get; set; }
        string FullName { get; set; }
        string MarketCapitalization { get; set; }
        string NumberOfCoins { get; set; }
        int? PopIndex { get; set; }
    }
}
