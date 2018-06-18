using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;

namespace XUnitTestCommon.Consumers
{
    public class BaseUrls
    {
        public String ApiV2BaseUrl { get; set; }
        public String BlueApiBaseUrl { get; set; }
        public String ExchangeOperationsBaseUrl { get; set; }
        public String ClientAccountApiBaseUrl { get; set; }
        public String SessionApiBaseUrl { get; set; }
        public String AssetsApiBaseUrl { get; set; }
        public String AlgoStoreApiBaseUrl { get; set; }
        public String RegistrationApiBaseUrl { get; set; }
        public String BalancesApiBaseUrl { get; set; }

        public BaseUrls(ConfigBuilder config)
        {
            ApiV2BaseUrl = config.Config["ApiV2BaseUrl"];
            BlueApiBaseUrl = config.Config["BlueApiBaseUrl"];
            ExchangeOperationsBaseUrl = config.Config["ExchangeOperationsBaseUrl"];
            ClientAccountApiBaseUrl = config.Config["ClientAccountApiBaseUrl"];
            SessionApiBaseUrl = config.Config["SessionApiBaseUrl"];
            AssetsApiBaseUrl = config.Config["AssetsApiBaseUrl"];
            AlgoStoreApiBaseUrl = config.Config["AlgoStoreApiBaseUrl"];
            RegistrationApiBaseUrl = config.Config["RegistrationApiBaseUrl"];
            BalancesApiBaseUrl = config.Config["BalancesApiBaseUrl"];
        }

        public BaseUrls(ServicesSettings serviceSettings)
        {
            ApiV2BaseUrl = serviceSettings.ApiV2BaseUrl;
            BlueApiBaseUrl = serviceSettings.BlueApiBaseUrl;
            ExchangeOperationsBaseUrl = serviceSettings.ExchangeOperationsBaseUrl;
            ClientAccountApiBaseUrl = serviceSettings.ClientAccountApiBaseUrl;
            SessionApiBaseUrl = serviceSettings.SessionApiBaseUrl;
            AssetsApiBaseUrl = serviceSettings.AssetsApiBaseUrl;
            AlgoStoreApiBaseUrl = serviceSettings.AlgoStoreApiBaseUrl;
            RegistrationApiBaseUrl = serviceSettings.RegistrationApiBaseUrl;
            BalancesApiBaseUrl = serviceSettings.BalancesApiBaseUrl;
        }
    }
}
