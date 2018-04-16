using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs.InstanceData
{
    public class InstanceDataDTO
    {
        public string InstanceId { get; set; }
        public string AlgoId { get; set; }
        public string AlgoClientId { get; set; }
        public string WalletId { get; set; }
        public string InstanceName { get; set; }
        public AlgoMetaDataInformation AlgoMetaDataInformation { get; set; }
    }
}
