using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entitites.ApiV2.Assets
{
    public class AssetSettingsEntity : TableEntity, IAssetSettings
    {
        public static string GeneratePartitionKey()
        {
            return "Asset";
        }

        public string Id
        {
            get { return RowKey; }
            set { this.RowKey = value; }
        }
        public string Asset
        {
            get { return RowKey; }
            set { this.RowKey = value; }
        }
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
}
