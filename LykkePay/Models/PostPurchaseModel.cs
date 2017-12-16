using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models
{
    public class PostPurchaseModel
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


        public PostPurchaseModel(string destinationAddress, string assetPair, string baseAsset, decimal amount)
        {
            this.destinationAddress = destinationAddress;
            this.assetPair = assetPair;
            this.baseAsset = baseAsset;
            this.amount = amount;
            //markup = new PostMarkup() { fixedFee = 0.001 };
        }
    }

    public class PostMarkup
    {
        public double? percent { get; set; }
        public int? pips { get; set; }
        public double? fixedFee { get; set; }

        public PostMarkup() { }

        public PostMarkup(double? percent, int? pips, double? fixedFee)
        {
            this.percent = percent;
            this.pips = pips;
            this.fixedFee = fixedFee;
        }

        public PostMarkup(MarkupModel markupModel, double fixedFee)
        {
            this.percent = markupModel.markup.percent;
            this.pips = markupModel.markup.pips;
            this.fixedFee = fixedFee;
        }
    }
}
