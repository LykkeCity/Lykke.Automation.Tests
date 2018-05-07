using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class CSharpAlgoTemplateUserLogEntity : TableEntity, ICSharpAlgoTemplateUserLog
    {
        public string Id => this.RowKey;

        public string DateTime { get; set; }
        public string Message { get ; set ; }
    }
}
