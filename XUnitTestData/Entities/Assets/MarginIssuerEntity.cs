using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class MarginIssuerEntity : TableEntity, IMarginIssuer
    {
        public static string GeneratePartitionKey()
        {
            return "MarginIssuer";
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
