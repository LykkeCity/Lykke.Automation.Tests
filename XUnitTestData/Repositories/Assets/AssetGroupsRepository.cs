using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using System.Threading.Tasks;
using AzureStorage;
using System.Linq;

namespace XUnitTestData.Repositories.Assets
{
    public class AssetGroupEntity : TableEntity, IAssetGroup
    {
        public static string GeneratePartitionKey()
        {
            return "AssetGroup";
        }

        public string Id => RowKey;
        public bool ClientsCanCashInViaBankCards { get; set; }
        public bool IsIosDevice { get; set; }
        public string Name { get; set; }
        public bool SwiftDepositEnabled { get; set; }
    }

    public class AssetGroupsRepository : IDictionaryRepository<IAssetGroup>
    {
        private readonly INoSQLTableStorage<AssetGroupEntity> _tableStorage;

        public AssetGroupsRepository(INoSQLTableStorage<AssetGroupEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IAssetGroup>> GetGroupAssetIDsAsync(string groupName)
        {
            string partitionKey = "AssetLink_" + groupName;
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IEnumerable<IAssetGroup>> GetGroupClientIDsAsync(string groupName)
        {
            string partitionKey = "ClientGroupLink_" + groupName;
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IAssetGroup> TryGetAsync(string id)
        {
            string partitionKey = AssetGroupEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey, id);
        }

        public async Task<IEnumerable<IAssetGroup>> GetAllAsync()
        {
            string partitionKey = AssetGroupEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
