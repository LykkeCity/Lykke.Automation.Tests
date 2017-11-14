using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Repositories.ApiV2
{
    public class TradersEntity : TableEntity, ITrader
    {

        public string Id => RowKey;
        public string PrimeryPartitionKey { get; set; }
        public string PrimeryRowKey { get; set; }
    }

    public class TradersRepository : IDictionaryRepository<ITrader>
    {
        private readonly INoSQLTableStorage<TradersEntity> _tableStorage;

        public TradersRepository(INoSQLTableStorage<TradersEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<ITrader>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync();
        }

        public async Task<IEnumerable<ITrader>> GetAllAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(t => t.PrimeryRowKey == clientId);
        }
    }
}
