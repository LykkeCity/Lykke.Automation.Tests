using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon;

namespace AlgoStoreData.DTOs
{
    public class InstanceDataDTO
    {
        public string AlgoId { get; set; }
        public string AlgoClientId { get; set; }
        public string WalletId { get; set; }
        public string InstanceName { get; set; }
        public AlgoMetaDataInformation AlgoMetaDataInformation { get; set; }
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
        public InstanceDataDTO returnInstanceDataDTO(string AlgoId)
        {
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = AlgoId,
                AlgoClientId = "e658abfc-1779-427c-8316-041a2deb1db8",
                WalletId = "50531d18-fab8-489b-a54c-2529e8a7e61e",
                InstanceName = Helpers.RandomString(13),
                AlgoMetaDataInformation = new AlgoMetaDataInformation()
                {
                    Parameters = new List<AlgoMetaDataParameter>()
                    {
                        new AlgoMetaDataParameter()
                        {
                           Key = "StartFrom",
                           Value = "10-02-2018",
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
                           Value = "86400",
                           Type  ="CandleTimeInterval",
                           PredefinedValues = new List<PredefinedValues>()
                           {
                               new PredefinedValues()
                               {
                                   Key = "Unspecified",
                                   Value = 0
                               },
                               new PredefinedValues()
                               {
                                   Key = "Second",
                                   Value = 1
                               },
                               new PredefinedValues()
                               {
                                   Key = "Minute",
                                   Value = 60
                               },
                               new PredefinedValues()
                               {
                                   Key = "5 Minutes",
                                   Value = 300
                               },
                               new PredefinedValues()
                               {
                                   Key = "15 Minutes",
                                   Value = 900
                               },
                               new PredefinedValues()
                               {
                                   Key = "30 Minutes",
                                   Value = 1800
                               },
                               new PredefinedValues()
                               {
                                   Key = "Hour",
                                   Value = 3600
                               },
                               new PredefinedValues()
                               {
                                   Key = "4 Hours",
                                   Value = 7200
                               },
                               new PredefinedValues()
                               {
                                   Key = "6 Hours",
                                   Value = 21600
                               },
                               new PredefinedValues()
                               {
                                   Key = "12 Hours",
                                   Value = 43200
                               },
                               new PredefinedValues()
                               {
                                   Key = "Day",
                                   Value = 86400
                               },
                               new PredefinedValues()
                               {
                                   Key = "Week",
                                   Value = 604800
                               },
                               new PredefinedValues()
                               {
                                   Key = "Month",
                                   Value = 3000000
                               }
                           }
                        },
                    },
                    Functions = new List<AlgoMetaDataFunction>()
                        { new AlgoMetaDataFunction()
                            {
                                Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Services.Functions.SMA.SmaFunction",
                                Id = "SMA_Short",
                                FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Services.Functions.SMA.SmaParameters",
                                Parameters = new List<AlgoMetaDataParameter>
                                {
                                    new AlgoMetaDataParameter
                                    {
                                        Key = "StartingDate",
                                        Value = "10-05-2018",
                                        Type = "DateTime"
                                    },
                                    new AlgoMetaDataParameter
                                    {
                                        Key = "FunctionInstanceIdentifier",
                                        Value = "SMA_Short",
                                        Type = "String"
                                    },
                                    new AlgoMetaDataParameter
                                    {
                                        Key = "CandleTimeInterval",
                                        Value = "86400",
                                        Type = "CandleTimeInterval",
                                        PredefinedValues = new List<PredefinedValues>()
                                        {
                                            new PredefinedValues()
                                                {
                                                     Key = "Unspecified",
                                                     Value = 0
                                                },
                                            new PredefinedValues()
                                                {
                                                     Key = "Second",
                                                    Value = 1
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Minute",
                                                    Value = 60
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "5 Minutes",
                                                    Value = 300
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "15 Minutes",
                                                   Value = 900
                                                },
                                            new PredefinedValues()
                                                {
                                                     Key = "30 Minutes",
                                                     Value = 1800
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Hour",
                                                    Value = 3600
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "4 Hours",
                                                    Value = 7200
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "6 Hours",
                                                    Value = 21600
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "12 Hours",
                                                    Value = 43200
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Day",
                                                    Value = 86400
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Week",
                                                    Value = 604800
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Month",
                                                    Value = 3000000
                                                },
                                        }
                                    },
                                    new AlgoMetaDataParameter
                                    {
                                        Key = "CandleOperationMode",
                                        Value = "1",
                                        Type = "CandleValue",
                                        PredefinedValues = new List<PredefinedValues>()
                                        {
                                            new PredefinedValues()
                                            {
                                                Key = "OPEN",
                                                Value = 0
                                            },
                                            new PredefinedValues()
                                            {
                                                Key = "CLOSE",
                                                Value = 1
                                            },
                                            new PredefinedValues()
                                            {
                                                Key = "LOW",
                                                Value = 2
                                            },
                                            new PredefinedValues()
                                            {
                                                Key = "HIGH",
                                                Value = 3
                                            }
                                        }
                                    },
                                }
                            },
                        new AlgoMetaDataFunction
                            {
                                Type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Services.Functions.SMA.SmaFunction",
                                Id = "SMA_Long",
                                FunctionParameterType = "Lykke.AlgoStore.CSharp.AlgoTemplate.Services.Functions.SMA.SmaParameters",
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
                                        Value = "10-05-2018",
                                        Type = "DateTime"
                                    },
                                    new AlgoMetaDataParameter()
                                    {
                                        Key = "CandleTimeInterval",
                                        Value = "86400",
                                        Type = "CandleTimeInterval",
                                        PredefinedValues = new List<PredefinedValues>()
                                        {
                                             new PredefinedValues()
                                                {
                                                     Key = "Unspecified",
                                                     Value = 0
                                                },
                                            new PredefinedValues()
                                                {
                                                     Key = "Second",
                                                    Value = 1
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Minute",
                                                    Value = 60
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "5 Minutes",
                                                    Value = 300
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "15 Minutes",
                                                   Value = 900
                                                },
                                            new PredefinedValues()
                                                {
                                                     Key = "30 Minutes",
                                                     Value = 1800
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Hour",
                                                    Value = 3600
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "4 Hours",
                                                    Value = 7200
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "6 Hours",
                                                    Value = 21600
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "12 Hours",
                                                    Value = 43200
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Day",
                                                    Value = 86400
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Week",
                                                    Value = 604800
                                                },
                                            new PredefinedValues()
                                                {
                                                    Key = "Month",
                                                    Value = 3000000
                                                },
                                        }
                                    },
                                    new AlgoMetaDataParameter()
                                    {
                                        Key = "CandleOperationMode",
                                        Value = "1",
                                        Type = "CandleValue",
                                        PredefinedValues = new List<PredefinedValues>()
                                        {
                                             new PredefinedValues()
                                            {
                                                Key = "OPEN",
                                                Value = 0
                                            },
                                            new PredefinedValues()
                                            {
                                                Key = "CLOSE",
                                                Value = 1
                                            },
                                            new PredefinedValues()
                                            {
                                                Key = "LOW",
                                                Value = 2
                                            },
                                            new PredefinedValues()
                                            {
                                                Key = "HIGH",
                                                Value = 3
                                            }
                                        }
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
