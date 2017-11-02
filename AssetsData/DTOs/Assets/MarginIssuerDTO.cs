using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class MarginIssuerDTO : BaseDTO
    {
        public string Id { get; set; }
        public string IconUrl { get; set; }
        public string Name { get; set; }
    }

    public class MarginIssuerReturnDTO : BaseDTO
    {
        public List<MarginIssuerDTO> Items { get; set; }
    }
}
