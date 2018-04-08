using FIX.Client;
using LykkeAutomationPrivate.Api;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XUnitTestCommon.Tests;

namespace AFTests.FIX
{
    [NonParallelizable]
    public class FixBaseTest : BaseTest
    {
        protected FixClient fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
        protected WalletApi.Api.WalletApi walletApi = new WalletApi.Api.WalletApi();
        protected LykkeApi privateApi = new LykkeApi();


        [OneTimeSetUp]
        public void SetUp()
        {
            fixClient.Start();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            fixClient.Stop();
        }

        protected static string JsonRepresentation(QuickFix.FIX44.Message message)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore};
            var json = JsonConvert.SerializeObject(message, settings);
            return json;
        }
    }


    public class Init
    {
        public static IConfigurationRoot LocalConfig()
        {
            if (!File.Exists(Path.Combine(TestContext.CurrentContext.WorkDirectory, "FIX/appsettings.json")))
                File.WriteAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "FIX/appsettings.json"), Environment.GetEnvironmentVariable("fixappjson"));

            var builder = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.WorkDirectory)
                .AddJsonFile("FIX/appsettings.json", optional: false, reloadOnChange: true);
           return builder.Build();       
        }
    }
}
