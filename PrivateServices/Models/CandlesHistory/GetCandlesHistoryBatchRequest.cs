// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class GetCandlesHistoryBatchRequest
    {
        /// <summary>
        /// Initializes a new instance of the GetCandlesHistoryBatchRequest
        /// class.
        /// </summary>
        public GetCandlesHistoryBatchRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GetCandlesHistoryBatchRequest
        /// class.
        /// </summary>
        /// <param name="priceType">Possible values include: 'Unspecified',
        /// 'Bid', 'Ask', 'Mid', 'Trades'</param>
        /// <param name="timeInterval">Possible values include: 'Unspecified',
        /// 'Sec', 'Minute', 'Min5', 'Min15', 'Min30', 'Hour', 'Hour4',
        /// 'Hour6', 'Hour12', 'Day', 'Week', 'Month'</param>
        /// <param name="fromMoment">Inclusive from moment</param>
        /// <param name="toMoment">Exclusive to moment. If equals to the
        /// Lykke.Service.CandlesHistory.Models.CandlesHistory.GetCandlesHistoryBatchRequest.FromMoment,
        /// then exactly candle for exactly this moment will be
        /// returned</param>
        public GetCandlesHistoryBatchRequest(CandlePriceType priceType, CandleTimeInterval timeInterval, System.DateTime fromMoment, System.DateTime toMoment, IList<string> assetPairs = default(IList<string>))
        {
            AssetPairs = assetPairs;
            PriceType = priceType;
            TimeInterval = timeInterval;
            FromMoment = fromMoment;
            ToMoment = toMoment;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetPairs")]
        public IList<string> AssetPairs { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Unspecified', 'Bid', 'Ask',
        /// 'Mid', 'Trades'
        /// </summary>
        [JsonProperty(PropertyName = "PriceType")]
        public CandlePriceType PriceType { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Unspecified', 'Sec',
        /// 'Minute', 'Min5', 'Min15', 'Min30', 'Hour', 'Hour4', 'Hour6',
        /// 'Hour12', 'Day', 'Week', 'Month'
        /// </summary>
        [JsonProperty(PropertyName = "TimeInterval")]
        public CandleTimeInterval TimeInterval { get; set; }

        /// <summary>
        /// Gets or sets inclusive from moment
        /// </summary>
        [JsonProperty(PropertyName = "FromMoment")]
        public System.DateTime FromMoment { get; set; }

        /// <summary>
        /// Gets or sets exclusive to moment. If equals to the
        /// Lykke.Service.CandlesHistory.Models.CandlesHistory.GetCandlesHistoryBatchRequest.FromMoment,
        /// then exactly candle for exactly this moment will be returned
        /// </summary>
        [JsonProperty(PropertyName = "ToMoment")]
        public System.DateTime ToMoment { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}