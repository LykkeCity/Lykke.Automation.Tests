// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ClientDialogSubmitModel
    {
        /// <summary>
        /// Initializes a new instance of the ClientDialogSubmitModel class.
        /// </summary>
        public ClientDialogSubmitModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ClientDialogSubmitModel class.
        /// </summary>
        public ClientDialogSubmitModel(System.Guid? id = default(System.Guid?), System.Guid? buttonId = default(System.Guid?))
        {
            Id = id;
            ButtonId = buttonId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ButtonId")]
        public System.Guid? ButtonId { get; set; }

    }
}
