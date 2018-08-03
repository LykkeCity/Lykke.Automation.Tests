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
    /// Response model for placing new market orders.
    /// </summary>
    public partial class MarketOrderResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the MarketOrderResponseModel class.
        /// </summary>
        public MarketOrderResponseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MarketOrderResponseModel class.
        /// </summary>
        /// <param name="price">The (average) price for which the market order
        /// was settled.</param>
        public MarketOrderResponseModel(double price)
        {
            Price = price;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the (average) price for which the market order was
        /// settled.
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