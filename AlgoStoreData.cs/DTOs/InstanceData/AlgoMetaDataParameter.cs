using System;
using System.Collections.Generic;
using XUnitTestCommon.Utils;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class AlgoMetaDataParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public List<PredefinedValue> PredefinedValues { get; set; }

        public static AlgoMetaDataParameter CreateInstance<T>(string key, string value, string type)
        {
            AlgoMetaDataParameter algoMetaDataParameter = new AlgoMetaDataParameter
            {
                Key = key,
                Value = value,
                Type = type
            };

            return algoMetaDataParameter;
        }
    }
}
