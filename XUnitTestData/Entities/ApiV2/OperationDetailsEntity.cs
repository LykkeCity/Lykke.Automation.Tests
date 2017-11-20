using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Entities.ApiV2
{
    public class OperationDetailsEntity : TableEntity, IOperationDetails
    {
        public string Id => RowKey;
        public string TransactionId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
    }
}
