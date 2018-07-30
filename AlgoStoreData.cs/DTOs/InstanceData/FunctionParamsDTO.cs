using System;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class FunctionParamsDTO
    {
        public string FunctionInstanceIdentifier { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string AssetPair { get; set; }
        public int Period { get; set; }
        public CandleOperationMode CandleOperationMode { get; set; }
        public CandleTimeInterval CandleTimeInterval { get; set; }

        public FunctionParamsDTO() { }

        public FunctionParamsDTO(FunctionType functionType, DateTime startingDate, DateTime endingDate, 
                                    string assetPair, int period, CandleOperationMode candleOperationMode, CandleTimeInterval candleTimeInterval)
        {
            FunctionInstanceIdentifier = GetFunctionIdentifierByFunctionType(functionType);
            StartingDate = startingDate;
            EndingDate = endingDate;
            AssetPair = assetPair;
            Period = period;
            CandleOperationMode = candleOperationMode;
            CandleTimeInterval = candleTimeInterval;
        }

        public FunctionParamsDTO(FunctionType functionType, int startDaysOffset, int endDaysOffset, string assetPair, 
                                    int period, CandleOperationMode candleOperationMode, CandleTimeInterval candleTimeInterval)
        {
            var dateTimeNow = DateTime.Now;
            FunctionInstanceIdentifier = GetFunctionIdentifierByFunctionType(functionType);
            StartingDate = dateTimeNow.AddDays(startDaysOffset);
            EndingDate = dateTimeNow.AddDays(endDaysOffset);
            AssetPair = assetPair;
            Period = period;
            CandleOperationMode = candleOperationMode;
            CandleTimeInterval = candleTimeInterval;
        }

        private string GetFunctionIdentifierByFunctionType(FunctionType functionType)
        {
            switch (functionType)
            {
                case FunctionType.SMA_Short:
                    return "SMA_Short";
                case FunctionType.SMA_Long:
                    return "SMA_Long";
                case FunctionType.ADX:
                    return "ADX";
                case FunctionType.MACD:
                    return "MACD";
                default:
                    throw new NotImplementedException($"{functionType} is not defined yet!");
            }
        }
    }
}
