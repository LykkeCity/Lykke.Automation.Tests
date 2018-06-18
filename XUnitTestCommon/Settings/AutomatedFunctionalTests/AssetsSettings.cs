using Lykke.SettingsReader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests
{
    public class AssetsSettings : IAppSettings
    {
        public String UrlPefix { get; set; }
        public String BaseUrl { get; set; }
        public bool IsHttps { get; set; }
        [Optional] public String BaseUrlAuth { get; set; }
        [Optional] public String BaseUrlRegister { get; set; }
        [Optional] public String AuthPath { get; set; }
        [Optional] public String AuthClientId { get; set; }
        [Optional] public String AuthEmail { get; set; }
        [Optional] public String AuthPassword { get; set; }
        [Optional] public String AuthClientInfo { get; set; }
        [Optional] public String AuthPartnerId { get; set; }
        public int AuthTokenTimeout { get; set; }
        public String DictionariesConnectionString { get; set; }
        public String MainConnectionString { get; set; }
        public String DictionaryCacheExpirationSeconds { get; set; }
        [Optional] public String RegisterPath { get; set; }
    }
}
