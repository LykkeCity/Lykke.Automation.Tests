using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class AssetCategoryEntity : TableEntity, IAssetCategory
    {
        public static string Partition = "AssetCategory";

        public string Id => RowKey;
        public string Name { get; set; }
        public string IosIconUrl { get; set; }
        public string AndroidIconUrl { get; set; }
        public int SortOrder { get; set; }

        public static AssetCategoryEntity Create(IAssetCategory assetCategory)
        {
            return new AssetCategoryEntity
            {
                RowKey = assetCategory.Id,
                Name = assetCategory.Name,
                PartitionKey = Partition,
                IosIconUrl = assetCategory.IosIconUrl,
                AndroidIconUrl = assetCategory.AndroidIconUrl,
                SortOrder = assetCategory.SortOrder
            };
        }
    }
}
