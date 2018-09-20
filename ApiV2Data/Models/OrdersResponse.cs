namespace ApiV2Data.Models
{
    public class OrdersResponse
    {
        public string Id { get; set; }
        public string AssetPairId { get; set; }
        public float Volume { get; set; }
        public float Price { get; set; }
        public float LowerLimitPrice { get; set; }
        public float LowerPrice { get; set; }
        public float UpperLimitPrice { get; set; }
        public float UpperPrice { get; set; }
        public string CreateDateTime { get; set; }
        public string OrderAction { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public float RemainingVolume { get; set; }
    }
}
