using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Repositories.Assets
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

    public class MarginAssetRepository : IDictionaryRepository<IMarginAsset>
    {
        private readonly INoSQLTableStorage<MarginAssetEntity> _tableStorage;

        public MarginAssetRepository(INoSQLTableStorage<MarginAssetEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IMarginAsset>> GetAllAsync()
        {
            var partitionKey = MarginAssetEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }
    }
}
