using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using System.Threading.Tasks;
using System.Linq;

namespace XUnitTestData.Repositories
{
    public class GenericRepository<TEntity, TDictionaryItem> : IDictionaryRepository<TDictionaryItem> 
        where TEntity : TableEntity, TDictionaryItem, new()
        where TDictionaryItem : IDictionaryItem
    {
        protected readonly INoSQLTableStorage<TEntity> TableStorage;
        private readonly string _partitionKey;

        public GenericRepository(INoSQLTableStorage<TEntity> tableStorage, string partitionKey = null)
        {
            TableStorage = tableStorage;
            _partitionKey = partitionKey;
        }

        public async Task<IEnumerable<TDictionaryItem>> GetAllAsync()
        {
            return await TableStorage.GetDataAsync(_partitionKey);
        }

        public async Task<IEnumerable<TDictionaryItem>> GetAllAsync(string partitionKey)
        {
            return await TableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IEnumerable<TDictionaryItem>> GetAllAsync(Func<TDictionaryItem, bool> lambda)
        {
            return await TableStorage.GetDataAsync(lambda);
        }

        public async Task<TDictionaryItem> TryGetAsync(string id)
        {
            return await TableStorage.GetDataAsync(_partitionKey, id);
        }

        public async Task<TDictionaryItem> TryGetAsync(string partitionKey, string id)
        {
            return await TableStorage.GetDataAsync(partitionKey, id);
        }

        public async Task<TDictionaryItem> TryGetAsync(Func<TDictionaryItem, bool> lambda)
        {
            return (await TableStorage.GetDataAsync(lambda)).FirstOrDefault();
        }
    }
}
