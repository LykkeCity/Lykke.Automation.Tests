// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ResponseModelDictionariesUpdatesRespModel
    {
        /// <summary>
        /// Initializes a new instance of the
        /// ResponseModelDictionariesUpdatesRespModel class.
        /// </summary>
        public ResponseModelDictionariesUpdatesRespModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// ResponseModelDictionariesUpdatesRespModel class.
        /// </summary>
        public ResponseModelDictionariesUpdatesRespModel(DictionariesUpdatesRespModel result = default(DictionariesUpdatesRespModel), ErrorModel error = default(ErrorModel))
        {
            Result = result;
            Error = error;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Result")]
        public DictionariesUpdatesRespModel Result { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Error")]
        public ErrorModel Error { get; set; }

    }
}