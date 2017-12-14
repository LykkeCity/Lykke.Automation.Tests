using System;
using System.Collections.Generic;
using System.Text;

namespace BalancesData.DTOs
{
    public class BalanceDTO
    {
        public string AssetId { get; set; }
        public double Balance { get; set; }
        public double Reserved { get; set; }
    }
}
