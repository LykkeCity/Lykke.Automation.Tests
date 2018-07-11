using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class PredefinedValue
    {
        public string Key { get; set; }
        public int Value { get; set; }

        public PredefinedValue(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
