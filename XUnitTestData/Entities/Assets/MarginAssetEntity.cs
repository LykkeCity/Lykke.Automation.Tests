using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class MarginAssetEntity : TableEntity, IMarginAsset
    {
        public static string GeneratePartitionKey()
        {
            return "MarginAsset";
        }

        public string Id => RowKey;
        public int Accuracy { get; set; }
        public double DustLimit { get; set; }
        public string IdIssuer { get; set; }
        public double Multiplier { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
