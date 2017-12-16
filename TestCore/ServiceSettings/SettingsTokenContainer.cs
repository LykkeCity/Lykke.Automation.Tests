using System;
using System.Collections.Generic;
using System.Text;

namespace TestsCore.ServiceSettings
{
    public class SettingsTokenContainer
    {
        public static string PersonalDataToken { get { return Environment.GetEnvironmentVariable("PersonalDataService"); } }
    }
}
