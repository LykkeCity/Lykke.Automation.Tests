using Lykke.SettingsReader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests
{
    public class BalancesSettings : IAppSettings
    {
        public string UrlPefix { get; set; }
        public string BaseUrl { get; set; }
        [Optional] public int AuthTokenTimeout { get; set; }
        [Optional] public string AuthPath { get; set; }
        [Optional] public string RegisterPath { get; set; }
        [Optional] public string BaseUrlAuth { get; set; }
        [Optional] public string BaseUrlRegister { get; set; }
        [Optional] public string AuthEmail { get; set; }
        [Optional] public string AuthPassword { get; set; }
        public bool IsHttps { get; set; }
        public String DictionariesConnectionString { get; set; }
        public String MainConnectionString { get; set; }
        public String CacheExpirationSeconds { get; set; }
    }
}
