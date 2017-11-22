using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Entities.ApiV2
{
    public class TradersEntity : TableEntity, ITrader
    {

        public string Id => RowKey;
        public string PrimaryPartitionKey { get; set; }
        public string PrimaryRowKey { get; set; }
        public DateTime Registered { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NotificationsId { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public bool IsReviewAccount { get; set; }
        public bool IsTrusted { get; set; }
        public int? Pin { get; set; }
    }
}
