using ApiV2Data.DTOs;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs
{
    public class InstanceDataDTO
    {
        public string InstanceId { get; set; }
        public string AlgoId { get; set; }
        public string AlgoClientId { get; set; }
        public string WalletId { get; set; }
        public string InstanceName { get; set; }
        public AlgoInstanceType AlgoInstanceType { get; set; }
        public AlgoMetaDataInformation AlgoMetaDataInformation { get; set; }
        public double? FakeTradingTradingAssetBalance { get; set; }
        public double? FakeTradingAssetTwoBalance { get; set; }
    }

    public class AlgoMetaDataInformation
    {
        public IEnumerable<AlgoMetaDataParameter> Parameters { get; set; }
        public IEnumerable<AlgoMetaDataFunction> Functions { get; set; }
    }

    public class AlgoMetaDataParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public IEnumerable<PredefinedValues> PredefinedValues { get; set; }
    }

    public class PredefinedValues
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }

    public class AlgoMetaDataFunction
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string FunctionParameterType { get; set; }
        public IEnumerable<AlgoMetaDataParameter> Parameters { get; set; }
    }

    public class GetPopulatedInstanceDataDTO
    {
        public static InstanceDataDTO ReturnInstanceDataDTO(string AlgoId, WalletDTO walletDTO, AlgoInstanceType algoInstanceType)
        {
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = AlgoId,
                AlgoClientId = "e658abfc-1779-427c-8316-041a2deb1db8",
                InstanceName = $"{algoInstanceType}{GlobalConstants.AutoTest}_AlgoIntanceName_{Helpers.GetFullUtcTimestamp()}",
                AlgoInstanceType = algoInstanceType,
                AlgoMetaDataInformation = new AlgoMetaDataInformation()
                {
                    Parameters = new List<AlgoMetaDataParameter>()
                    {
                        new AlgoMetaDataParameter()
                        {
                           Key = "StartFrom",
                           Value = "2018-04-01T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "EndOn",
                           Value = "2019-05-16T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "ADXTreshhold",
                           Value = "15",
                           Type  ="int"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "AssetPair",
                           Value = "BTCEUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "TradedAsset",
                           Value = "EUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "Volume",
                           Value = "2",
                           Type  ="double"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "CandleInterval",
                           Value = "60",
                           Type  ="CandleTimeInterval"
                        }
                    },
                    Functions = new List<AlgoMetaDataFunction>()
                    {
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Short",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2016-04-01T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2018-04-01T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                    new AlgoMetaDataParameter
                                {
                                    Key = "Capacity",
                                    Value = "5",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Short",
                                    Type = "String"
                                },
                                    new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "TradedAsset",
                                    Value = "EUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "604800",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Long",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>()
                            {
                                new AlgoMetaDataParameter()
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Long",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "StartingDate",
                                    Value = "2016-04-01T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2018-04-01T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                    new AlgoMetaDataParameter()
                                {
                                    Key = "Capacity",
                                    Value = "10",
                                    Type = "int"
                                },
                                    new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "604800",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxFunction",
                            Id = "ADX",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "ADX",
                                    Type = "String"
                                },
                                    new AlgoMetaDataParameter
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2016-04-01T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2018-04-01T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "604800",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AdxPeriod",
                                    Value = "14",
                                    Type = "int"
                                }
                            }
                        }
                    }
                }
            };

            if (algoInstanceType == AlgoInstanceType.Live)
            {
                instanceForAlgo.WalletId = walletDTO.Id;
                instanceForAlgo.FakeTradingTradingAssetBalance = null;
                instanceForAlgo.FakeTradingAssetTwoBalance = null;
            } else
            {
                instanceForAlgo.WalletId = null;
                instanceForAlgo.FakeTradingTradingAssetBalance = 8192;
                instanceForAlgo.FakeTradingAssetTwoBalance = 2.048;
            }

            return instanceForAlgo;
        }




        public static InstanceDataDTO returnInstanceDataDTOInvalidAssetPair(string AlgoId, WalletDTO walletDTO)
        {
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = AlgoId,
                AlgoClientId = "e658abfc-1779-427c-8316-041a2deb1db8",
                WalletId = walletDTO.Id,
                InstanceName = $"{ GlobalConstants.AutoTest }_AlgoIntanceName_{Helpers.GetFullUtcTimestamp()}",
                AlgoMetaDataInformation = new AlgoMetaDataInformation()
                {
                    Parameters = new List<AlgoMetaDataParameter>()
                    {
                        new AlgoMetaDataParameter()
                        {
                           Key = "StartFrom",
                           Value = "2018-02-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "EndOn",
                           Value = "2019-05-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "ADXTreshhold",
                           Value = "15",
                           Type  ="int"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "AssetPair",
                           Value = "BTCBTC",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "TradedAsset",
                           Value = "EUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "Volume",
                           Value = "2",
                           Type  ="double"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "CandleInterval",
                           Value = "60",
                           Type  ="CandleTimeInterval"
                        }
                    },
                    Functions = new List<AlgoMetaDataFunction>()
                    {
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Short",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "Capacity",
                                    Value = "5",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Short",
                                    Type = "String"
                                },
                                    new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCBTC",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "TradedAsset",
                                    Value = "EUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Long",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>()
                            {
                                new AlgoMetaDataParameter()
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Long",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "Capacity",
                                    Value = "10",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCBTC",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxFunction",
                            Id = "ADX",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "ADX",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AssetPair",
                                    Value = "BTCBTC",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-02-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AdxPeriod",
                                    Value = "14",
                                    Type = "int"
                                }
                            }
                        }
                    }
                }
            };

            return instanceForAlgo;
        }




        public static InstanceDataDTO returnInstanceDataDTOInvalidTradedAsset(string AlgoId, WalletDTO walletDTO)
        {
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = AlgoId,
                AlgoClientId = "e658abfc-1779-427c-8316-041a2deb1db8",
                WalletId = walletDTO.Id,
                InstanceName = $"{ GlobalConstants.AutoTest }_AlgoIntanceName_{Helpers.GetFullUtcTimestamp()}",
                AlgoMetaDataInformation = new AlgoMetaDataInformation()
                {
                    Parameters = new List<AlgoMetaDataParameter>()
                    {
                        new AlgoMetaDataParameter()
                        {
                           Key = "StartFrom",
                           Value = "2018-02-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "EndOn",
                           Value = "2019-05-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "ADXTreshhold",
                           Value = "15",
                           Type  ="int"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "AssetPair",
                           Value = "BTCEUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "TradedAsset",
                           Value = "USD",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "Volume",
                           Value = "2",
                           Type  ="double"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "CandleInterval",
                           Value = "60",
                           Type  ="CandleTimeInterval"
                        }
                    },
                    Functions = new List<AlgoMetaDataFunction>()
                    {
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Short",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "Capacity",
                                    Value = "5",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Short",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "TradedAsset",
                                    Value = "USD",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Long",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>()
                            {
                                new AlgoMetaDataParameter()
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Long",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "Capacity",
                                    Value = "10",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxFunction",
                            Id = "ADX",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "ADX",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-02-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AdxPeriod",
                                    Value = "14",
                                    Type = "int"
                                }
                            }
                        }
                    }
                }
            };

            return instanceForAlgo;
        }



        public static InstanceDataDTO returnInstanceDataDTOInvalidVolume(string AlgoId, WalletDTO walletDTO)
        {
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = AlgoId,
                AlgoClientId = "e658abfc-1779-427c-8316-041a2deb1db8",
                WalletId = walletDTO.Id,
                InstanceName = $"{ GlobalConstants.AutoTest }_AlgoIntanceName_{Helpers.GetFullUtcTimestamp()}",
                AlgoMetaDataInformation = new AlgoMetaDataInformation()
                {
                    Parameters = new List<AlgoMetaDataParameter>()
                    {
                        new AlgoMetaDataParameter()
                        {
                           Key = "StartFrom",
                           Value = "2018-02-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "EndOn",
                           Value = "2019-05-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "ADXTreshhold",
                           Value = "15",
                           Type  ="int"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "AssetPair",
                           Value = "BTCEUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "TradedAsset",
                           Value = "EUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "Volume",
                           Value = "0",
                           Type  ="double"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "CandleInterval",
                           Value = "60",
                           Type  ="CandleTimeInterval"
                        }
                    },
                    Functions = new List<AlgoMetaDataFunction>()
                    {
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Short",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "Capacity",
                                    Value = "5",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Short",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "TradedAsset",
                                    Value = "EUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Long",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>()
                            {
                                new AlgoMetaDataParameter()
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Long",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "Capacity",
                                    Value = "10",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxFunction",
                            Id = "ADX",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "ADX",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-02-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AdxPeriod",
                                    Value = "14",
                                    Type = "int"
                                }
                            }
                        }
                    }
                }
            };

            return instanceForAlgo;
        }




        public static InstanceDataDTO returnInstanceDataDTONegativeVolume(string AlgoId, WalletDTO walletDTO)
        {
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = AlgoId,
                AlgoClientId = "e658abfc-1779-427c-8316-041a2deb1db8",
                WalletId = walletDTO.Id,
                InstanceName = $"{ GlobalConstants.AutoTest }_AlgoIntanceName_{Helpers.GetFullUtcTimestamp()}",
                AlgoMetaDataInformation = new AlgoMetaDataInformation()
                {
                    Parameters = new List<AlgoMetaDataParameter>()
                    {
                        new AlgoMetaDataParameter()
                        {
                           Key = "StartFrom",
                           Value = "2018-02-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "EndOn",
                           Value = "2019-05-10T00:00:00.000Z",
                           Type  ="DateTime"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "ADXTreshhold",
                           Value = "15",
                           Type  ="int"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "AssetPair",
                           Value = "BTCEUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "TradedAsset",
                           Value = "EUR",
                           Type  ="String"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "Volume",
                           Value = "-2",
                           Type  ="double"
                        },
                        new AlgoMetaDataParameter()
                        {
                           Key = "CandleInterval",
                           Value = "60",
                           Type  ="CandleTimeInterval"
                        }
                    },
                    Functions = new List<AlgoMetaDataFunction>()
                    {
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Short",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "Capacity",
                                    Value = "5",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Short",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "TradedAsset",
                                    Value = "EUR",
                                    Type  ="String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaFunction",
                            Id = "SMA_Long",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.SMA.SmaParameters",
                            Parameters = new List<AlgoMetaDataParameter>()
                            {
                                new AlgoMetaDataParameter()
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "SMA_Long",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "StartingDate",
                                    Value = "2018-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "Capacity",
                                    Value = "10",
                                    Type = "int"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter()
                                {
                                    Key = "CandleOperationMode",
                                    Value = "1",
                                    Type = "CandleValue"
                                }
                            }
                        },
                        new AlgoMetaDataFunction()
                        {
                            Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxFunction",
                            Id = "ADX",
                            FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Functions.ADX.AdxParameters",
                            Parameters = new List<AlgoMetaDataParameter>
                            {
                                new AlgoMetaDataParameter
                                {
                                    Key = "FunctionInstanceIdentifier",
                                    Value = "ADX",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AssetPair",
                                    Value = "BTCEUR",
                                    Type = "String"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "StartingDate",
                                    Value = "2018-02-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "EndingDate",
                                    Value = "2050-05-10T00:00:00.000Z",
                                    Type = "DateTime"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "CandleTimeInterval",
                                    Value = "60",
                                    Type = "CandleTimeInterval"
                                },
                                new AlgoMetaDataParameter
                                {
                                    Key = "AdxPeriod",
                                    Value = "14",
                                    Type = "int"
                                }
                            }
                        }
                    }
                }
            };

            return instanceForAlgo;
        }
    }
}
