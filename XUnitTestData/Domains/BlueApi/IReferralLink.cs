using System;

namespace XUnitTestData.Domains.BlueApi
{
    public interface IReferralLink : IDictionaryItem
    {
        string Url { get; set; }
        string SenderClientId { get; set; }
        DateTime? ExpirationDate { get; set; }
        string Asset { get; set; }
        double Amount { get; set; }
        string SenderOffchainTransferId { get; set; }
        string Type { get; set; }
        string State { get; set; }
    }
}