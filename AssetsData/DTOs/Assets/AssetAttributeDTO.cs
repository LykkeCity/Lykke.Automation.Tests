using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class AssetAttributeDTO : BaseDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AssetAttributeIdentityDTO : BaseDTO
    {
        public string AssetId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AssetAttributesReturnDTO : BaseDTO
    {
        public string AssetID { get; set; }
        public List<AssetAttributeDTO> Attributes { get; set; }
        public object errorResponse { get; set; }
    }
}
