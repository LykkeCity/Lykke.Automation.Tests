using AzureStorage;
using XUnitTestData.Domains.Assets;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;

namespace XUnitTestData.Repositories.Assets
{
    public class AssetExtendedInfosEntity : TableEntity, IAssetExtendedInfo
    {
        public static string GeneratePartitionKey()
        {
            return "aei";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string AssetId => RowKey;

        public string AssetClass { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string MarketCapitalization { get; set; }
        public string NumberOfCoins { get; set; }
        public int? PopIndex { get; set; }
    }

    public class AssetExtendedInfosRepository : IDictionaryRepository<IAssetExtendedInfo>
    {
        private readonly INoSQLTableStorage<AssetExtendedInfosEntity> _tableStorage;

        public AssetExtendedInfosRepository(INoSQLTableStorage<AssetExtendedInfosEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IAssetExtendedInfo> TryGetAsync(string id)
        {
            var partitionKey = AssetExtendedInfosEntity.GeneratePartitionKey();
            var rowKey = AssetExtendedInfosEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync()
        {
            var partitionKey = AssetExtendedInfosEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);

        }
    }
}
