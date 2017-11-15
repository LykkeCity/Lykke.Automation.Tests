namespace XUnitTestData.Domains.ApiV2
{
    public interface ITrader : IDictionaryItem
    {
        string PrimeryPartitionKey { get; set; }
        string PrimeryRowKey { get; set; }
    }
}