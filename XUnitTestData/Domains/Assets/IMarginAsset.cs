namespace XUnitTestData.Domains.Assets
{
    public interface IMarginAsset : IDictionaryItem
    {
        int Accuracy { get; set; }
        double DustLimit { get; set; }
        string IdIssuer { get; set; }
        double Multiplier { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
    }
}