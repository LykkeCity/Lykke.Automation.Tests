// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class AskBid
    {
        /// <summary>
        /// Initializes a new instance of the AskBid class.
        /// </summary>
        public AskBid()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AskBid class.
        /// </summary>
        public AskBid(double? a = default(double?), double? b = default(double?))
        {
            A = a;
            B = b;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "A")]
        public double? A { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "B")]
        public double? B { get; set; }

    }
}