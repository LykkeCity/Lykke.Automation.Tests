// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CheckDocumentsToUploadModel
    {
        /// <summary>
        /// Initializes a new instance of the CheckDocumentsToUploadModel
        /// class.
        /// </summary>
        public CheckDocumentsToUploadModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CheckDocumentsToUploadModel
        /// class.
        /// </summary>
        public CheckDocumentsToUploadModel(bool? idCard = default(bool?), bool? proofOfAddress = default(bool?), bool? selfie = default(bool?))
        {
            IdCard = idCard;
            ProofOfAddress = proofOfAddress;
            Selfie = selfie;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IdCard")]
        public bool? IdCard { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ProofOfAddress")]
        public bool? ProofOfAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Selfie")]
        public bool? Selfie { get; set; }

    }
}