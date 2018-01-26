using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models
{
    public class PostPurchaseResponseModel
    {
        public string transferStatus { get; set; }
        public TransferResponse transferResponse { get; set; }
    }
}
