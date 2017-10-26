using AssetsData.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsData.DTOs.Assets
{
    public class AssetGroupDTO : BaseDTO
    {
        public bool ClientsCanCashInViaBankCards { get; set; }
        public bool IsIosDevice { get; set; }
        public string Name { get; set; }
        public bool SwiftDepositEnabled { get; set; }
    }
}
