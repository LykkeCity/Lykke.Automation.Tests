// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ApiIssuer
    {
        /// <summary>
        /// Initializes a new instance of the ApiIssuer class.
        /// </summary>
        public ApiIssuer()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ApiIssuer class.
        /// </summary>
        public ApiIssuer(string name = default(string), string iconUrl = default(string), string id = default(string))
        {
            Name = name;
            IconUrl = iconUrl;
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IconUrl")]
        public string IconUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

    }
}
