namespace XUnitTestData.Domains.ApiV2
{
    public interface IPledgeEntity : IDictionaryItem
    {
        string ClientId { get; set; }
        int ClimatePositiveValue { get; set; }
        int CO2Footprint { get; set; }
    }
}