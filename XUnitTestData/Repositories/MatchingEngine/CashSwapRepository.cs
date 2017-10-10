using Common;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using System.Threading.Tasks;
using AzureStorage;
using System.Linq;
using XUnitTestData.Domains.MatchingEngine;

namespace XUnitTestData.Repositories.MatchingEngine
{
    public class CashSwapEntity : TableEntity, ICashSwap
    {
        public string Id => ExternalId;
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public DateTime DateTime { get; set; }
        public string ExternalId { get; set; }
        public string FromClientId { get; set; }
        public string ToClientId { get; set; }

        public double Amount1 { get; set; }
        public double Amount2 { get; set; }
        public double AssetId1 { get; set; }
        public double AssetId2 { get; set; }
        public double ClientId1 { get; set; }
        public double ClientId2 { get; set; }

    }



    public class CashSwapRepository : IDictionaryRepository<ICashSwap>
    {
        private readonly INoSQLTableStorage<CashSwapEntity> _tableStorage;

        public CashSwapRepository(INoSQLTableStorage<CashSwapEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<ICashSwap>> GetAllAsync()
        {
            return (await _tableStorage.GetDataAsync());
        }

        public async Task<ICashSwap> TryGetAsync(string externalId)
        {
            return (_tableStorage.Where(a => a.ExternalId == externalId).FirstOrDefault());
        }
    }
}
