using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class AlgoStoreApiLogEntity : TableEntity, IAlgoStoreApiLog
    {
        public string Id => this.RowKey;

        public string DateTime { get; set; }
        public string Level { get; set; }
        public string AppName { get; set; }
        public string Version { get; set; }
        public string Component { get; set; }
        public string Process { get; set; }
        public string Context { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }
        public string Stack { get; set; }
    }
}
