using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XUnitTestCommon.Config;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace XUnitTestCommon.ServiceSettings
{
    public class NewServiceSettings
    {
        private static string devUrl = "SettingsServiceURL";
        private static string devToken = "SettingsServiceAccessToken";

        private static string testUrl = "SettingsServiceTestURL";
        private static string testToken = "SettingsServiceTestAccessToken";

        static JObject cfg = LocalConfig.GetLocalConfigJobject();

        public static JObject Settings()
        {
            string content = "";
            if (Environment.GetEnvironmentVariable("WalletApiUrl") == "dev" || Environment.GetEnvironmentVariable("WalletApiUrl") == null)
            {
                var response = Requests.For(cfg[devUrl].ToString()).Get(cfg[devToken].ToString()).Build().Execute();
                content = Requests.For(cfg[devUrl].ToString()).Get(cfg[devToken].ToString()).Build().Execute().Content;
                Console.WriteLine($"================= CONFIG URl: {cfg[devUrl]}{cfg[devToken]}  ===================");
                Console.WriteLine($"================= CONFIG STATUS CODE: {response.StatusCode}  ===================");
                Console.WriteLine($"================= CONFIG STATUS CODE: {response.Content}  ===================");
            }
            else
                content = Requests.For(cfg[testUrl].ToString()).Get(cfg[testToken].ToString()).Build().Execute().Content;
            if (string.IsNullOrEmpty(content))
                throw new Exception("Recieved empty config from settings server");

            return JObject.Parse(content);
        }
    }
}
