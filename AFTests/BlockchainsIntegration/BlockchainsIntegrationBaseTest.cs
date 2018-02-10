using BlockchainsIntegration.BlockchainWallets;
using BlockchainsIntegration.Api;
using BlockchainsIntegration.Sign;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;
using Newtonsoft.Json;
using System.IO;

namespace AFTests.BlockchainsIntegrationTests
{
    class BlockchainsIntegrationBaseTest : BaseTest
    {

       protected static string SpecificBlockchain()
        {
            return Environment.GetEnvironmentVariable("BlockchainIntegration") ?? "Zcash";// "Dash"; "Litecoin";
        }

        private static BlockchainSpecificModel _settings;

        protected static BlockchainSpecificModel BlockchainSpecificSettings()
        {
            if (_settings != null)
                return _settings;

            if (File.Exists(TestContext.CurrentContext.WorkDirectory + "\\properties.json"))
            {
                _settings = LocalConfig.LocalConfigModel();
                return _settings;
            }

            if (SpecificBlockchain().ToLower() == "litecoin")
                _settings = new LitecoinSettings();

            if (SpecificBlockchain().ToLower() == "dash")
                _settings = new DashSettings();

            if (SpecificBlockchain().ToLower() == "zcash")
                _settings = new ZcashSettings();

            return _settings;
        }

        protected static string CurrentAssetId()
        {
            return BlockchainSpecificSettings().AssetId;
        }

        protected BlockchainApi blockchainApi = new BlockchainApi(BlockchainSpecificSettings().BlockchainApi);
        protected BlockchainSign blockchainSign = new BlockchainSign(BlockchainSpecificSettings().BlockchainSign);
        protected BlockchainWallets blockchainWallets = new BlockchainWallets();

        protected static string HOT_WALLET = new BlockchainSign(BlockchainSpecificSettings().BlockchainSign).PostWallet().GetResponseObject().PublicAddress;
        protected static string WALLET_ADDRESS = BlockchainSpecificSettings().WalletAddress;
        protected static string PKey = BlockchainSpecificSettings().WalletKey;

        protected static string WALLET_SINGLE_USE = BlockchainSpecificSettings().WalletSingleUse;
        protected static string KEY_WALLET_SINGLE_USE = BlockchainSpecificSettings().WalletSingleUseKey;

        protected static string CLIENT_ID = BlockchainSpecificSettings().ClientId;
        //fill here http://faucet.thonguyen.net/ltc
    }

    public abstract class BlockchainSpecificModel
    {
        public string BlockchainIntegration { get; set; }
        public string BlockchainApi { get; set; }
        public string BlockchainSign { get; set; }
        public string WalletsUrl { get; set; }
        public string WalletAddress { get; set; }
        public string WalletKey { get; set; }
        public string WalletSingleUse { get; set; }
        public string WalletSingleUseKey { get; set; }
        public string ClientId { get; set; }
        public string AssetId { get; set; }
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

    public class LocalConfig : BlockchainSpecificModel
    {
        public static BlockchainSpecificModel LocalConfigModel()
        {
            return JsonConvert.DeserializeObject<LocalConfig>(File.ReadAllText(TestContext.CurrentContext.WorkDirectory + "\\properties.json")); ;
        }

    }
}
