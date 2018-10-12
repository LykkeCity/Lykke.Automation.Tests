// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Model for a trading fee.
    /// </summary>
    public partial class FeeModel
    {
        /// <summary>
        /// Initializes a new instance of the FeeModel class.
        /// </summary>
        public FeeModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the FeeModel class.
        /// </summary>
        /// <param name="amount">The fee amount.</param>
        /// <param name="type">The fee type. Possible values include:
        /// 'Unknown', 'Absolute', 'Relative'</param>
        /// <param name="feeAssetId">Asset that was used for fee.</param>
        public FeeModel(double ?amount, FeeType type, string feeAssetId)
        {
            Amount = amount;
            Type = type;
            FeeAssetId = feeAssetId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the fee amount.
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public double ?Amount { get; set; }

        /// <summary>
        /// Gets or sets the fee type. Possible values include: 'Unknown',
        /// 'Absolute', 'Relative'
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public FeeType Type { get; set; }

        /// <summary>
        /// Gets or sets asset that was used for fee.
        /// </summary>
        [JsonProperty(PropertyName = "FeeAssetId")]
        public string FeeAssetId { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {

        }
    }
}
