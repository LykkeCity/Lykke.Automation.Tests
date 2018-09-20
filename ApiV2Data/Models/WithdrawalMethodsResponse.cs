// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class WithdrawalMethodsResponse
    {
        /// <summary>
        /// Initializes a new instance of the WithdrawalMethodsResponse class.
        /// </summary>
        public WithdrawalMethodsResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the WithdrawalMethodsResponse class.
        /// </summary>
        public WithdrawalMethodsResponse(IList<WithdrawalMethod> withdrawalMethods)
        {
            WithdrawalMethods = withdrawalMethods;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "WithdrawalMethods")]
        public IList<WithdrawalMethod> WithdrawalMethods { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (WithdrawalMethods == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "WithdrawalMethods");
            }
            if (WithdrawalMethods != null)
            {
                foreach (var element in WithdrawalMethods)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}
