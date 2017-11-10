using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Collections.Generic;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;
using XUnitTestData.Domains.BlueApi;

namespace XUnitTestData.Repositories.BlueApi
{
    public class PledgeEntity : TableEntity, IPledgeEntity
    {
        public static string GeneratePartitionKey()
        {
            return "Pledge";
        }

        public string Id => RowKey;
        public string ClientId { get; set; }
        public int CO2Footprint { get; set; }
        public int ClimatePositiveValue { get; set; }
    }

    public class PledgesRepository : IDictionaryRepository<IPledgeEntity>
    {
        private readonly INoSQLTableStorage<PledgeEntity> _tableStorage;

        public PledgesRepository(INoSQLTableStorage<PledgeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IPledgeEntity> TryGetAsync(string id)
        {
            var partitionKey = PledgeEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey, id));
        }

        public async Task<IEnumerable<IPledgeEntity>> GetAllAsync()
        {
            var partitionKey = PledgeEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }

        public async Task<IPledgeEntity> GetPledgeAsync(string clientId)
        {
            var partitionKey = PledgeEntity.GeneratePartitionKey();

            return _tableStorage.FirstOrDefault(p => p.PartitionKey == partitionKey && p.ClientId == clientId);
        }
    }
}
