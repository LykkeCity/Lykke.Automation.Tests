using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using XUnitTestData.Domains.BlueApi;

namespace XUnitTestData.Repositories.BlueApi
{
    public class TwitterEntity : TableEntity, ITwitterEntity
    {
        public static string GeneratePartitionKey()
        {
            return "AccountId";
        }

        public string Id => RowKey;
        public bool IsExtendedSearch { get; set; }
        public string AccountEmail { get; set; }
        public string SearchQuery { get; set; }
        public int MaxResult { get; set; }
        public DateTime UntilDate { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }


    class TwitterRepository : IDictionaryRepository<ITwitterEntity>
    {

        private readonly INoSQLTableStorage<TwitterEntity> _tableStorage;

        public TwitterRepository(INoSQLTableStorage<TwitterEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<ITwitterEntity> TryGetAsync(string id)
        {
            var partitionKey = TwitterEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey, id));
        }

        public async Task<IEnumerable<ITwitterEntity>> GetAllAsync()
        {
            var partitionKey = TwitterEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }
    }
}
