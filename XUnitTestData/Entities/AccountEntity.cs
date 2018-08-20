using Common;
using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains;

namespace XUnitTestData.Entities
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
        public string AssetId { get; set; }
        public double Balance { get; set; }
        public double Reserved { get; set; }
    }
}
