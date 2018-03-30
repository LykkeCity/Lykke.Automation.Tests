using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XUnitTestCommon.TestsCore;

namespace AFTests.BlockchainsIntegration
{
    public class BlockchainSpecificSettingsFactory
    {
        private static BlockchainSpecificModel _settings;

        public static BlockchainSpecificModel BlockchainSpecificSettings(string blockchain)
        {
            if (_settings != null)
                return _settings;

            if (File.Exists(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")))
            {
                try
                {
                    _settings = LocalConfig.LocalConfigModel();
                }catch(Exception e)
                {
                    TestContext.Progress.WriteLine("An error while parsing settings from properties.json");
                    TestContext.Progress.WriteLine(e);
                    TestContext.Progress.WriteLine(File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json")));
                }
                if (!string.IsNullOrEmpty(_settings?.BlockchainApi))
                {
                    TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");
                    return _settings;
                }  
                else
                    TestContext.Progress.WriteLine("properties.json is present but api url is null or empty");
            }

            blockchain = blockchain.ToLower();

            if (blockchain == "litecoin")
                _settings = new LitecoinSettings();

            if (blockchain == "dash")
                _settings = new DashSettings();

            if (blockchain == "zcash")
                _settings = new ZcashSettings();

            if (blockchain == "ripple")
                _settings = new RippleSettings();

            if (blockchain == "stellar")
                _settings = new StellarSettings();

            if (blockchain == "stellar-v2")
                _settings = new StellarV2Settings();

            if (blockchain == "bitshares")
                _settings = new BitsharesSettings();

            TestContext.Progress.WriteLine($"propeties.json: {JsonConvert.SerializeObject(_settings)}");

            return _settings;
        }


        class LitecoinSettings : BlockchainSpecificModel
        {
            public LitecoinSettings()
            {
                BlockchainApi = "http://litecoin-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://litecoin-sign.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                DepositWalletAddress = "msvNWBpFNDQ6JxiEcTFU3xXbSnDir4EqCk";
                DepositWalletKey = "cRTB3eAajJchgNuybhH5SwC9L5PFoTwxXBjgB8vRNJeJ4EpcXmAP";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "LTC";
            }
        }

        class DashSettings : BlockchainSpecificModel
        {
            public DashSettings()
            {
                BlockchainApi = "http://dash-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://dash-sign.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                DepositWalletAddress = "yUDQmubM2HtBFmkvbSK1rER1t57M5Mcvng";
                DepositWalletKey = "cPX3K2xfuzoakmXMaJG5HrdFKuACxegcax5eq55SMHJ8YxvmttZz";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "DASH";
            }
        }

        class ZcashSettings : BlockchainSpecificModel
        {
            public ZcashSettings()
            {
                BlockchainApi = "http://zcash-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://zcash-sign-service.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                DepositWalletAddress = "tmQjumT79zunETQgFxNTEMUKz8D841fMCf1";
                DepositWalletKey = "cRPW3spyP9riDJWniNpcbDkiBjpLrhneSh2qTs3uSZUbm4HZLEyB";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "ZEC";
            }
        }

        class RippleSettings : BlockchainSpecificModel
        {
            public RippleSettings()
            {
                BlockchainApi = "http://ripple-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://ripple-sign-service.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                DepositWalletKey = "snfFfwhb7tobJcamWtH6GhvvhM6pi";
                DepositWalletAddress = "rwAC8DbqubkRcYKuQQaqSBYppHD4JU84Pa";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XRP";
            }
        }

        class StellarSettings : BlockchainSpecificModel
        {
            public StellarSettings()
            {
                BlockchainApi = "http://stellar-api.lykke-service.svc.cluster.local/api";
                BlockchainSign = "http://stellar-sign-service.lykke-service.svc.cluster.local/api";
                WalletsUrl = null;
                DepositWalletKey = "SA7W5C7CQOOJF2AGFL2B2LC7VM6WMAONYTAWVITGAKOJM757YDG4VOQG";
                DepositWalletAddress = "GDGZG75SP7UVW6RRDNMFGCFFC5D5RZTXLAWYWUKYCHJ6SBJ2FTXFLXHA";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XLM";
            }
        }

        class StellarV2Settings : BlockchainSpecificModel
        {
            public StellarV2Settings()
            {
                BlockchainIntegration = "Stellar";
                BlockchainApi = "http://stellar-api-schnidlo.lykke-service.svc.cluster.local/api";
                BlockchainSign = "http://stellar-sign-service-schnidlo.lykke-service.svc.cluster.local/api";
                HotWallet = "GCCWY6MNVWORHO7B3L6W5LULGFR337K5UAATA2Z3FSPQA5MYRW5X33ZP";
                HotWalletKey = "SBBEPWMAINBQKQFW42L6GTWF73WPP5VRVG6KZHCWZVSVM4DUMWZESK2Z";
                WalletsUrl = null;
                DepositWalletKey = "SA7W5C7CQOOJF2AGFL2B2LC7VM6WMAONYTAWVITGAKOJM757YDG4VOQG";
                DepositWalletAddress = "GDGZG75SP7UVW6RRDNMFGCFFC5D5RZTXLAWYWUKYCHJ6SBJ2FTXFLXHA";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XLM";
                ExternalWalletAddress = "GDTCLM7L3RKSP324DBRROKRLTRU4WLSPLHDEVSC75LAVZFN6ZDQPK7LD";
                ExternalWalletKey = "SCCSEPSZNHAIJXUVVHAN7ZHBSBSTBJY5Y2W7CJYIP3FALGAFEV5FEGMQ";
            }
        }

        class BitsharesSettings : BlockchainSpecificModel
        {
            public BitsharesSettings()
            {
                BlockchainIntegration = "Bitshares";
                BlockchainApi = "http://lykke-service-bitshares-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://lykke-service-bitshares-sign.autotests-service.svc.cluster.local/api";
                WalletsUrl = null;
                DepositWalletKey = "keep_keys_private";
                DepositWalletAddress = "1.2.20477:::23088da2-9a5f-42de-bdd6-fe5ee259c8cd";
                HotWallet = "1.2.20477:::e91757da-6a69-43f4-8283-b2612703af59";
                HotWalletKey = "keep_keys_private";
                ExternalWalletAddress = "1.2.20407:::06eea045-43ee-4cca-a19d-1356abc2b70e";
                ExternalWalletKey = "5JonSsFtX5XD15rBzFCnx46DQMmhWYmSjgJPMPuZGwY9ne86xJ6";
                ClientId = "";
                AssetId = "1.3.0";
                ExternalWallerAddressContext = "5KJBVnJaiYhVq7x3mF47f5xd6RUisnqjWCdc5fx9uhWSDdrd1MR";
            }
        }

        public class LocalConfig : BlockchainSpecificModel
        {
            public static BlockchainSpecificModel LocalConfigModel()
            {
                var json = File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json"));
                return JsonConvert.DeserializeObject<LocalConfig>(json);
            }
        }
    }

    public abstract class BlockchainSpecificModel
    {
        public string BlockchainIntegration { get; set; }
        public string BlockchainApi { get; set; }
        public string BlockchainSign { get; set; }
        public string HotWallet { get; set; }
        public string HotWalletKey { get; set; }
        public string WalletsUrl { get; set; }
        public string DepositWalletAddress { get; set; }
        public string DepositWalletKey { get; set; }
        public string ClientId { get; set; }
        public string AssetId { get; set; }
        public string ExternalWalletAddress { get; set; }
        public string ExternalWalletKey { get; set; }
        public string ExternalWallerAddressContext { get; set; }
    }
}
