using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests.FIX
{
    public class TestClientSettings
    {
        public String ServiceUrl { get; set; }
        public CredentialsSettings Credentials { get; set; }
        public SessionsSettings Sessions { get; set; }
    }
}
