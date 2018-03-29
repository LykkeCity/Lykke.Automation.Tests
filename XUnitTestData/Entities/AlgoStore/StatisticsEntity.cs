using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class StatisticsEntity : TableEntity, IStatisticss
    {
        public string Id => this.RowKey;

        public int BuildImageId { get ; set ; }
        public long ImageId { get ; set; }
        public bool IsBuy { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
