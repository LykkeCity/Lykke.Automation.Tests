using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class PutTransactionsRequest
    {
        public string operationId { get; set; }
        public float feeFactor { get; set; }
    }
}
