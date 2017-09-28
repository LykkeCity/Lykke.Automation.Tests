namespace XUnitTestData.Domains.Assets
{
    public interface IAssetPair : IDictionaryItem
    {
        string Name { get; }
        string BaseAssetId { get; }
        string QuotingAssetId { get; }
        int Accuracy { get; }
        int InvertedAccuracy { get; }
        string Source { get; }
        string Source2 { get; }
        bool IsDisabled { get; }
    }
}