using BlockchainsIntegration.LiteCoin.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainsIntegration.Api
{
    public class BlockchainApi
    {
        string BaseUrl;

        private Address _Address;
        private Assets _Assets;
        private Balances _Balances;
        private IsAlive _IsAlive;
        private Operations _Operations;
        private Capabilities _Capabilities;
        private Testing _Testing;
        private Constants _Constants;

        public BlockchainApi(string url = null)
        {
            BaseUrl = url;
            _Address = new Address(BaseUrl);
            _Assets = new Assets(BaseUrl);
            _Balances = new Balances(BaseUrl);
            _IsAlive = new IsAlive(BaseUrl);
            _Operations = new Operations(BaseUrl);
            _Capabilities = new Capabilities(BaseUrl);
            _Testing = new Testing(BaseUrl);
            _Constants = new Constants(BaseUrl);
    }

        public Address Address { get { return _Address; } }
        public Assets Assets { get { return _Assets; } }
        public Balances Balances { get { return _Balances; } }
        public IsAlive IsAlive { get { return _IsAlive; } }
        public Operations Operations { get { return _Operations; } }
        public Capabilities Capabilities { get { return _Capabilities; } }
        public Testing Testing { get { return _Testing; } }
        public Constants Constants { get { return _Constants; } }
    }
}
