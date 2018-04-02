using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    [JsonArray]
    public class GetTransactionsHistoryFromToResponse
    {    
        public TransactionHistory[] TransactionHistoryFrom { get; set; }
    }
    public class TransactionHistory
    {
        public string timestamp { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public string assetId { get; set; }
        public string amount { get; set; }
        public string hash { get; set; }
        public string transactionType { get; set; }
    }
}
