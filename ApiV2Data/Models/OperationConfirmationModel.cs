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

    public partial class OperationConfirmationModel
    {
        /// <summary>
        /// Initializes a new instance of the OperationConfirmationModel class.
        /// </summary>
        public OperationConfirmationModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the OperationConfirmationModel class.
        /// </summary>
        public OperationConfirmationModel(string type, string operationId, OperationConfirmationSignature signature)
        {
            Type = type;
            OperationId = operationId;
            Signature = signature;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "OperationId")]
        public string OperationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Signature")]
        public OperationConfirmationSignature Signature { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Type == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Type");
            }
            if (OperationId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "OperationId");
            }
            if (Signature == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Signature");
            }
            if (Signature != null)
            {
                Signature.Validate();
            }
        }
    }
}
