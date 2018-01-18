using BlockchainsIntegration.LiteCoin.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainsIntegration.LiteCoin
{
    public class LitecoinApi
    {
        public Address Address => new Address();
        public Assets Assets => new Assets();
        public Balances Balances => new Balances();
        public IsAlive IsAlive => new IsAlive();
        public Operations Operations => new Operations();
    }
}
