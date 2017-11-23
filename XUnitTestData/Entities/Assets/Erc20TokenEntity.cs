using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class Erc20TokenEntity : TableEntity, IErc20Token
    {
        public string Id => RowKey;

        public string AssetId { get; set; }

        public string Address { get; set; }

        public string BlockHash { get; set; }

        public int BlockTimestamp { get; set; }

        public string DeployerAddress { get; set; }

        public int? TokenDecimals { get; set; }

        public string TokenName { get; set; }

        public string TokenSymbol { get; set; }

        public string TokenTotalSupply { get; set; }

        public string TransactionHash { get; set; }

        public static string GeneratePartitionKey()
        {
            return "Erc20Token";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }
    }
}