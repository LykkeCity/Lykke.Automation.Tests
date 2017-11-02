namespace XUnitTestData.Domains.Assets
{
    public interface IMarginAssetPairs : IDictionaryItem
    {
        int Accuracy { get; set; }
        string BaseAssetId { get; set; }
        int InvertedAccuracy { get; set; }
        string Name { get; set; }
        string QuotingAssetId { get; set; }
    }
}