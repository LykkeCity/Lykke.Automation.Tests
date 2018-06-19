using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using XUnitTestData.Domains.AlgoStore;
using XUnitTestData.Enums;

namespace XUnitTestData.Entities.AlgoStore
{
    public class AlgoEntity : TableEntity, IAlgo
    {
        public string AlgoId { get => RowKey; set { } }
        public string ClientId { get => PartitionKey; set { } }
        public string Name { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public string AlgoVisibilityValue { get; set; }
        public string AlgoMetaDataInformationJSON { get; set; }
        public string AlgoVisibility { get; set; }
        public string Author { get; set; }
        public string Id { get => RowKey; set { } }
    }
}
