using LykkeAutomation.TestsCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TestsCore.ApiRestClient
{
    public abstract class RestApi
    {
        protected RestClientWrapper client;
        private string BaseURL = "https://payapi-test.lykkex.net/api";

        protected static bool firstUse = true;

        public RestApi()
        {
            client = new RestClientWrapper(BaseURL);
            SetLocalProxy();
            if (firstUse)
            {
                firstUse = false;
                SetAllureProperties();         
            }
        }

        public RestApi(string BaseURL)
        {
            this.BaseURL = BaseURL;
            client = new RestClientWrapper(BaseURL);
            SetLocalProxy();
            if (firstUse)
            {
                firstUse = false;
                SetAllureProperties();             
            }
        }

        private void SetLocalProxy()
        {
            if (Environment.OSVersion.ToString().ToLower().Contains("windows"))
                client.Proxy = new WebProxy("127.0.0.1", 8888);
        }

        public abstract void SetAllureProperties();
    }
}
