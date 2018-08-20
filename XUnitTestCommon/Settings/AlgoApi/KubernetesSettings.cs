using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AlgoApi
{
    public class KubernetesSettings
    {
        public String Url { get; set; }
        public String BasicAuthenticationValue { get; set; }
        public String CertificateHash { get; set; }
    }
}
