using System;
using System.Collections.Generic;
using System.Text;

namespace ApiV2Data.DTOs
{
    public class WalletDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }

    public class WalletCreateDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
