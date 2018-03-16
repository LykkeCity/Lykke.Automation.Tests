using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class DeployBinaryDTO
    {
        public string AlgoId { get; set;}
        public string InstanceId { get; set; }

        private string clientId = "e658abfc-1779-427c-8316-041a2deb1db8";

        public string  AlgoClientId
        {
            get
            {
                return clientId;
            }
            set
            {
                clientId = value;
            }
        }
    }
}
