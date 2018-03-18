using HFT.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace HFT
{
    public class Hft
    {
        public AssetPairs AssetPairs => new AssetPairs();
        public History History => new History();
        public IsAlive IsAlive => new IsAlive();
        public OrderBooks OrderBooks => new OrderBooks();
        public Orders Orders => new Orders();
        public Wallets Wallets => new Wallets();
    }
}
