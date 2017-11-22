using System;
using System.Collections.Generic;
using System.Text;

namespace ApiV2Data.DTOs
{
    public class WalletDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string ApiKey { get; set; }
    }

    public class WalletBalanceDTO : WalletDTO
    {
        public List<WBalanceDTO> Balances { get; set; }
    }

    public class WalletSingleBalanceDTO : WalletDTO
    {
        public WBalanceDTO Balances { get; set; }
    }

    public class WBalanceDTO
    {
        public string AssetId { get; set; }
        public double Balance { get; set; }
        public double Reserved { get; set; }
    }

    public class WalletCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class WalletCreateHFTDTO
    {
        public string ApiKey { get; set; }
        public string WalletId { get; set; }
    }
}
