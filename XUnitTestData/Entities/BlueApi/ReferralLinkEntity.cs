using System;
using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.BlueApi;

namespace XUnitTestData.Entities.BlueApi
{
    public class ReferralLinkEntity : TableEntity, IReferralLink
    {
        public static string GeneratePartitionKey()
        {
            return "Pledge";
        }

        public string Id => RowKey;
        public string Url { get; set; }
        public string SenderClientId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Asset { get; set; }
        public double Amount { get; set; }
        public string SenderOffchainTransferId { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
    }
}