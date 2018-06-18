using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.FileProviders;
using XUnitTestCommon.Config;
using Lykke.SettingsReader;
using XUnitTestCommon.Settings;
using System.IO;

namespace XUnitTestCommon
{
    public sealed class ConfigBuilder
    {
        public IConfigurationRoot Config { get; }

        public IReloadingManager<AppSettings> ReloadingManager { get; }

        public ConfigBuilder()
        {
            LocalConfig localConfig = LocalConfig.GetLocalConfig();

            Config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            Config.Providers.First().Set("SettingsUrl", $"{localConfig.SettingsServiceURL}{localConfig.SettingsServiceAccessToken}");
            
            ReloadingManager = Config.LoadSettings<AppSettings>("SettingsUrl", false);
        }

        public static IReloadingManager<AppSettings> InitConfiguration()
        {
            LocalConfig localConfig = LocalConfig.GetLocalConfig();

            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            config.Providers.First().Set("SettingsUrl", $"{localConfig.SettingsServiceURL}{localConfig.SettingsServiceAccessToken}");

            return config.LoadSettings<AppSettings>("SettingsUrl", false);
        }
    }
}
