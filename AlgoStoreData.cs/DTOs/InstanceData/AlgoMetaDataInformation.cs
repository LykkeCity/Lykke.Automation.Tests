using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class AlgoMetaDataInformation
    {
        public IEnumerable<AlgoMetaDataParameter> Parameters { get; set; }
        public IEnumerable<AlgoMetaDataFunction> Functions { get; set; }
    }
}
