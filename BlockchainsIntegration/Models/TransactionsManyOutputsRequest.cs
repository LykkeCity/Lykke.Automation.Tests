using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class TransactionsManyOutputsRequest
    {
        public string operationId { get; set; }
        public string fromAddress { get; set; }
        public Output[] outputs { get; set; }
        public string assetId { get; set; }
    }
    public class Output
    {
        public string toAddress { get; set; }
        public string amount { get; set; }
    }
}
