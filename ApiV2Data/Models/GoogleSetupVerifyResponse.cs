// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class GoogleSetupVerifyResponse
    {
        /// <summary>
        /// Initializes a new instance of the GoogleSetupVerifyResponse class.
        /// </summary>
        public GoogleSetupVerifyResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GoogleSetupVerifyResponse class.
        /// </summary>
        public GoogleSetupVerifyResponse(bool isValid)
        {
            IsValid = isValid;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsValid")]
        public bool IsValid { get; set; }

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
