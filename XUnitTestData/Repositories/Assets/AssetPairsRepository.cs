using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Repositories.Assets
{
    public class AssetPairEntity : TableEntity, IAssetPair
    {
        public static string GeneratePartitionKey()
        {
            return "AssetPair";
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string BaseAssetId { get; set; }
        public string QuotingAssetId { get; set; }
        public int Accuracy { get; set; }
        public int InvertedAccuracy { get; set; }
        public string Source { get; set; }
        public string Source2 { get; set; }
        public bool IsDisabled { get; set; }
    }

    public class AssetPairsRepository : IDictionaryRepository<IAssetPair>
    {
        private readonly INoSQLTableStorage<AssetPairEntity> _tableStorage;

        public AssetPairsRepository(INoSQLTableStorage<AssetPairEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IAssetPair>> GetAllAsync()
        {
            var partitionKey = AssetPairEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }
    }
}
