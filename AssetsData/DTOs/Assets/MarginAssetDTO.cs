using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class MarginAssetDTO : BaseDTO
    {
        public int Accuracy { get; set; }
        public double DustLimit { get; set; }
        public string Id { get; set; }
        public string IdIssuer { get; set; }
        public double Multiplier { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
