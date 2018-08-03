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
    /// Response model for placing new limit orders.
    /// </summary>
    public partial class LimitOrderResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the LimitOrderResponseModel class.
        /// </summary>
        public LimitOrderResponseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LimitOrderResponseModel class.
        /// </summary>
        /// <param name="id">The identifier under which the limit order was
        /// placed.</param>
        public LimitOrderResponseModel(System.Guid id)
        {
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the identifier under which the limit order was placed.
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public System.Guid Id { get; set; }

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