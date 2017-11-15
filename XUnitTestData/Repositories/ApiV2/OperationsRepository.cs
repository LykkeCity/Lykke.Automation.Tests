using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using System.Linq;

namespace XUnitTestData.Repositories.ApiV2
{
    public class OperationsEntity : TableEntity, IOperations
    {
        public static string GeneratePartitionKey()
        {
            return "Operations";
        }

        public string Id => RowKey;
        public string PrimaryPartitionKey { get; set; }
        public string PrimaryRowKey { get; set; }
        public DateTime Created { get; set; }
        public Guid ClientId { get; set; }
        public string StatusString { get; set; }
        public string TypeString { get; set; }
        public string Context { get; set; }

    }

    public class OperationsRepository : IDictionaryRepository<IOperations>
    {
        private readonly INoSQLTableStorage<OperationsEntity> _tableStorage;

        public OperationsRepository(INoSQLTableStorage<OperationsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IOperations> TryGetAsync(string partitionKey, string id)
        {
            return await _tableStorage.GetDataAsync(partitionKey, id);
        }

        public async Task<IOperations> TryGetAsync(string id)
        {
            string partitionKey = OperationsEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey, id);
        }

        public async Task<IEnumerable<IOperations>> GetAllAsync(string partitionKey, string clientId)
        {
            Guid clientIdGuid = Guid.Parse(clientId);

            return await _tableStorage.GetDataAsync(o => o.PartitionKey == partitionKey && o.ClientId == clientIdGuid);
        }

        public async Task<IEnumerable<IOperations>> GetAllAsync(string clientId)
        {
            string partitionKey = OperationsEntity.GeneratePartitionKey();
            Guid clientIdGuid = Guid.Parse(clientId);

            return await _tableStorage.GetDataAsync(o => o.PartitionKey == partitionKey && o.ClientId == clientIdGuid);
        }

        public async Task<IEnumerable<IOperations>> GetAllAsync()
        {
            string partitionKey = OperationsEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
