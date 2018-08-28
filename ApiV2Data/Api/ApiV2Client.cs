using System;
using System.Collections.Generic;
using System.Text;

namespace ApiV2Data.Api
{
    public class ApiV2Client
    {
        public Client Client => new Client();
        public Operations Operations => new Operations();
        public Wallets wallets => new Wallets();
    }
}
