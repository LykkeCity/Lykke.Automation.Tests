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

    public partial class CreateWalletRequest
    {
        /// <summary>
        /// Initializes a new instance of the CreateWalletRequest class.
        /// </summary>
        public CreateWalletRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CreateWalletRequest class.
        /// </summary>
        /// <param name="type">Possible values include: 'Trusted',
        /// 'Trading'</param>
        public CreateWalletRequest(WalletType type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets possible values include: 'Trusted', 'Trading'
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public WalletType Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (Description == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Description");
            }
        }
    }
}
