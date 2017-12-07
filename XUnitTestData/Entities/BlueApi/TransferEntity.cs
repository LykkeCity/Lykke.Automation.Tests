using Microsoft.WindowsAzure.Storage.Table;
using System;
using XUnitTestData.Domains.BlueApi;

namespace XUnitTestData.Entities.BlueApi
{
    public class TransferEntity : TableEntity, ITransfer
    {
        public string Id => RowKey;

        public DateTime DateTime { get; set; }
        public bool IsHidden { get; set; }
        public string AssetId { get; set; }
        public double Amount { get; set; }
        public string Multisig { get; set; }
        public string TransactionId { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public bool IsSettled { get; set; }
        public string StateField { get; set; }
        public string ClientId { get; set; }

    }
}
