
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class GetTransactionsManyInputsResponse
    {
            public string operationId { get; set; }
            public string state { get; set; }
            public string timestamp { get; set; }
            public Input[] inputs { get; set; }
            public string toAddress { get; set; }
            public string fee { get; set; }
            public string hash { get; set; }
            public string error { get; set; }
            public string errorCode { get; set; }
            public long block { get; set; }
    }
}
