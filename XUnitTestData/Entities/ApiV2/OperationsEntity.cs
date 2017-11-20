using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using System.Linq;

namespace XUnitTestData.Entities.ApiV2
{
    public class OperationsEntity : TableEntity, IOperations
    {
        public static string GeneratePartitionKey()
        {
            return "Operations";
        }

        public string Id => RowKey;
        public string PrimaryPartitionKey { get; set; }
        public string PrimaryRowKey { get; set; }
        public DateTime Created { get; set; }
        public Guid ClientId { get; set; }
        public string StatusString { get; set; }
        public string TypeString { get; set; }
        public string Context { get; set; }

    }
}
