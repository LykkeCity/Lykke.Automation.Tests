namespace XUnitTestData.Domains.Assets
{
    public interface IAssetSettings : IDictionaryItem
    {
        string CashinCoef { get; set; }
        string ChangeWallet { get; set; }
        string Dust { get; set; }
        string HotWallet { get; set; }
        string Id { get; }
        string MaxBalance { get; set; }
        int MaxOutputsCount { get; set; }
        int MaxOutputsCountInTx { get; set; }
        string MinBalance { get; set; }
        int MinOutputsCount { get; set; }
        string OutputSize { get; set; }
        int PrivateIncrement { get; set; }
    }
}