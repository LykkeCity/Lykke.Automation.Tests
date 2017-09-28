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
    public class AssetDescriptionEntity : TableEntity, IAssetDescription
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
        public string Description { get; set; }
        public string IssuerName { get; set; }
        public string NumberOfCoins { get; set; }
        public string MarketCapitalization { get; set; }
        public int PopIndex { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string FullName { get; set; }


        public static AssetDescriptionEntity Create(IAssetDescription src)
        {
            return new AssetDescriptionEntity
            {
                PartitionKey = GeneratePartitionKey(),
                PopIndex = src.PopIndex,
                MarketCapitalization = src.MarketCapitalization,
                AssetClass = src.AssetClass,
                NumberOfCoins = src.NumberOfCoins,
                Description = src.Description,
                AssetDescriptionUrl = src.AssetDescriptionUrl,
                FullName = src.FullName
            };
        }
    }

    public class AssetDescriptionRepository : IAssetDescriptionRepository, IDictionaryRepository<IAssetDescription>
    {
        private readonly INoSQLTableStorage<AssetDescriptionEntity> _tableStorage;

        public AssetDescriptionRepository(INoSQLTableStorage<AssetDescriptionEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(IAssetDescription src)
        {
            var newEntity = AssetDescriptionEntity.Create(src);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task<IAssetDescription> GetAssetExtendedInfoAsync(string id)
        {
            var partitionKey = AssetDescriptionEntity.GeneratePartitionKey();
            var rowKey = AssetDescriptionEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IAssetDescription>> GetAllAsync()
        {
            var partitionKey = AssetDescriptionEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);

        }
    }
}
