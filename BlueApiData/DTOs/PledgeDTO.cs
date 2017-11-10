namespace BlueApiData.DTOs
{
    public class PledgeDTO
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public int CO2Footprint { get; set; }
        public int ClimatePositiveValue { get; set; }
    }

    public class CreatePledgeDTO
    {
        public int CO2Footprint { get; set; }
        public int ClimatePositiveValue { get; set; }
    }
}
