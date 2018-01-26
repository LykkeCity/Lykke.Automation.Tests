using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models
{
    public class OrderRequestModel
    {
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string exchangeCurrency { get; set; }
        public string successURL { get; set; }
        public string errorURL { get; set; }
        public string progressURL { get; set; }
        public string orderId { get; set; }
        public PostMarkup markup { get; set; }
    }

    public class OrderResponse
    {
        public int timestamp { get; set; }
        public string address { get; set; }
        public string orderId { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public decimal recommendedFee { get; set; }
        public decimal totalAmount { get; set; }
        public decimal exchangeRate { get; set; }
    }
}
