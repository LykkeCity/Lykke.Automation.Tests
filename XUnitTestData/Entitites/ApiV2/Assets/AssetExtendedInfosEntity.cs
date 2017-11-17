using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entitites.ApiV2.Assets
{
    public class AssetExtendedInfosEntity : TableEntity, IAssetExtendedInfo
    {
        public static string GeneratePartitionKey()
        {
            return "aei";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string AssetId => RowKey;

        public string AssetClass { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string MarketCapitalization { get; set; }
        public string NumberOfCoins { get; set; }
        public int? PopIndex { get; set; }
    }
}
