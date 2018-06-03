using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class StopBinaryDTO
    {
        public string AlgoId { get; set; }
        public string InstanceId { get; set; }

        public string AlgoClientId { get; set; } = "e658abfc-1779-427c-8316-041a2deb1db8";
    }

    public class StopBinaryResponseDTO
    {
        public string Status { get; set; }
    }
}
