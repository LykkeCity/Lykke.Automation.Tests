using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class PublicsAlgosTableEntity : TableEntity, IPublicAlgosTable
    {
        public string Id => this.RowKey;
    }
}
