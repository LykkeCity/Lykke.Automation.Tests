namespace XUnitTestData.Domains.Assets
{
    public interface IAssetGroup : IDictionaryItem
    {
        bool ClientsCanCashInViaBankCards { get; set; }
        string Id { get; }
        bool IsIosDevice { get; set; }
        string Name { get; set; }
        bool SwiftDepositEnabled { get; set; }
    }
}