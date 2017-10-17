using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using XUnitTestData.Domains.MatchingEngine;

namespace XUnitTestData.Repositories.MatchingEngine
{
    public class MarketOrderEntity : TableEntity, IMarketOrderEntity
    {
        public static string GeneratePartitionKey()
        {
            return "OrderId";
        }

        public string Id { get; set; }
        public string AssetPairId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Registered { get; set; }
        public string Status { get; set; }
        public bool Straight { get; set; }
        public double Volume { get; set; }
        public DateTime MatchedAt { get; set; }
        public double Price { get; set; }
        public string DustSize { get; set; }
    }

    public class MarketOrdersRepository : IDictionaryRepository<IMarketOrderEntity>
    {
        private readonly INoSQLTableStorage<MarketOrderEntity> _tableStorage;

        public MarketOrdersRepository(INoSQLTableStorage<MarketOrderEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IMarketOrderEntity>> GetAllAsync()
        {
            string partitionKey = MarketOrderEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }

        public async Task<IMarketOrderEntity> TryGetAsync(string id)
        {
            string partitionKey = MarketOrderEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey, id));
        }
    }
}
