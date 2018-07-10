using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class ClientWalletDataDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"WalletId: {Id}; WalletName: {Name}";
        }
    }
}
