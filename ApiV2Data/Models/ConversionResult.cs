// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ConversionResult
    {
        /// <summary>
        /// Initializes a new instance of the ConversionResult class.
        /// </summary>
        public ConversionResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ConversionResult class.
        /// </summary>
        /// <param name="result">Possible values include: 'Unknown', 'Ok',
        /// 'InvalidInputParameters', 'NoLiquidity'</param>
        public ConversionResult(AssetWithAmount fromProperty, AssetWithAmount to, double price, double volumePrice, OperationResult result)
        {
            FromProperty = fromProperty;
            To = to;
            Price = price;
            VolumePrice = volumePrice;
            Result = result;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "From")]
        public AssetWithAmount FromProperty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "To")]
        public AssetWithAmount To { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Price")]
        public double Price { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "VolumePrice")]
        public double VolumePrice { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Unknown', 'Ok',
        /// 'InvalidInputParameters', 'NoLiquidity'
        /// </summary>
        [JsonProperty(PropertyName = "Result")]
        public OperationResult Result { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (FromProperty == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "FromProperty");
            }
            if (To == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "To");
            }
            if (FromProperty != null)
            {
                FromProperty.Validate();
            }
            if (To != null)
            {
                To.Validate();
            }
        }
    }
}
