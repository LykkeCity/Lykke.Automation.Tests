using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using XUnitTestData.Domains.Assets;
using XUnitTestData.Entities.Assets;

namespace XUnitTestData.Repositories.ApiV2
{
    public class AssetAttributesRepository : GenericRepository<AssetAttributesEntity, IAssetAttributes>
    {
        public AssetAttributesRepository(INoSQLTableStorage<AssetAttributesEntity> tableStorage, string partitionKey = null) : base(tableStorage, partitionKey)
        {
        }

        public new async Task<IEnumerable<IAssetAttributes>> GetAllAsync()
        {
            var entities = await TableStorage.GetDataAsync();

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

        public Task EditAsync(string assetId, IAssetAttributesKeyValue keyValue)
        {
            return TableStorage.MergeAsync(assetId, keyValue.Key,
                entity =>
                {
                    entity.Value = keyValue.Value;
                    return entity;
                });
        }

        public async Task<IAssetAttributesKeyValue> GetAsync(string assetId, string key)
        {
            var entity = await TableStorage.GetDataAsync(assetId, key);
            return new KeyValue { Key = entity.Key, Value = entity.Value };
        }

        public new async Task<IAssetAttributesKeyValue> TryGetAsync(string assetId, string key)
        {
            var entity = await TableStorage.GetDataAsync(assetId, key);
            if (entity == null)
                return null;
            return new KeyValue { Key = entity.Key, Value = entity.Value };
        }

        public new async Task<IAssetAttributesKeyValue[]> GetAllAsync(string assetId)
        {
            var entities = await TableStorage.GetDataAsync(assetId);
            return entities.Select(x => new KeyValue { Key = x.Key, Value = x.Value }).ToArray();
        }
    }
}