using System;
using System.Collections.Generic;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class AlgoMetaDataFunction
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string FunctionParameterType { get; set; }
        public List<MetaDataParameter> Parameters { get; set; }

        public AlgoMetaDataFunction(FunctionType functionType)
        {
            switch (functionType)
            {
                case FunctionType.SMA_Short:
                case FunctionType.SMA_Long:
                    Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction";
                    FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters";
                    break;
                case FunctionType.ADX:
                    Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxFunction";
                    FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxParameters";
                    break;
                case FunctionType.MACD:
                    throw new NotImplementedException($"{functionType} is not implemented yet");
                default:
                    throw new NotImplementedException($"{functionType} is not defined. Consider adding it to the switch statement.");
            }
        }
    }
}
