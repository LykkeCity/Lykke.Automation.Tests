using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Repositories.ApiV2
{
    public class OperationDetailsEntity : TableEntity, IOperationDetails
    {
        public string Id => RowKey;
        public string TransactionId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
    }

    public class OperationDetailsRepository : IDictionaryRepository<IOperationDetails>
    {
        private readonly INoSQLTableStorage<OperationDetailsEntity> _tableStorage;

        public OperationDetailsRepository(INoSQLTableStorage<OperationDetailsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IOperationDetails>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync();
        }

        public async Task<IEnumerable<IOperationDetails>> GetAllAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(clientId);
        }

        public async Task<IOperationDetails> TryGetAsync(string clientId, string id)
        {
            return await _tableStorage.GetDataAsync(clientId, id);
        }

        public async Task<IOperationDetails> TryGetByTransactionId(string clientId, string transactionId)
        {
            return (await _tableStorage.GetDataAsync(d => d.PartitionKey == clientId && d.TransactionId == transactionId)).FirstOrDefault();
        }
    }
}
