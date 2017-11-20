using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class MarginAssetPairsEntity : TableEntity, IMarginAssetPairs
    {
        public static string GeneratePartitionKey()
        {
            return "MarginAssetPair";
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string BaseAssetId { get; set; }
        public string QuotingAssetId { get; set; }
        public int Accuracy { get; set; }
        public int InvertedAccuracy { get; set; }
    }
}
