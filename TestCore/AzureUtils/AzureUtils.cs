using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsCore.AzureUtils
{
    public class AzureUtils
    {
        private CloudTableClient cloudTableClient;
        private CloudTable cloudTable;
        private TableQuerySegment tableSegment;


        public AzureUtils(string connectionString)
        {
            cloudTableClient =  CloudStorageAccount.Parse(connectionString).CreateCloudTableClient();
        }

        public AzureUtils GetCloudTable(string tableName)
        {
            cloudTable = cloudTableClient.GetTableReference(tableName);
            return this;
        }

        public AzureUtils GetSearchResult(string rowPropertyName, string expectedPropertyValue)
        {
            tableSegment = cloudTable.ExecuteQuerySegmentedAsync(
                new TableQuery().Where(TableQuery.GenerateFilterCondition(rowPropertyName, QueryComparisons.Equal, expectedPropertyValue)), null).Result;
            return this;
        }

        public EntityProperty GetCellByKnowRowKeyAndKnownCellValue(string cellRowName, string knownRowKeyValue)
        {
            return tableSegment.Results.ToList().Find(r => r.RowKey == knownRowKeyValue)
                .Properties.ToList().Find(cell => cell.Key == cellRowName).Value;
        }
    }
}
