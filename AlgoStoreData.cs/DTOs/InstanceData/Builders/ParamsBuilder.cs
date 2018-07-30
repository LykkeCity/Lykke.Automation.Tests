using System;
using System.Collections.Generic;

namespace AlgoStoreData.DTOs.InstanceData.Builders
{
    class ParamsBuilder
    {
        private List<MetaDataParameter> parameters { get; set; }

        public ParamsBuilder CreateParameters()
        {
            parameters = new List<MetaDataParameter>();
            return this;
        }

        public ParamsBuilder WithParameterStartFrom(string paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("StartFrom", paramValue, "System.DateTime");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterEndOn(string paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("EndOn", paramValue, "System.DateTime");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterStartingDate(string paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("startingDate", paramValue, "System.DateTime");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterEndingDate(string paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("endingDate", paramValue, "System.DateTime");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterAssetPair(string paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("AssetPair", paramValue, "System.String");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterTradedAsset(string paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("TradedAsset", paramValue, "System.String");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterVolume(double paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("Volume", paramValue, "System.Double");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterPeriod(int paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("period", paramValue, "System.Int32");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterCandleOperationMode(CandleOperationMode paramValue)
        {
            var type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Abstractions.Core.Functions.FunctionParamsBase+CandleValue";
            MetaDataParameter metaDataParameter = new MetaDataParameter("candleOperationMode", paramValue, type);
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterFunctionInstanceIdentifier(string paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("FunctionInstanceIdentifier", paramValue, "System.String");
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterCandleInterval(CandleTimeInterval paramValue)
        {
            var type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Models.Enumerators.CandleTimeInterval";
            MetaDataParameter metaDataParameter = new MetaDataParameter("CandleInterval", paramValue, type);
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithParameterCandleTimeInterval(CandleTimeInterval paramValue)
        {
            var type = "Lykke.AlgoStore.CSharp.AlgoTemplate.Models.Enumerators.CandleTimeInterval";
            MetaDataParameter metaDataParameter = new MetaDataParameter("candleTimeInterval", paramValue, type);
            parameters.Add(metaDataParameter);

            return this;
        }

        public ParamsBuilder WithAdxPeriod(int paramValue)
        {
            MetaDataParameter metaDataParameter = new MetaDataParameter("AdxPeriod", paramValue, "System.Int32");
            parameters.Add(metaDataParameter);

            return this;
        }

        public List<MetaDataParameter> Build()
        {
            if (parameters.Count == 0)
            {
                throw new Exception("Use some of the 'With' methods to add parameters");
            }

            return parameters;
        }
    }
}
