using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.Authentication
{
    public class UserExtended : User
    {
        public string ClientId { get; set; }
        public string TradingWalletId { get; set; }
    }
}
