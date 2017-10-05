using System;
using System.Collections.Generic;
using System.Text;

namespace FirstXUnitTest.DTOs.Assets
{
    public class AssetExtendedDTO : BaseDTO
    {
        public AssetDTO Asset { get; set; }
        public AssetDescriptionDTO Description { get; set; }
        public AssetCategoryDTO Category { get; set; }
        public AssetAttributesReturnDTO Attributes { get; set; }
    }

    public class AssetExtendedReturnDTO : BaseDTO
    {
        public List<AssetExtendedDTO> Assets { get; set; }
    }
}
