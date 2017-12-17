using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestsCore.ServiceSettings.SettingsModels
{
    public class PersonalDataServiceModel
    {
        [JsonProperty("PersonalDataServiceSettings")]
        public PersonalDataSettings PersonalDataSettings { get; set; }
    }

    public class PersonalDataSettings
    {
        public string ServiceUri { get; set; }
        public string ServiceExternalUri { get; set; }
        public string ApiKey { get; set; }
    }
}
