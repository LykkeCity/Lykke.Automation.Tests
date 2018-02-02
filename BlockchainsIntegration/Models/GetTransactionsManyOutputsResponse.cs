using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class GetTransactionsManyOutputsResponse
    {
        public string operationId { get; set; }
        public string state { get; set; }
        public string timestamp { get; set; }
        public string fromAddress { get; set; }
        public Output[] outputs { get; set; }
        public string fee { get; set; }
        public string hash { get; set; }
        public string error { get; set; }
        public string errorCode { get; set; }
        public long block { get; set; }
    }
}
