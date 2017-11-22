namespace XUnitTestData.Domains.ApiV2
{
    public interface ITrader : IDictionaryItem
    {
        string PrimaryPartitionKey { get; set; }
        string PrimaryRowKey { get; set; }
    }
}