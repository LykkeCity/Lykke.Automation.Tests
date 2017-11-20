using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Collections.Generic;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;
using XUnitTestData.Domains.BlueApi;

namespace XUnitTestData.Entities.BlueApi
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
}
