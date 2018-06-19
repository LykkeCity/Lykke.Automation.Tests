using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings.AutomatedFunctionalTests
{
    public class BlueApiSettings : IAppSettings
    {
        public String UrlPefix { get; set; }
        public String BaseUrl { get; set; }
        public String BaseUrlAuth { get; set; }
        public String BaseUrlRegister { get; set; }
        public bool IsHttps { get; set; }
        public String AuthPath { get; set; }
        public String RegisterPath { get; set; }
        public String AuthClientId { get; set; }
        public String AuthEmail { get; set; }
        public String AuthPassword { get; set; }
        public String AuthClientInfo { get; set; }
        public String AuthPartnerId { get; set; }
        public int AuthTokenTimeout { get; set; }
        public String DictionariesConnectionString { get; set; }
        public String MainConnectionString { get; set; }
        public String DictionaryCacheExpirationSeconds { get; set; }
        public String PledgeCreateAuthEmail { get; set; }
        public String PledgeUpdateAuthEmail { get; set; }
        public String AuthPledgeUpdateClientId { get; set; }
        public String AuthPledgeDeleteClientId { get; set; }
        public String AuthPledgeCreateClientId { get; set; }
        public String PledgeDeleteAuthEmail { get; set; }
        public String TwitterAccountEmail { get; set; }
        public bool TwitterAggressiveCheck { get; set; }
        public String LykkeBluePartnerId { get; set; }
        public String TestPartnerId { get; set; }
        public String BalancesServiceUrl { get; set; }
    }
}
