// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class OffchainCashoutModel
    {
        /// <summary>
        /// Initializes a new instance of the OffchainCashoutModel class.
        /// </summary>
        public OffchainCashoutModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the OffchainCashoutModel class.
        /// </summary>
        public OffchainCashoutModel(string destination = default(string), double? amount = default(double?), string asset = default(string), string prevTempPrivateKey = default(string))
        {
            Destination = destination;
            Amount = amount;
            Asset = asset;
            PrevTempPrivateKey = prevTempPrivateKey;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Destination")]
        public string Destination { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Asset")]
        public string Asset { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PrevTempPrivateKey")]
        public string PrevTempPrivateKey { get; set; }

    }
}