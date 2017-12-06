using System;
using System.Collections.Generic;
using System.Text;

namespace BlueApiData.DTOs
{
    public class RefferalLinkDataDTO
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string SenderClientId { get; set; }
        public string Asset { get; set; }
        public string State { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public string SenderOffchainTransferId { get; set; }
    }
}
