using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests.FIX
{
    public class SessionTypeSettings
    {
        public String SenderCompID { get; set; }
        public String TargetCompID { get; set; }
        public List<String> FixConfiguration { get; set; }
    }
}
