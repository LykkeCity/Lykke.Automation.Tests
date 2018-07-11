using ApiV2Data.DTOs;
using System;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs.InstanceData.Builders
{
    public class InstanceDataBuilder
    {
        private static AlgoMetaDataBuilder algoMetaDataBuilder;

        public static Dictionary<string, FunctionParamsDTO> FunctionsDictionary = new Dictionary<string, FunctionParamsDTO>();

        private static AlgoMetaDataBuilder GetAlgoMetaDataBuilder()
        {
            if (algoMetaDataBuilder == null)
            {
                algoMetaDataBuilder = new AlgoMetaDataBuilder();
            }

            return algoMetaDataBuilder;
        }

        public static InstanceDataDTO BuildInstanceData(AlgoDataDTO algoData, WalletDTO walletDTO, AlgoInstanceType instanceType, InstanceParameters instanceParameters, DaysOffsetDTO daysOffsetDTO, bool withInvalidAlgoId = false, bool withoutMetadaData = false)
        {
            AlgoMetaDataInformation algoMetaDataInformation = BuildAlgoMetaDataInformation(instanceParameters, daysOffsetDTO);

            if (withInvalidAlgoId)
            {
                algoData.Id = $"NonExistingAlgoId - {Helpers.GetFullUtcTimestamp()}";
            }

            return new InstanceDataDTO(algoData, walletDTO, instanceType, algoMetaDataInformation);
        }

        private static AlgoMetaDataInformation BuildAlgoMetaDataInformation(InstanceParameters instanceParameters, DaysOffsetDTO daysOffsetDTO)
        {
            FunctionParamsDTO smaShortFunctionParamsDTO = null;
            FunctionParamsDTO smaLongFunctionParamsDTO = null;
            FunctionParamsDTO adxFunctionParamsDTO = null;
            FunctionParamsDTO macdFunctionParamsDTO = null;

            AlgoParamsDTO algoParamsDTO = new AlgoParamsDTO(daysOffsetDTO.AlgoStartOffset, daysOffsetDTO.AlgoEndOffset,
                                                            instanceParameters.AssetPair, instanceParameters.TradedAsset,
                                                            instanceParameters.InstanceTradeVolume, instanceParameters.InstanceCandleInterval);

            // Create SMA_SHORT function parameters
            if (instanceParameters.InstanceFunctions.Contains(FunctionType.SMA_Short))
            {
                smaShortFunctionParamsDTO = new FunctionParamsDTO(FunctionType.SMA_Short, daysOffsetDTO.SmaShortStartOffset, daysOffsetDTO.SmaShortEndOffset,
                                                                  instanceParameters.AssetPair, instanceParameters.FunctionCapacity,
                                                                  instanceParameters.FunctionCandleOperationMode, instanceParameters.FunctionCandleInterval);

                // Add the function params to the functionsDictionary
                AddToFunctionsDictionary(FunctionType.SMA_Short, smaShortFunctionParamsDTO);
            }

            // Create SMA_Long function parameters
            if (instanceParameters.InstanceFunctions.Contains(FunctionType.SMA_Long))
            {
                smaLongFunctionParamsDTO = new FunctionParamsDTO(FunctionType.SMA_Long, daysOffsetDTO.SmaLongStartOffset, daysOffsetDTO.SmaLongEndOffset,
                                                                 instanceParameters.AssetPair, instanceParameters.FunctionCapacity,
                                                                 instanceParameters.FunctionCandleOperationMode, instanceParameters.FunctionCandleInterval);

                // Add the function params to the functionsDictionary
                AddToFunctionsDictionary(FunctionType.SMA_Long, smaLongFunctionParamsDTO);
            }

            // Create ADX function parameters
            if (instanceParameters.InstanceFunctions.Contains(FunctionType.ADX))
            {
                throw new NotImplementedException("Creating parameters for ADX function is not yet implemented");

                // Add the function params to the functionsDictionary
                //AddToFunctionsDictionary(FunctionType.ADX, adxFunctionParamsDTO);
            }

            // Create MACD function parameters
            if (instanceParameters.InstanceFunctions.Contains(FunctionType.MACD))
            {
                throw new NotImplementedException("Creating parameters for MACD function is not yet implemented");

                // Add the function params to the functionsDictionary
                //AddToFunctionsDictionary(FunctionType.MACD, macdFunctionParamsDTO);
            }

            // Build Algo Metadata
            return GetAlgoMetaDataBuilder()
                                        .CreateAlgoMetaData()
                                        .WithAlgoParams(algoParamsDTO)
                                        .WithFunction(smaShortFunctionParamsDTO, FunctionType.SMA_Short)
                                        .WithFunction(smaLongFunctionParamsDTO, FunctionType.SMA_Long)
                                        .WithFunction(adxFunctionParamsDTO, FunctionType.ADX)
                                        .WithFunction(macdFunctionParamsDTO, FunctionType.MACD)
                                        .Build();
        }

        private static void AddToFunctionsDictionary(FunctionType functionType, FunctionParamsDTO functionParamsDTO)
        {
            if (FunctionsDictionary.ContainsKey(functionType.ToString()))
            {
                FunctionsDictionary[functionType.ToString()] = functionParamsDTO;
            }
            else
            {
                FunctionsDictionary.Add(functionType.ToString(), functionParamsDTO);
            }
        }
    }
}
