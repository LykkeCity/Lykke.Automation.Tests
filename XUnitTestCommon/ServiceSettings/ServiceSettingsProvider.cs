using LykkeAutomation.TestsCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests;
using TestsCore.TestsCore;

namespace TestsCore.ServiceSettings
{
    public class ServiceSettingsProvider
    {
        protected string URL = "https://settings-test.lykkex.net/";


        public void ServiceSettings<T>(string resource,ref T type)
        {
            var response = Requests.For(URL).Get(resource).Build().Execute();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                type = JsonConvert.DeserializeObject<T>(response.Content);      
        }
    }
}
