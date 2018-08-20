using System;

namespace AlgoStoreData.DTOs
{
    public class ManualCashInDTO
    {
        public String UserId { get; set; }
        public String Comment { get; set; }
        public String ClientId { get; set; }
        public String AssetId { get; set; }
        public Double Amount { get; set; }

        public ManualCashInDTO() { }

        public ManualCashInDTO(String userId, String clientId, String assetId, Double amount)
        {
            this.UserId = userId;
            this.ClientId = clientId;
            this.AssetId = assetId;
            this.Amount = amount;
            this.Comment = $"Add '{this.Amount} {this.AssetId}' test funds for WalletId: {this.ClientId}. Added at: {DateTime.UtcNow.ToString("s")}";
        }
    }
}
