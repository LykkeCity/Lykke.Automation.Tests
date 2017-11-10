namespace XUnitTestData.Domains.ApiV2
{
    public interface IWallet : IDictionaryItem
    {
        string ClientId { get; set; }
        string Description { get; set; }
        string Name { get; set; }
        string State { get; set; }
        string Type { get; set; }
    }
}