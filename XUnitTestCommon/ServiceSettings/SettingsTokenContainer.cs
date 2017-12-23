using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.ServiceSettings
{
    public class SettingsTokenContainer
    {
        public static string PersonalDataToken { get { return Environment.GetEnvironmentVariable("PersonalDataService"); } }
    }
}
