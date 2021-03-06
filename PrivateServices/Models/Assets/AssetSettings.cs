// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class AssetSettings
    {
        /// <summary>
        /// Initializes a new instance of the AssetSettings class.
        /// </summary>
        public AssetSettings()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AssetSettings class.
        /// </summary>
        public AssetSettings(double cashinCoef, double dust, int maxOutputsCount, int maxOutputsCountInTx, double minBalance, int minOutputsCount, double outputSize, int privateIncrement, string asset = default(string), string changeWallet = default(string), string hotWallet = default(string), double? maxBalance = default(double?))
        {
            Asset = asset;
            CashinCoef = cashinCoef;
            ChangeWallet = changeWallet;
            Dust = dust;
            HotWallet = hotWallet;
            MaxBalance = maxBalance;
            MaxOutputsCount = maxOutputsCount;
            MaxOutputsCountInTx = maxOutputsCountInTx;
            MinBalance = minBalance;
            MinOutputsCount = minOutputsCount;
            OutputSize = outputSize;
            PrivateIncrement = privateIncrement;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Asset")]
        public string Asset { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CashinCoef")]
        public double CashinCoef { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ChangeWallet")]
        public string ChangeWallet { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Dust")]
        public double Dust { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "HotWallet")]
        public string HotWallet { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MaxBalance")]
        public double? MaxBalance { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MaxOutputsCount")]
        public int MaxOutputsCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MaxOutputsCountInTx")]
        public int MaxOutputsCountInTx { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MinBalance")]
        public double MinBalance { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MinOutputsCount")]
        public int MinOutputsCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "OutputSize")]
        public double OutputSize { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PrivateIncrement")]
        public int PrivateIncrement { get; set; }

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
