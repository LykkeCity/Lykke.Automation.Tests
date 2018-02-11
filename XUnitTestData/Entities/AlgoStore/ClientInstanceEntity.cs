using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains.AlgoStore;
using Microsoft.WindowsAzure.Storage.Table;

namespace XUnitTestData.Entities.AlgoStore
{
    public class ClientInstanceEntity : TableEntity, IClientInstance
    {
        public string AssetPair { get; set;}
        public string HftApiKey { get; set;}
        public int Margin { get; set; }
        public string TradedAsset { get; set; }
        public int Volume { get; set; }

        public string Id => this.RowKey;
    }
}
