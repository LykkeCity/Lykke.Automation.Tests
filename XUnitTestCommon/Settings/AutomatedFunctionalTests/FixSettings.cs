using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Settings.AutomatedFunctionalTests.FIX;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests
{
    public class FixSettings
    {
        public String AzureConnectionString { get; set; }
        public String User { get; set; }
        public String Pass { get; set; }
        public TestClientSettings TestClient { get; set; }
    }
}
