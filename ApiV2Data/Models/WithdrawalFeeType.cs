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
    /// Defines values for WithdrawalFeeType.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WithdrawalFeeType
    {
        [EnumMember(Value = "Absolute")]
        Absolute,
        [EnumMember(Value = "Relative")]
        Relative
    }
    internal static class WithdrawalFeeTypeEnumExtension
    {
        internal static string ToSerializedValue(this WithdrawalFeeType? value)
        {
            return value == null ? null : ((WithdrawalFeeType)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this WithdrawalFeeType value)
        {
            switch( value )
            {
                case WithdrawalFeeType.Absolute:
                    return "Absolute";
                case WithdrawalFeeType.Relative:
                    return "Relative";
            }
            return null;
        }

        internal static WithdrawalFeeType? ParseWithdrawalFeeType(this string value)
        {
            switch( value )
            {
                case "Absolute":
                    return WithdrawalFeeType.Absolute;
                case "Relative":
                    return WithdrawalFeeType.Relative;
            }
            return null;
        }
    }
}
