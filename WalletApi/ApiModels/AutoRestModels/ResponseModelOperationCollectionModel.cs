// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ResponseModelOperationCollectionModel
    {
        /// <summary>
        /// Initializes a new instance of the
        /// ResponseModelOperationCollectionModel class.
        /// </summary>
        public ResponseModelOperationCollectionModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// ResponseModelOperationCollectionModel class.
        /// </summary>
        public ResponseModelOperationCollectionModel(OperationCollectionModel result = default(OperationCollectionModel), ErrorModel error = default(ErrorModel))
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
        public OperationCollectionModel Result { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Error")]
        public ErrorModel Error { get; set; }

    }
}
