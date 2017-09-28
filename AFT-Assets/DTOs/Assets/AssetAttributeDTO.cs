using System;
using System.Collections.Generic;
using System.Text;

namespace FirstXUnitTest.DTOs.Assets
{
    class AssetAttributeDTO : BaseDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    class AssetAttributesReturnDTO : BaseDTO
    {
        public string AssetID { get; set; }
        public List<AssetAttributeDTO> Attributes { get; set; }
        public object errorResponse { get; set; }
    }
}
