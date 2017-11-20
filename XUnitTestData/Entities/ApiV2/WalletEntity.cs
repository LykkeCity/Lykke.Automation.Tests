using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;
using System.Linq;

namespace XUnitTestData.Entities.ApiV2
{
    public class WalletEntity : TableEntity, IWallet
    {
        public static string GeneratePartitionKey()
        {
            return "Wallet";
        }

        public string Id => RowKey;
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
    }
}
