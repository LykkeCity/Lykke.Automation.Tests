// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Erc20TokenSpecification
    {
        /// <summary>
        /// Initializes a new instance of the Erc20TokenSpecification class.
        /// </summary>
        public Erc20TokenSpecification()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Erc20TokenSpecification class.
        /// </summary>
        public Erc20TokenSpecification(IList<string> ids = default(IList<string>))
        {
            Ids = ids;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Ids")]
        public IList<string> Ids { get; set; }

    }
}
