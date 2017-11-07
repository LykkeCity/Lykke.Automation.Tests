using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;
using System.Linq;

namespace XUnitTestData.Repositories.ApiV2
{
    public class WalletEntity : TableEntity, IWallet
    {
        public static string GeneratePartitionKey()
        {
            return "Wallet";
        }

        public string Id => RowKey;
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
    }

    public class WalletRepository : IDictionaryRepository<IWallet>
    {
        private readonly INoSQLTableStorage<WalletEntity> _tableStorage;

        public WalletRepository(INoSQLTableStorage<WalletEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IWallet> TryGetAsync(string id)
        {
            string partitionKey = WalletEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey, id);
        }

        public async Task<IEnumerable<IWallet>> GetAllAsync()
        {
            string partitionKey = WalletEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IEnumerable<IWallet>> GetAllAsync(string clientId)
        {
            string partitionKey = WalletEntity.GeneratePartitionKey();

            return _tableStorage.Where(e => e.ClientId == clientId).ToList();
        }
    }
}
