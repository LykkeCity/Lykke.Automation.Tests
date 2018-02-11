using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class TransactionsManyInputsRequest
    {
        public string operationId { get; set; }
        public Input[] inputs { get; set; }
        public string toAddress { get; set; }
        public string assetId { get; set; }
    }
    public class Input
    {
        public string fromAddress { get; set; }
        public string amount { get; set; }
    }
}
