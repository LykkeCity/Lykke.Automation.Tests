namespace XUnitTestData.Domains.Assets
{
    public interface IAssetIssuers : IDictionaryItem
    {
        string IconUrl { get; set; }
        string Name { get; set; }
    }
}