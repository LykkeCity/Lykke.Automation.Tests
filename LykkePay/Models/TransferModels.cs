using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models
{
    public class TransferRequestModel
    {
        public string sourceAddress { get; set; }
        public string destinationAddress { get; set; }
        public decimal amount { get; set; }
        public string assetId { get; set; }
        public string successURL { get; set; }
        public string errorURL { get; set; }
        public string orderId { get; set; }
    }

    public class TransferResponseModel
    {
        public string transferStatus { get; set; }
        public TransferResponse transferResponse { get; set; }
    }

    public class TransferResponse
    {
        public string currency { get; set; }
        public Int64 timestamp { get; set; }
        public string settlement { get; set; }
        public string transactionId { get; set; }
    }
}
