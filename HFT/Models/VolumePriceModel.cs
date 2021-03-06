// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// The volume and price of an order in the orderbook.
    /// </summary>
    public partial class VolumePriceModel
    {
        /// <summary>
        /// Initializes a new instance of the VolumePriceModel class.
        /// </summary>
        public VolumePriceModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the VolumePriceModel class.
        /// </summary>
        /// <param name="volume">The volume of the order.</param>
        /// <param name="price">The price of the order.</param>
        public VolumePriceModel(double volume, double price)
        {
            Volume = volume;
            Price = price;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the volume of the order.
        /// </summary>
        [JsonProperty(PropertyName = "Volume")]
        public double Volume { get; set; }

        /// <summary>
        /// Gets or sets the price of the order.
        /// </summary>
        [JsonProperty(PropertyName = "Price")]
        public double Price { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
