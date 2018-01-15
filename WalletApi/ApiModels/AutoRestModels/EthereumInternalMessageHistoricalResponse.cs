// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class EthereumInternalMessageHistoricalResponse
    {
        /// <summary>
        /// Initializes a new instance of the
        /// EthereumInternalMessageHistoricalResponse class.
        /// </summary>
        public EthereumInternalMessageHistoricalResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// EthereumInternalMessageHistoricalResponse class.
        /// </summary>
        public EthereumInternalMessageHistoricalResponse(string transactionHash = default(string), long? blockNumber = default(long?), string fromAddress = default(string), string toAddress = default(string), int? depth = default(int?), double? value = default(double?), int? messageIndex = default(int?), string type = default(string), System.DateTime? blockTimeUtc = default(System.DateTime?), int? blockTimestamp = default(int?))
        {
            TransactionHash = transactionHash;
            BlockNumber = blockNumber;
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Depth = depth;
            Value = value;
            MessageIndex = messageIndex;
            Type = type;
            BlockTimeUtc = blockTimeUtc;
            BlockTimestamp = blockTimestamp;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TransactionHash")]
        public string TransactionHash { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BlockNumber")]
        public long? BlockNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "FromAddress")]
        public string FromAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ToAddress")]
        public string ToAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Depth")]
        public int? Depth { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Value")]
        public double? Value { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MessageIndex")]
        public int? MessageIndex { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BlockTimeUtc")]
        public System.DateTime? BlockTimeUtc { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BlockTimestamp")]
        public int? BlockTimestamp { get; set; }

    }
}