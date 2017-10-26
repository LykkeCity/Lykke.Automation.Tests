using AzureStorage;
using XUnitTestData.Domains.Assets;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;

namespace XUnitTestData.Repositories.Assets
{
    public class AssetAttributesEntity : TableEntity, IAssetAttributes
    {
        public string Id => PartitionKey;
        public string AssetId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }
        public string Key
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        public string Value { get; set; }
        public IEnumerable<IAssetAttributesKeyValue> Attributes { get; set; }

        public static AssetAttributesEntity Create(string assetId, IAssetAttributesKeyValue keyValue)
        {
            return new AssetAttributesEntity
            {
                RowKey = keyValue.Key,
                PartitionKey = assetId,
                Value = keyValue.Value
            };
        }
    }

    public class AssetAttributesRepository : IAssetAttributesRepository, IDictionaryRepository<IAssetAttributes>
    {
        private readonly INoSQLTableStorage<AssetAttributesEntity> _tableStorage;

        public AssetAttributesRepository(INoSQLTableStorage<AssetAttributesEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task AddAsync(string assetId, IAssetAttributesKeyValue keyValue)
        {
            var entity = AssetAttributesEntity.Create(assetId, keyValue);
            return _tableStorage.InsertAsync(entity);
        }

        public Task EditAsync(string assetId, IAssetAttributesKeyValue keyValue)
        {
            return _tableStorage.MergeAsync(assetId, keyValue.Key,
                entity =>
                {
                    entity.Value = keyValue.Value;
                    return entity;
                });
        }

        public Task RemoveAsync(string assetId, string key)
        {
            return _tableStorage.DeleteAsync(assetId, key);
        }

        public async Task<IAssetAttributesKeyValue> GetAsync(string assetId, string key)
        {
            var entity = await _tableStorage.GetDataAsync(assetId, key);
            return new KeyValue { Key = entity.Key, Value = entity.Value };
        }

        public async Task<IAssetAttributesKeyValue> TryGetAsync(string assetId, string key)
        {
            var entity = await _tableStorage.GetDataAsync(assetId, key);
            if (entity == null)
                return null;
            return new KeyValue { Key = entity.Key, Value = entity.Value };
        }

        public async Task<IAssetAttributesKeyValue[]> GetAllAsync(string assetId)
        {
            var entities = await _tableStorage.GetDataAsync(assetId);
            return entities.Select(x => new KeyValue { Key = x.Key, Value = x.Value }).ToArray();
        }

        public async Task<IEnumerable<IAssetAttributes>> GetAllAsync()
        {
            var entities = await _tableStorage.GetDataAsync();

            return entities
                .GroupBy(x => x.AssetId)
                .Select(g => new AssetAttributesEntity
                {
                    AssetId = g.Key,
                    Attributes = g.Select(x => new KeyValue
                    {
                        Key = x.Key,
                        Value = x.Value
                    })
                }).ToList();
        }
    }
}
