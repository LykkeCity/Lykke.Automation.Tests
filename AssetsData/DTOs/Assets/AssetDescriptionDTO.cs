using System;
using System.Collections.Generic;
using System.Text;

namespace FirstXUnitTest.DTOs.Assets
{
    public class AssetDescriptionDTO : BaseDTO
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public string AssetClass { get; set; }
        public string Description { get; set; }
        public string IssuerName { get; set; }
        public string NumberOfCoins { get; set; }
        public string MarketCapitalization { get; set; }
        public int? PopIndex { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string FullName { get; set; }
    }

    //public class AssetDescriptionReturnDTO : BaseDTO
    //{
    //    public List<AssetDescriptionDTO> Descriptions { get; set; }
    //    public object errorResponse { get; set; }
    //}

    public class AssetDescriptionBodyParamDTO : BaseDTO
    {
        public List<string> Ids { get; set; }

        public AssetDescriptionBodyParamDTO()
        {
            Ids = new List<string>();
        }
    }
}
