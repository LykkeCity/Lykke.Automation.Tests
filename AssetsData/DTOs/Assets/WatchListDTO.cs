using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class WatchListDTO : BaseDTO
    {
        public List<string> AssetIds { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool ReadOnly { get; set; }
    }
}
