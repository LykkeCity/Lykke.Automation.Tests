using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models
{
    public class ConvertRequestModel
    {
        public string destinationAddress { get; set; }
        public string assetPair { get; set; }
        public string baseAsset { get; set; }
        public double amount { get; set; }
        public string successURL { get; set; }
        public string errorURL { get; set; }
        public string progressURL { get; set; }
        public PostMarkup markup { get; set; }
    }
}
