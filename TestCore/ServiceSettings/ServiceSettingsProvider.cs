using LykkeAutomation.TestsCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;
using TestsCore.TestsCore;

namespace TestsCore.ServiceSettings
{
    public class ServiceSettingsProvider : RestApi
    {
        public ServiceSettingsProvider() : base("https://settings-test.lykkex.net/")
        {
        }

        public void ServiceSettings<T>(string resource,ref T type)
        {
            var request = new RestRequest(resource, Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                type = JsonConvert.DeserializeObject<T>(response.Content);      
        }

        public override void SetAllureProperties()
        {
            firstUse = true;
        }
    }
}
