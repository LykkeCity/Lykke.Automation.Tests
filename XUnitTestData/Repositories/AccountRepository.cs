using Common;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using System.Threading.Tasks;
using AzureStorage;
using System.Linq;

namespace XUnitTestData.Repositories
{
    public class AccountEntity : TableEntity, IAccount
    {
        public static string GeneratePartitionKey()
        {
            return "ClientBalance";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string Balances { get; set; }

        public BalanceDTO[] BalancesParsed
        {
            get => Balances.DeserializeJson<BalanceDTO[]>();
            set => value?.ToJson();
        }





    }

    public class BalanceDTO
    {
        public string Asset { get; set; }
        public double Balance { get; set; }
        public double Reserved { get; set; }
    }

    public class AccountRepository : IDictionaryRepository<IAccount>
    {
        private readonly INoSQLTableStorage<AccountEntity> _tableStorage;

        public AccountRepository(INoSQLTableStorage<AccountEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IAccount>> GetAllAsync()
        {
            var partitionKey = AccountEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }

        public async Task<IAccount> TryGetAsync(string clientId)
        {
            var partitionKey = AccountEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey, clientId));
        }
    }
}
