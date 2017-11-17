using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class AssetGroupEntity : TableEntity, IAssetGroup
    {
        public static string GeneratePartitionKey()
        {
            return "AssetGroup";
        }

        public string Id => RowKey;
        public bool ClientsCanCashInViaBankCards { get; set; }
        public bool IsIosDevice { get; set; }
        public string Name { get; set; }
        public bool SwiftDepositEnabled { get; set; }
    }
}
