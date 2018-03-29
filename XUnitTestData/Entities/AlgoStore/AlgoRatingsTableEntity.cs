using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class AlgoRatingsTableEntity : TableEntity, IAlgoRatingsTable
    {
 
        public string Id => this.RowKey;

        public int Rating { get ; set ; }
    }
}
