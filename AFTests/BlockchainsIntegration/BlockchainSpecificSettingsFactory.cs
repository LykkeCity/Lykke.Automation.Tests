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
                _settings = LocalConfig.LocalConfigModel();
                return _settings;
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
                WalletAddress = "msvNWBpFNDQ6JxiEcTFU3xXbSnDir4EqCk";
                WalletKey = "cRTB3eAajJchgNuybhH5SwC9L5PFoTwxXBjgB8vRNJeJ4EpcXmAP";
                WalletSingleUse = "mvErcbPuL4T4kxbJYejk6xLbv8pfBiiSPu";
                WalletSingleUseKey = "cNn38kw6LSfAS6WvJJbFTqWRewa1GgfwczftXrBcyAmygM1V7qKr";
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
                WalletAddress = "yUDQmubM2HtBFmkvbSK1rER1t57M5Mcvng";
                WalletKey = "cPX3K2xfuzoakmXMaJG5HrdFKuACxegcax5eq55SMHJ8YxvmttZz";
                WalletSingleUse = "yiH2MLsx6bVZFgQ9qQNj5QeetGft8xDacC";
                WalletSingleUseKey = "cQinitZ5SkZdARZPuXicFgGkKepWcjpB5fTx5WKHdvkMTdnB1yrq";
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
                WalletAddress = "tmQjumT79zunETQgFxNTEMUKz8D841fMCf1";
                WalletKey = "cRPW3spyP9riDJWniNpcbDkiBjpLrhneSh2qTs3uSZUbm4HZLEyB";
                WalletSingleUse = "tmCiRyHFZYeRyXV1wqyiqZad2Zf9ifdR9H5";
                WalletSingleUseKey = "cVhrfsddvvhJhEEoqocSpdwZjJrTdQmHdWRKyvSqhyYvBDFD4die";
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
                WalletKey = "snfFfwhb7tobJcamWtH6GhvvhM6pi";
                WalletAddress = "rwAC8DbqubkRcYKuQQaqSBYppHD4JU84Pa";
                WalletSingleUse = "rwAC8DbqubkRcYKuQQaqSBYppHD4JU84Pa";
                WalletSingleUseKey = "snfFfwhb7tobJcamWtH6GhvvhM6pi";
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
                WalletKey = "SA7W5C7CQOOJF2AGFL2B2LC7VM6WMAONYTAWVITGAKOJM757YDG4VOQG";
                WalletAddress = "GDGZG75SP7UVW6RRDNMFGCFFC5D5RZTXLAWYWUKYCHJ6SBJ2FTXFLXHA";
                WalletSingleUse = "GDGZG75SP7UVW6RRDNMFGCFFC5D5RZTXLAWYWUKYCHJ6SBJ2FTXFLXHA";
                WalletSingleUseKey = "SA7W5C7CQOOJF2AGFL2B2LC7VM6WMAONYTAWVITGAKOJM757YDG4VOQG";
                ClientId = "b623b171-a307-4485-897c-f3a70b763217";
                AssetId = "XRP";
            }
        }

        class BitsharesSettings : BlockchainSpecificModel
        {
            public BitsharesSettings()
            {
                BlockchainApi = "http://lykke-service-bitshares-api.autotests-service.svc.cluster.local/api";
                BlockchainSign = "http://lykke-service-bitshares-sign.lykke-service.svc.cluster.local/api";
                WalletsUrl = null;
                WalletKey = "";
                WalletAddress = "";
                WalletSingleUse = "";
                WalletSingleUseKey = "";
                ClientId = "";
                AssetId = "";
            }
        }

        public class LocalConfig : BlockchainSpecificModel
        {
            public static BlockchainSpecificModel LocalConfigModel()
            {
                var json = File.ReadAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "properties.json"));
                TestContext.Progress.WriteLine($"properties.json: {json}");
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
        public string WalletsUrl { get; set; }
        public string WalletAddress { get; set; }
        public string WalletKey { get; set; }
        public string WalletSingleUse { get; set; }
        public string WalletSingleUseKey { get; set; }
        public string ClientId { get; set; }
        public string AssetId { get; set; }
    }
}
