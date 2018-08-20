using System.Collections.Generic;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class AlgoMetaDataInformation
    {
        public List<MetaDataParameter> Parameters { get; set; }
        public List<AlgoMetaDataFunction> Functions { get; set; }
    }
}
