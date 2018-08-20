using FIX.Client;
using LykkeAutomationPrivate.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XUnitTestCommon.AzureUtils;
using XUnitTestCommon.Tests;

namespace AFTests.FIX
{
    [NonParallelizable]
    public class FixBaseTest : BaseTest
    {
        protected WalletApi.Api.WalletApi walletApi = new WalletApi.Api.WalletApi();
        protected LykkeApi privateApi = new LykkeApi();

        protected static string JsonRepresentation(QuickFix.FIX44.Message message)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore};
            var json = JsonConvert.SerializeObject(message, settings);
            return json;
        }

        public static List<EntityProperty> GetValueFromAzure(string message)
        {
           return new AzureUtils(Init.LocalConfig()["AzureConnectionString"].ToString())
                    .GetCloudTable("FixGatewayMessagesLog")
                    .GetSearchResult("Message", message)
                    .GetCellsByKnownCellName("Message");
        }
    }

    public class Init
    {
        private static Lazy<JToken> lazy = new Lazy<JToken>(isThreadSafe: true);

        public static JToken LocalConfig()
        {
            if (lazy.IsValueCreated)
                return lazy.Value;

            var localConfig = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.WorkDirectory)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true).Build();

            var request = new RestRequest(localConfig.GetSection("SettingsServiceAccessToken").Value, Method.GET);
            var client = new RestClient(localConfig.GetSection("SettingsServiceURL").Value);

            var settings = JObject.Parse(client.Execute(request).Content);
            lazy = new Lazy<JToken>(() => settings["AutomatedFunctionalTests"]["FIX"], true);

            TestContext.Progress.WriteLine($"current appsettings.json file: {lazy.Value}");

            return lazy.Value;
        }
    }
}
