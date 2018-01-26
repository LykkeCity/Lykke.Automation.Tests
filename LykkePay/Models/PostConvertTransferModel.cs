using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models
{
    public class PostConvertTransferModel
    {
        public string destinationAddress { get; set; }
        public string assetPair { get; set; }
        public string baseAsset { get; set; }
        public decimal amount { get; set; }
        public string successUrl { get; set; }
        public string errorUrl { get; set; }
        public string progressUrl { get; set; }
        public string orderId { get; set; }
        public PostMarkup markup { get; set; }


        public PostConvertTransferModel(string destinationAddress, string assetPair, string baseAsset, decimal amount)
        {
            this.destinationAddress = destinationAddress;
            this.assetPair = assetPair;
            this.baseAsset = baseAsset;
            this.amount = amount;
            //markup = new PostMarkup() { fixedFee = 0.001 };
        }
    }
}
