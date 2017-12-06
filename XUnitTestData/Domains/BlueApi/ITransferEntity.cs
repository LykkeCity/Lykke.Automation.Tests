using System;

namespace XUnitTestData.Domains.BlueApi
{
    public interface ITransfer : IDictionaryItem
    {
        string AddressFrom { get; set; }
        string AddressTo { get; set; }
        double Amount { get; set; }
        string AssetId { get; set; }
        string ClientId { get; set; }
        DateTime DateTime { get; set; }
        bool IsHidden { get; set; }
        bool IsSettled { get; set; }
        string Multisig { get; set; }
        string StateField { get; set; }
        string TransactionId { get; set; }
    }
}