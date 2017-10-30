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
    public class AssetIssuersEntity : TableEntity, IAssetIssuers
    {
        public static string GeneratePartitionKey()
        {
            return "Issuer";
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string IconUrl { get; set; }

    }

    public class AssetIssuersRepository : IDictionaryRepository<IAssetIssuers>
    {
        private readonly INoSQLTableStorage<AssetIssuersEntity> _tableStorage;

        public AssetIssuersRepository(INoSQLTableStorage<AssetIssuersEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IAssetIssuers>> GetAllAsync()
        {
            string partitionKey = AssetIssuersEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
