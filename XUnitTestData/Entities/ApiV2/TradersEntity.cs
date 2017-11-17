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
        public string PrimeryPartitionKey { get; set; }
        public string PrimeryRowKey { get; set; }
    }
}
