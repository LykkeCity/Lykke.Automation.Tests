using Microsoft.WindowsAzure.Storage.Table;
using System;
using XUnitTestData.Domains.BlueApi;

namespace XUnitTestData.Entities.BlueApi
{
    public class TwitterEntity : TableEntity, ITwitterEntity
    {
        public static string GeneratePartitionKey()
        {
            return "AccountId";
        }

        public string Id => RowKey;
        public bool IsExtendedSearch { get; set; }
        public string AccountEmail { get; set; }
        public string SearchQuery { get; set; }
        public int MaxResult { get; set; }
        public DateTime UntilDate { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
