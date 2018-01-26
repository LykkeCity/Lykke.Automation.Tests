using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Config;

namespace XUnitTestCommon.ServiceSettings
{
    public class SettingsTokenContainer
    {
        public static string PersonalDataToken { get { return LocalConfig.GetLocalConfig().PersonalDataService; } }
    }
}
