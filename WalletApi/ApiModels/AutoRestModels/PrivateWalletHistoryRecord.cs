// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class PrivateWalletHistoryRecord
    {
        /// <summary>
        /// Initializes a new instance of the PrivateWalletHistoryRecord class.
        /// </summary>
        public PrivateWalletHistoryRecord()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PrivateWalletHistoryRecord class.
        /// </summary>
        public PrivateWalletHistoryRecord(string transactionHash = default(string), System.DateTime? dateTime = default(System.DateTime?), string assetId = default(string), double? amount = default(double?), string baseAssetId = default(string), double? amountInBase = default(double?))
        {
            TransactionHash = transactionHash;
            DateTime = dateTime;
            AssetId = assetId;
            Amount = amount;
            BaseAssetId = baseAssetId;
            AmountInBase = amountInBase;
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
        [JsonProperty(PropertyName = "DateTime")]
        public System.DateTime? DateTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BaseAssetId")]
        public string BaseAssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AmountInBase")]
        public double? AmountInBase { get; set; }

    }
}
