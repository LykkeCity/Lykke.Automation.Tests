namespace BlueApiData.DTOs
{
    public class RequestGiftCoinsLinkRequestDto
    {
        public string SenderClientId { get; set; }
        public string Asset { get; set; }
        public decimal Amount { get; set; }
    }
}