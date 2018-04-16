using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class AlgoMetaDataFunction
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string FunctionParameterType { get; set; }
        public IEnumerable<AlgoMetaDataParameter> Parameters { get; set; }
    }
}
