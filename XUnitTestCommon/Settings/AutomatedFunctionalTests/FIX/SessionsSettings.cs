using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests.FIX
{
    public class SessionsSettings
    {
        public SessionTypeSettings QuoteSession { get; set; }
        public SessionTypeSettings TradeSession { get; set; }
    }
}
