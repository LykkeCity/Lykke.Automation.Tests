using System.Collections.Generic;

namespace AssetsData.DTOs.Assets
{
    public class Erc20TokenDto : BaseDTO
    {
        public string Id { get; set; }

        public string AssetId { get; set; }

        public string Address { get; set; }

        public string BlockHash { get; set; }

        public int BlockTimestamp { get; set; }

        public string DeployerAddress { get; set; }

        public int? TokenDecimals { get; set; }

        public string TokenName { get; set; }

        public string TokenSymbol { get; set; }

        public string TokenTotalSupply { get; set; }

        public string TransactionHash { get; set; }
    }

    public class Erc20TokenItemsDto : BaseDTO
    {
        public List<Erc20TokenDto> Items { get; set; }
    }
}