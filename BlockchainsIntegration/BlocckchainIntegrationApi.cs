using BlockchainsIntegration.LiteCoin.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainsIntegration.Api
{
    public class BlockchainApi
    {
        string BaseUrl;

        public BlockchainApi(string url = null)
        {
            BaseUrl = url;
        }

        public Address Address => new Address(BaseUrl);
        public Assets Assets => new Assets(BaseUrl);
        public Balances Balances => new Balances(BaseUrl);
        public IsAlive IsAlive => new IsAlive(BaseUrl);
        public Operations Operations => new Operations(BaseUrl);
        public Capabilities Capabilities => new Capabilities(BaseUrl);
        //public History History => new History(BaseUrl);
    }
}
