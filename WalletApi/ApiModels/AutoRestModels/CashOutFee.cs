// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CashOutFee
    {
        /// <summary>
        /// Initializes a new instance of the CashOutFee class.
        /// </summary>
        public CashOutFee()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CashOutFee class.
        /// </summary>
        public CashOutFee(string assetId = default(string), double? size = default(double?), string type = default(string))
        {
            AssetId = assetId;
            Size = size;
            Type = type;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Size")]
        public double? Size { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

    }
}
