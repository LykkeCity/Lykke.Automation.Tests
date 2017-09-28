using Microsoft.Extensions.Configuration;
using Lykke.SettingsReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.FileProviders;
using XUnitTestCommon.Config;

namespace XUnitTestCommon
{
    public sealed class ConfigBuilder
    {
        private ConfigBuilder() { }

        private static readonly Lazy<IConfigurationRoot> lazyLocal =
            new Lazy<IConfigurationRoot>(() => new ConfigurationBuilder().AddJsonFile("Config.json").Build());

        public static IConfigurationRoot LocalConfig { get { return lazyLocal.Value; } }

        private static readonly Lazy<IConfigurationRoot> lazy =
           new Lazy<IConfigurationRoot>(() => new ConfigurationBuilder()
           .AddHttpJsonConfig(
               LocalConfig["SettingsServiceURL"], 
               LocalConfig["SettingsServiceAccessToken"],
               LocalConfig["SettingsServiceRootItemName"],
               LocalConfig["SettingsServiceTestItemName"]
               ).Build());

        public static IConfigurationRoot Config { get { return lazy.Value; } }

    }
}
