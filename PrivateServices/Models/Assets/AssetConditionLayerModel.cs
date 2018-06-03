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

    /// <summary>
    /// Represents an asset conditon layer.
    /// </summary>
    public partial class AssetConditionLayerModel
    {
        /// <summary>
        /// Initializes a new instance of the AssetConditionLayerModel class.
        /// </summary>
        public AssetConditionLayerModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AssetConditionLayerModel class.
        /// </summary>
        /// <param name="priority">The asset condition layer priority.</param>
        /// <param name="id">The asset condition layer id.</param>
        /// <param name="description">The asset condition layer
        /// description.</param>
        /// <param name="clientsCanCashInViaBankCards">The client ability to
        /// cash in via bank cards.</param>
        /// <param name="swiftDepositEnabled">The client ability to swift
        /// deposit.</param>
        /// <param name="assetConditions">The collection of asset conditions
        /// for layer.</param>
        /// <param name="defaultCondition">The wildcard asset condition. It
        /// applies to all asset not included in
        /// Lykke.Service.Assets.Responses.v2.AssetConditions.AssetConditionLayerModel.AssetConditions.</param>
        public AssetConditionLayerModel(double priority, string id = default(string), string description = default(string), bool? clientsCanCashInViaBankCards = default(bool?), bool? swiftDepositEnabled = default(bool?), IList<AssetConditionModel> assetConditions = default(IList<AssetConditionModel>), AssetDefaultConditionModel defaultCondition = default(AssetDefaultConditionModel))
        {
            Id = id;
            Priority = priority;
            Description = description;
            ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards;
            SwiftDepositEnabled = swiftDepositEnabled;
            AssetConditions = assetConditions;
            DefaultCondition = defaultCondition;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the asset condition layer id.
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the asset condition layer priority.
        /// </summary>
        [JsonProperty(PropertyName = "Priority")]
        public double Priority { get; set; }

        /// <summary>
        /// Gets or sets the asset condition layer description.
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the client ability to cash in via bank cards.
        /// </summary>
        [JsonProperty(PropertyName = "ClientsCanCashInViaBankCards")]
        public bool? ClientsCanCashInViaBankCards { get; set; }

        /// <summary>
        /// Gets or sets the client ability to swift deposit.
        /// </summary>
        [JsonProperty(PropertyName = "SwiftDepositEnabled")]
        public bool? SwiftDepositEnabled { get; set; }

        /// <summary>
        /// Gets or sets the collection of asset conditions for layer.
        /// </summary>
        [JsonProperty(PropertyName = "AssetConditions")]
        public IList<AssetConditionModel> AssetConditions { get; set; }

        /// <summary>
        /// Gets or sets the wildcard asset condition. It applies to all asset
        /// not included in
        /// Lykke.Service.Assets.Responses.v2.AssetConditions.AssetConditionLayerModel.AssetConditions.
        /// </summary>
        [JsonProperty(PropertyName = "DefaultCondition")]
        public AssetDefaultConditionModel DefaultCondition { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}