using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Repositories.Assets
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

    public class MarginAssetPairsRepository : IDictionaryRepository<IMarginAssetPairs>
    {
        private readonly INoSQLTableStorage<MarginAssetPairsEntity> _tableStorage;

        public MarginAssetPairsRepository(INoSQLTableStorage<MarginAssetPairsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public async Task<IEnumerable<IMarginAssetPairs>> GetAllAsync()
        {
            var partitionKey = MarginAssetPairsEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }
    }
}
