namespace Lykke.Client.ApiV2.Models
{
    public class HistoryWalletFundsResponse
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public string AssetName { get; set; }
        public double Volume { get; set; }
        public string Operation { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Timestamp { get; set; }
    }
}