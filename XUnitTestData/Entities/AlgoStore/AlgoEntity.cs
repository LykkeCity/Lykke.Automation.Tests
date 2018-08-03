using Microsoft.WindowsAzure.Storage.Table;
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
        public DateTime? DatePublished { get; set; }
        public string Description { get; set; }
        public string AlgoVisibilityValue { get; set; }

        public AlgoVisibility AlgoVisibility
        {
            get
            {
                Enum.TryParse(AlgoVisibilityValue, out AlgoVisibility type);
                return type;
            }
        }

        public string AlgoMetaDataInformationJSON { get; set; }
        public string Author { get; set; }
        public string Id { get => RowKey; set { } }
    }
}
