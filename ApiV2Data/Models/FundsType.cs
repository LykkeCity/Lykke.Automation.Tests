// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for FundsType.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FundsType
    {
        [EnumMember(Value = "Undefined")]
        Undefined,
        [EnumMember(Value = "Card")]
        Card,
        [EnumMember(Value = "Bank")]
        Bank,
        [EnumMember(Value = "Blockchain")]
        Blockchain
    }
    internal static class FundsTypeEnumExtension
    {
        internal static string ToSerializedValue(this FundsType? value)
        {
            return value == null ? null : ((FundsType)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this FundsType value)
        {
            switch( value )
            {
                case FundsType.Undefined:
                    return "Undefined";
                case FundsType.Card:
                    return "Card";
                case FundsType.Bank:
                    return "Bank";
                case FundsType.Blockchain:
                    return "Blockchain";
            }
            return null;
        }

        internal static FundsType? ParseFundsType(this string value)
        {
            switch( value )
            {
                case "Undefined":
                    return FundsType.Undefined;
                case "Card":
                    return FundsType.Card;
                case "Bank":
                    return FundsType.Bank;
                case "Blockchain":
                    return FundsType.Blockchain;
            }
            return null;
        }
    }
}
