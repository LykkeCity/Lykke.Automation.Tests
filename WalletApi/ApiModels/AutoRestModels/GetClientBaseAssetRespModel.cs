// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class GetClientBaseAssetRespModel
    {
        /// <summary>
        /// Initializes a new instance of the GetClientBaseAssetRespModel
        /// class.
        /// </summary>
        public GetClientBaseAssetRespModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GetClientBaseAssetRespModel
        /// class.
        /// </summary>
        public GetClientBaseAssetRespModel(ApiAssetModel asset = default(ApiAssetModel))
        {
            Asset = asset;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Asset")]
        public ApiAssetModel Asset { get; set; }

    }
}