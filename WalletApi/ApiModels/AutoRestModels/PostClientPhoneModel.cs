// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class PostClientPhoneModel
    {
        /// <summary>
        /// Initializes a new instance of the PostClientPhoneModel class.
        /// </summary>
        public PostClientPhoneModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PostClientPhoneModel class.
        /// </summary>
        public PostClientPhoneModel(string phoneNumber = default(string))
        {
            PhoneNumber = phoneNumber;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }

    }
}
