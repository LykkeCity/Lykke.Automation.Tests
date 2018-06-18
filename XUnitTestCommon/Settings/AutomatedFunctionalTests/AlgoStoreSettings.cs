using Lykke.SettingsReader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests
{
    public class AlgoStoreSettings : IAppSettings
    {
        public String UrlPefix { get; set; }
        public String BaseUrl { get; set; }
        public bool IsHttps { get; set; }
        public String MainConnectionString { get; set; }
        public String BaseUrlAuth { get; set; }
        [Optional] public String BaseUrlRegister { get; set; }
        public String AuthPath { get; set; }
        public String AuthEmail { get; set; }
        public String AuthPassword { get; set; }
        public int AuthTokenTimeout { get; set; }
        public String SessionServiceUrl { get; set; }
        public String LogsConnString { get; set; }
        [Optional] public String RegisterPath { get; set; }
    }
}
