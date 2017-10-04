using Microsoft.Extensions.Configuration;
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
        private readonly Lazy<IConfigurationRoot> lazyLocal;

        public IConfigurationRoot LocalConfig { get { return lazyLocal.Value; } }

        private readonly Lazy<IConfigurationRoot> lazy;

        public IConfigurationRoot Config { get { return lazy.Value; } }

        public ConfigBuilder(string TestItemName)
        {
            this.lazyLocal = new Lazy<IConfigurationRoot>(() => new ConfigurationBuilder().AddJsonFile("Config.json").Build());
            this.lazy = new Lazy<IConfigurationRoot>(() => new ConfigurationBuilder()
           .AddHttpJsonConfig(
               LocalConfig["SettingsServiceURL"],
               LocalConfig["SettingsServiceAccessToken"],
               LocalConfig["SettingsServiceRootItemName"],
               TestItemName
               ).Build());
        }

    }
}
