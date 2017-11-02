using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Repositories.Assets
{
    public class MarginIssuerEntity : TableEntity, IMarginIssuer
    {
        public static string GeneratePartitionKey()
        {
            return "MarginIssuer";
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class MarginIssuerRepository : IDictionaryRepository<IMarginIssuer>
    {
        private readonly INoSQLTableStorage<MarginIssuerEntity> _tableStorage;

        public MarginIssuerRepository(INoSQLTableStorage<MarginIssuerEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IMarginIssuer>> GetAllAsync()
        {
            string partitionKey = MarginIssuerEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
