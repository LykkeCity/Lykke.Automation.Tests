using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class BaseAssetDTO: BaseDTO
    {
        public string BaseAssetId { get; set; }

        public BaseAssetDTO(string id)
        {
            this.BaseAssetId = id;
        }
    }    
}
