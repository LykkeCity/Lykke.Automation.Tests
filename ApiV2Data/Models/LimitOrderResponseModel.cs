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
        public LimitOrderResponseModel(System.Guid id, string assetPairId, double volume, double price, double lowerLimitPrice, double lowerPrice, double upperLimitPrice, double upperPrice, System.DateTime createDateTime, string orderAction, string status, string type, double remainingVolume)
        {
            Id = id;
            AssetPairId = assetPairId;
            Volume = volume;
            Price = price;
            LowerLimitPrice = lowerLimitPrice;
            LowerPrice = lowerPrice;
            UpperLimitPrice = upperLimitPrice;
            UpperPrice = upperPrice;
            CreateDateTime = createDateTime;
            OrderAction = orderAction;
            Status = status;
            Type = type;
            RemainingVolume = remainingVolume;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public System.Guid Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetPairId")]
        public string AssetPairId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Volume")]
        public double Volume { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Price")]
        public double Price { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LowerLimitPrice")]
        public double LowerLimitPrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LowerPrice")]
        public double LowerPrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "UpperLimitPrice")]
        public double UpperLimitPrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "UpperPrice")]
        public double UpperPrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CreateDateTime")]
        public System.DateTime CreateDateTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "OrderAction")]
        public string OrderAction { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RemainingVolume")]
        public double RemainingVolume { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (AssetPairId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "AssetPairId");
            }
            if (OrderAction == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "OrderAction");
            }
            if (Status == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Status");
            }
            if (Type == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Type");
            }
        }
    }
}
