using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class MetaDataEntity : TableEntity , IMetaData
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Id => this.RowKey;
    }
}
