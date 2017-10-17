using Microsoft.WindowsAzure.Storage.Table;
using System;
using XUnitTestData.Domains;
using XUnitTestData.Domains.MatchingEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Repositories.MatchingEngine
{
    public class LimitOrderEntity : TableEntity, ILimitOrderEntity
    {
        public string AssetPairId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
        public string MatchingId { get; set; }
        public double Price { get; set; }
        public double RemainingVolume { get; set; }
        public string Status { get; set; }
        public bool Straight { get; set; }
        public double Volume { get; set; }
    }

    public class LimitOrderRepository : IDictionaryRepository<ILimitOrderEntity>
    {
        private readonly INoSQLTableStorage<LimitOrderEntity> _tableStorage;

        public LimitOrderRepository(INoSQLTableStorage<LimitOrderEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<ILimitOrderEntity>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync();
        }

        public async Task<IEnumerable<ILimitOrderEntity>> GetAllAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(clientId);
        }

        public async Task<ILimitOrderEntity> TryGetAsync(string clientId, string orderId)
        {
            return await _tableStorage.GetDataAsync(clientId, orderId);
        }
    }
}
