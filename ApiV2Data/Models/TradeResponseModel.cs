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

    public partial class TradeResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the TradeResponseModel class.
        /// </summary>
        public TradeResponseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TradeResponseModel class.
        /// </summary>
        /// <param name="direction">Possible values include: 'Buy',
        /// 'Sell'</param>
        public TradeResponseModel(System.Guid id, System.Guid orderId, string assetPairId, double price, Direction direction, string baseAssetName, double baseVolume, string quoteAssetName, double quoteVolume, System.DateTime timestamp)
        {
            Id = id;
            OrderId = orderId;
            AssetPairId = assetPairId;
            Price = price;
            Direction = direction;
            BaseAssetName = baseAssetName;
            BaseVolume = baseVolume;
            QuoteAssetName = quoteAssetName;
            QuoteVolume = quoteVolume;
            Timestamp = timestamp;
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
        [JsonProperty(PropertyName = "OrderId")]
        public System.Guid OrderId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetPairId")]
        public string AssetPairId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Price")]
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Buy', 'Sell'
        /// </summary>
        [JsonProperty(PropertyName = "Direction")]
        public Direction Direction { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BaseAssetName")]
        public string BaseAssetName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BaseVolume")]
        public double BaseVolume { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "QuoteAssetName")]
        public string QuoteAssetName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "QuoteVolume")]
        public double QuoteVolume { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Timestamp")]
        public System.DateTime Timestamp { get; set; }

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
            if (BaseAssetName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "BaseAssetName");
            }
            if (QuoteAssetName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "QuoteAssetName");
            }
        }
    }
}
