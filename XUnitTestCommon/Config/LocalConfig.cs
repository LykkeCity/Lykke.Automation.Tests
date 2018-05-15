using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XUnitTestCommon.Config
{
    public class LocalConfig
    {
            public string SettingsServiceURL { get; set; }
            public string SettingsServiceAccessToken { get; set; }
            public string SettingsServiceRootItemName { get; set; }
            public string SettingsServiceTestItemName { get; set; }
            public string AzureDeltaSpread { get; set; }
            public string PersonalDataService { get; set; }

        private LocalConfig() { }

        public static LocalConfig GetLocalConfig()
        {
            var json = File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "Config.json"));
            return JsonConvert.DeserializeObject<LocalConfig>(json);
        }

        public static JObject GetLocalConfigJobject()
        {
            var json = File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "Config.json"));
            return JObject.Parse(json); 
        }
    }
}
