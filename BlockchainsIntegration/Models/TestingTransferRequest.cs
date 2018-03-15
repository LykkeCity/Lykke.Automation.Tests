using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainsIntegration.Models
{
    public class TestingTransferRequest
    {
        public string fromAddress { get; set; }
        public string fromPrivateKey { get; set; }
        public string toAddress { get; set; }
        public string assetId { get; set; }
        public string amount { get; set; }
    }
}