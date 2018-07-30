using System;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs.InstanceData.Builders
{
    public class AlgoMetaDataBuilder
    {
        private AlgoMetaDataInformation algoMetaDataInformation;

        private static ParamsBuilder paramBuilder;

        private static ParamsBuilder GetParamBuilder()
        {
            if (paramBuilder == null)
            {
                paramBuilder = new ParamsBuilder();
            }

            return paramBuilder;
        }

        public AlgoMetaDataBuilder CreateAlgoMetaData()
        {
            algoMetaDataInformation = new AlgoMetaDataInformation();
            algoMetaDataInformation.Functions = new List<AlgoMetaDataFunction>();
            return this;
        }

        public AlgoMetaDataBuilder WithAlgoParams(AlgoParamsDTO algoParams)
        {
            algoMetaDataInformation.Parameters = GetParamBuilder().CreateParameters()
                    .WithParameterStartFrom(algoParams.StartFrom.ToString(GlobalConstants.ISO_8601_DATE_FORMAT))
                    .WithParameterEndOn(algoParams.EndOn.ToString(GlobalConstants.ISO_8601_DATE_FORMAT))
                    .WithParameterAssetPair(algoParams.AssetPair)
                    .WithParameterTradedAsset(algoParams.TradedAsset)
                    .WithParameterVolume(algoParams.Volume)
                    .WithParameterCandleInterval(algoParams.CandleInterval)
                    .Build();
            return this;
        }

        public AlgoMetaDataBuilder WithFunction(FunctionParamsDTO functionParams, FunctionType functionType)
        {
            if (functionParams != null)
            {
                AlgoMetaDataFunction functionMetaData = new AlgoMetaDataFunction(functionType)
                {
                    Id = functionParams.FunctionInstanceIdentifier,
                    Parameters = GetParamBuilder().CreateParameters()
                    //.WithParameterFunctionInstanceIdentifier(functionParams.FunctionInstanceIdentifier)
                    .WithParameterStartingDate(functionParams.StartingDate.ToString(GlobalConstants.ISO_8601_DATE_FORMAT))
                    .WithParameterEndingDate(functionParams.EndingDate.ToString(GlobalConstants.ISO_8601_DATE_FORMAT))
                    .WithParameterAssetPair(functionParams.AssetPair)
                    .WithParameterPeriod(functionParams.Period)
                    .WithParameterCandleOperationMode(functionParams.CandleOperationMode)
                    .WithParameterCandleTimeInterval(functionParams.CandleTimeInterval)
                    .Build()
                };

                algoMetaDataInformation.Functions.Add(functionMetaData);
            }
            return this;
        }

        public AlgoMetaDataInformation Build()
        {
            if (algoMetaDataInformation.Parameters.Count == 0 || algoMetaDataInformation.Functions.Count == 0)
            {
                throw new Exception("Use some of the 'With' methods to add algo meta data");
            }

            return algoMetaDataInformation;
        }
    }
}
